using System.ComponentModel;
using ConsoleExtensions.Commandline;
using ConsoleExtensions.Proxy;
using Demo;

Controller.Run(new Toolbox(), args);

namespace Demo
{
    public class Toolbox
    {
        /// <summary>
        ///     Gets or sets the source folder.
        /// </summary>
        /// <value>
        ///     The source folder.
        /// </value>
        [Description("Folder to copy files from.")]
        public string? SourceFolder { get; set; }

        /// <summary>
        ///     Gets or sets the target folder.
        /// </summary>
        /// <value>
        ///     The target folder.
        /// </value>
        [Description("Folder to copy files to.")]
        public string? TargetFolder { get; set; }

        /// <summary>
        ///     Copies the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        ///     A status message.
        /// </returns>
        [Description("Copy the files from source to destination.")]
        public string Copy(string filter = "*")
        {
            // your logic here
            return "Some files was copied";
        }

        public async Task<string> MoveAsync(CancellationToken token)
        {
            for (var i = 0; i < 100 && !token.IsCancellationRequested; i++)
            {
                ConsoleProxy.Instance().Write(".");
                await Task.Delay(1000, token);
            }
        
            return "Async return";
        }
    }
}