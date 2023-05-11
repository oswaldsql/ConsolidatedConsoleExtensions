// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateParser.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating;

using System;
using System.Collections.Generic;

using Proxy;
using Renderers;
using Token;

/// <summary>
///     Class TemplateParser.
/// </summary>
public class TemplateParser
{
    /// <summary>
    ///     The command factory used for mapping command names to actions.
    /// </summary>
    private readonly CommandFactory factory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateParser" /> class.
    /// </summary>
    public TemplateParser()
    {
        this.factory = new CommandFactory();
        this.Style = new Dictionary<string, ConsoleStyle>(StringComparer.OrdinalIgnoreCase)
        {
            { "Default", ConsoleStyle.Default },
            { "Error", ConsoleStyle.Error },
            { "Info", ConsoleStyle.Info },
            { "Ok", ConsoleStyle.Ok },
            { "Warning", ConsoleStyle.Warning }
        };

        this.TypeTemplates = new Dictionary<Type, Template>();
        this.TypeConverters = new Dictionary<Type, Func<object, object>>();
    }

    /// <summary>
    ///     Gets the default template parser. Used if no template parser is specified.
    /// </summary>
    public static TemplateParser Default { get; } = new TemplateParser();

    /// <summary>
    ///     Gets the style mapped in the parser.
    /// </summary>
    public Dictionary<string, ConsoleStyle> Style { get; }

    /// <summary>
    ///     Gets the type converters mapped in the parser. Used before a object is rendered by a type template.
    /// </summary>
    public Dictionary<Type, Func<object, object>> TypeConverters { get; }

    /// <summary>
    ///     Gets the type templates used for applying specific templates to specific types of objects.
    /// </summary>
    internal Dictionary<Type, Template> TypeTemplates { get; }

    /// <summary>
    ///     Adds a type template.
    /// </summary>
    /// <typeparam name="T">The type to add a template to.</typeparam>
    /// <param name="templateString">The template string.</param>
    /// <param name="typeConverter">The type converter.</param>
    /// <exception cref="T:ConsoleExtensions.Templating.InvalidTemplateException">Thrown if the tokens dos not match up.</exception>
    public void AddTypeTemplate<T>(string templateString, Func<T, object> typeConverter = null)
    {
        if (templateString != null)
        {
            var template = this.Parse(templateString);
            this.TypeTemplates[typeof(T)] = template;
        }
        else
        {
            if (this.TypeTemplates.ContainsKey(typeof(T)))
            {
                this.TypeTemplates.Remove(typeof(T));
            }
        }

        if (typeConverter != null)
        {
            this.TypeConverters[typeof(T)] = obj => typeConverter((T)obj);
        }
    }

    /// <summary>
    ///     Parses the specified template string.
    /// </summary>
    /// <param name="templateString">The template string.</param>
    /// <returns>The resulting Template.</returns>
    /// <exception cref="T:ConsoleExtensions.Templating.InvalidTemplateException">Thrown if the tokens dos not match up.</exception>
    public Template Parse(string templateString)
    {
        var tokens = new Tokenizer().Tokenize(templateString).Optimize();

        var template = this.BuildTemplate(tokens);

        return template;
    }

    /// <summary>
    ///     Builds the template from a enumerable of tokens.
    /// </summary>
    /// <param name="tokens">The tokens.</param>
    /// <returns>The resulting Template.</returns>
    internal Template BuildTemplate(IEnumerable<Token.Token> tokens)
    {
        var result = new Template(this);

        var renderers = this.BuildRenderTree(result, tokens.GetEnumerator());

        result.RenderTree = new RootRenderer(renderers.ToArray());

        return result;
    }

    /// <summary>
    ///     Builds the renderer.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="tokens">The tokens.</param>
    /// <param name="token">The token.</param>
    /// <returns>A root Renderer serving af the staring point of the render.</returns>
    private Renderer BuildRenderer(Template template, IEnumerator<Token.Token> tokens, Token.Token token)
    {
        var renderer = this.factory.Create(template, token);
        if (!renderer.IsClosed)
        {
            var subRen = this.BuildRenderTree(template, tokens).ToArray();
            renderer.SubRenderes = subRen;
        }

        return renderer;
    }

    /// <summary>
    ///     Builds the render tree.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="tokens">The tokens.</param>
    /// <returns>A list of Renderer.</returns>
    private List<Renderer> BuildRenderTree(Template template, IEnumerator<Token.Token> tokens)
    {
        var renderers = new List<Renderer>();

        while (tokens.MoveNext())
        {
            var token = tokens.Current;
            if (token != null)
            {
                switch (token.Type)
                {
                    case TokenType.Text:
                    {
                        renderers.Add(new TextRenderer(token.Substring));
                        break;
                    }

                    case TokenType.Substitution:
                    {
                        renderers.Add(new SubstitutionRenderer(token.Substring, template));
                        break;
                    }

                    case TokenType.Command:
                    {
                        renderers.Add(this.BuildRenderer(template, tokens, token));
                        break;
                    }

                    case TokenType.EndCommand:
                    {
                        renderers.Add(new EndRenderer(token.Substring));
                        return renderers;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return renderers;
    }
}