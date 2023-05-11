# ConsoleExtensions.Commandline

Wrap a toolbox object in a command-line interface in jus a few lines of code.

```csharp
    using ConsoleExtensions.Commandline;

    Controller.Run(new Toolbox());
```

Example Toolbox class

```csharp
    public class Toolbox
    {
        [Description("Folder to copy files from.")]
        public string SourceFolder { get; set; }

        [Description("Folder to copy files to.")]
        public string TargetFolder { get; set; }

        [Description("Copy the files from source to destination.")]
        public string Copy(string filter = "*")
        {
            /// your logic here
            return "Some files was copied";
        }
    }
```

Will give you a commandline interface with the following options.

- **demo**  will display help
- **demo Help copy**  will display help for the copy command
- **demo Copy \*.png -SourceFolder "somePics" -TargetFolder "otherPics"** will invoke the copy method

![Toolbox](Documentation/ToolBox.PNG)