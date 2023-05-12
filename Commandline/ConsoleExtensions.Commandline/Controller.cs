// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Controller.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("ConsoleExtensions.Commandline.Tests")]

namespace ConsoleExtensions.Commandline;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Arguments;
using Exceptions;
using Help;
using JetBrains.Annotations;
using Parser;
using Proxy;
using Templating;

/// <summary>
///     Class Controller. Takes a object and presents is as a command line interface.
/// </summary>
public class Controller
{
    /// <summary>
    ///     The result template. Used to present the result of a command.
    /// </summary>
    private readonly Template resultTemplate;

    /// <summary>
    ///     Initializes a new instance of the Controller class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="setup">
    ///     The setup. Optional overwrite of the extensions added to the
    ///     console. Is not specified the Default setup is applied.
    /// </param>
    public Controller(object model, Action<Controller> setup = null)
        : this(model, ConsoleProxy.Instance(), setup)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the Controller class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="proxy">The proxy.</param>
    /// <param name="setup">The setup. Optional overwrite of the extensions added to the console. Is not specified the Default setup is applied.</param>
    internal Controller([NotNull] object model, IConsoleProxy proxy, Action<Controller> setup = null)
    {
        this.Model = model ?? throw new ArgumentException("Model must be a initialized class", (nameof(model)));
        this.Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        this.TemplateParser = new TemplateParser();

        this.ModelMap = ModelParser.Parse(this.Model);

        this.DefaultSetup(this);
        setup?.Invoke(this);

        this.resultTemplate = this.TemplateParser.Parse("{}");
    }

    /// <summary>
    ///     Gets the model wrapped in the console.
    /// </summary>
    public object Model { get; }

    /// <summary>
    ///     Gets the model map used to map commands and options to methods
    ///     and properties.
    /// </summary>
    public ModelMap ModelMap { get; }

    /// <summary>
    ///     Gets the proxy used to serve as a output of the console.
    /// </summary>
    public IConsoleProxy Proxy { get; internal set; }

    /// <summary>
    ///     Gets the template parser used to present results of command and
    ///     exceptions.
    /// </summary>
    public TemplateParser TemplateParser { get; }

    /// <summary>
    /// Runs the model with specified setup.
    /// </summary>
    /// <typeparam name="T">Model type to construct.</typeparam>
    /// <param name="setup">The setup.</param>
    /// <returns>The exit code.</returns>
    public static int Run<T>(Action<Controller> setup = null) where T : new()
    {
        return Run(new T(), null, setup);
    }

    /// <summary>
    /// Runs the model with specified setup in a async task.
    /// </summary>
    public static Task<int> RunAsync<T>(Action<Controller> setup = null) where T : new()
    {
        return Task.FromResult(Run(new T(), null, setup));
    }

    /// <summary>
    ///     Instantiates a new controller with the model and standard setup and runs the arguments      against the model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="setup">The setup.</param>
    public static int Run(object model, string[] args = null, Action<Controller> setup = null)
    {
        return new Controller(model, setup).Run(args);
    }

    /// <summary>
    /// Gets or sets the command line argument provider.
    /// </summary>
    internal Func<string[]> ArgumentsProvider { get; set; } = () => Environment.GetCommandLineArgs().Skip(1).ToArray();

    /// <summary>
    ///     Runs the specified arguments against the controllers model.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public int Run(params string[] args)
    {
        if (args == null || args.Length == 0)
        {
            args = this.ArgumentsProvider();
        }
        if (args.Length == 0)
        {
            args = new[] { "Help" };
        }

        try
        {
            var arguments = ArgumentParser.Parse(args);

            this.ValidateArgumentsAgainstModel(arguments);

            foreach (var argument in arguments.Properties)
            {
                this.ModelMap.SetOption(argument.Key, argument.Value.ToArray());
            }

            var tokenSource = new CancellationTokenSource();
            this.Proxy.TreatControlCAsInput = false;
            this.Proxy.CancelKeyPress += (_, eventArgs) =>
            {
                tokenSource.Cancel();
                eventArgs.Cancel = true;
            };

            var result = this.ModelMap.Invoke(arguments.Command, tokenSource.Token, arguments.Arguments);

            this.Proxy.WriteTemplate(this.resultTemplate, result);
            return this.GetExitCode(result).Code;
        }
        catch (Exception e)
        {
            this.Proxy.WriteTemplate(this.resultTemplate, e);

            if (e is TargetInvocationException)
            {
                e = e.InnerException;
            }

            return this.GetExitCode(e).Code;
        }
    }

    /// <summary>
    ///     Applies the default setup to the controller.
    /// </summary>
    /// <param name="controller">The controller.</param>
    private void DefaultSetup(Controller controller)
    {
        controller.AddHelp().AddExceptionHandling().AddDefaultExitCodes();
    }

    /// <summary>
    ///     Validates the <paramref name="arguments" /> against model.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <exception cref="ConsoleExtensions.Commandline.Exceptions.UnknownOptionException">
    ///     Thrown when a requested options is unknown.
    /// </exception>
    /// <exception cref="ConsoleExtensions.Commandline.Exceptions.UnknownCommandException">
    ///     Thrown when a requested command is unknown.
    /// </exception>
    private void ValidateArgumentsAgainstModel(ParsedArguments arguments)
    {
        foreach (var argument in arguments.Properties)
        {
            if (!this.ModelMap.Options.TryGetValue(argument.Key, out _))
            {
                throw new UnknownOptionException(argument.Key, this.ModelMap.Options.Values);
            }
        }

        if (!this.ModelMap.Commands.TryGetValue(arguments.Command, out _))
        {
            throw new UnknownCommandException(arguments.Command, this.ModelMap.Commands.Values);
        }
    }

    /// <summary>
    ///     Gets the exit code.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    private ExitCode GetExitCode(object result)
    {
        return this.ExitCodes.OrderBy(o => o.Order).FirstOrDefault(o => o.Match(result));
    }

    /// <summary>
    /// Gets the exit codes.
    /// </summary>
    public List<ExitCode> ExitCodes { get; } = new ();
}
