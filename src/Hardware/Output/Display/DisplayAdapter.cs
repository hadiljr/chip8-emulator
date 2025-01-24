namespace Hardware.Output.Display;

/// <summary>
/// The Display adapter. Represents the display of the Chip8.
/// </summary>
public class DisplayAdapter
{
    /// <summary>
    /// Represent 64x32 pixels.
    /// </summary>
    private byte[,] _display = new byte[64, 32];

    public byte this[int x, int y]
    {
        get => _display[x, y];
        set => _display[x, y] = value;
    }

    /// <summary>
    /// Clear the display.
    /// </summary>
    public void Clear()
    {
        _display = new byte[64, 32];
    }
}
