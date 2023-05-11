namespace ConsoleExtensions.Proxy;

/// <summary>
/// Struct CursorPoint
/// </summary>
public struct CursorPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CursorPoint"/> struct.
    /// </summary>
    /// <param name="top">The top.</param>
    /// <param name="left">The left.</param>
    public CursorPoint(int top, int left)
    {
        this.Top = top;
        this.Left = left;
    }

    /// <summary>
    /// Gets the top distance from the top of the console window.
    /// </summary>
    /// <value>The top.</value>
    public int Top { get; }

    /// <summary>
    /// Gets the left distance from the left side of the console window.
    /// </summary>
    /// <value>The left.</value>
    public int Left { get; }

    /// <summary>
    /// Moves the cursor to the left.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>A new point with the new position.</returns>
    public CursorPoint MoveLeft(int offset)
    {
        return new CursorPoint(this.Top, this.Left + offset);
    }
}