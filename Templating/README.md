# ConsoleExtensions Templating
This project will allow you to simplify writing complex information to the console.
## Initialization
```C#
    var proxy = new ConsoleProxy();

    var suggestions = new List<string>() { "Initialize", "Load", "Save", "Create", "Read", "Update", "Delete", "Quit" };

    proxy.WriteTemplate("[hr/]Try writing one of the [c:white]{Count}[/] recognized commands:"
                        + "[br/][foreach] {} [/][hr/]", suggestions);
```
will result in
![output](Output.PNG)
## Supported options
- **Content**
  - *Raw text* : Outputs what ever the text is. Escape '{' with '{{' and '[' with '[['.
  - *Substitution* : Text wrapped in **\{ \}** will be substituted using the matching property in the given object.
    - Given '**\{ \}**' the object itselv will be rendered.
  - *Horizontal ruler* : **[hr/]** will be replaced with a vertical line of '-'.
  - *Line break* : **[br/]** will be replaced with a line break.
  - *Clear Line* : **[clearline/]** will fill the remainder of the line with whitespace characters.
- **Styling**
  - *Color change* : **[c:\<color\>]** content **[/]** will cause the font color to change to the specified color while writing the content. 
    - The standard console colors are supported (Black , DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkMagenta, DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta, Yellow & White)
  - *Style change* : **[s:\<style\>]** content **[/]** will switch front and back color to match the specified style for the content.
- **Flow Control**
  - *If* : **[if:\<property>]** content **[/]** if the value in the property is truthy the content is rendered.
  - *Ifnot* : **[ifnot:\<property>]** content **[/]** if the value in the property is **not** truthy the content is rendered.
- **Object traverals**
  - *With* : **[With:\<property\>]** content **[/]** changes the object used for substitution to the specified property.
  - *Enumeration* : **[foreach:\<property\>]** content **[/]** repeats the content for each object in the enumerable property.
