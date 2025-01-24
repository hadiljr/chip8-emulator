using System;

namespace Hardware.Input.Keyboard;

/// <summary>
/// The Keyboard adapter.
/// Use this class to handle the keyboard input.
/// </summary>
public class KeyboardAdapter
{
    /// <summary>
    /// Value that represents no key pressed.
    /// </summary>
    public const byte NO_KEY_PRESSED = 0xFF;

    /// <summary>
    /// The keys of the keyboard.
    /// </summary>
    private bool[] _keys = new bool[16];


    public bool this[int key]
    {
        get => _keys[key];
        set => _keys[key] = value;
    }

    /// <summary>
    /// Check if a key is pressed.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsKeyPressed(int key)
    {
        return _keys[key];
    }

    /// <summary>
    /// Check if a key is pressed.
    /// </summary>
    /// <param name="key">Chip8Keys enum value</param>
    /// <returns></returns>
    public bool IsKeyPressed(Chip8Keys key)
    {
        return _keys[(int)key];
    }

    /// <summary>
    /// Set the key as pressed.
    /// </summary>
    /// <param name="key">Chip8Keys enum value</param>
    public void KeyDown(Chip8Keys key)
    {
        _keys[(int)key] = true;
    }

    /// <summary>
    /// Set the key as released.
    /// </summary>
    /// <param name="key">Chip8Keys enum value</param>
    public void KeyUp(Chip8Keys key)
    {
        _keys[(int)key] = false;
    }

    /// <summary>
    /// Check if any key is pressed.
    /// </summary>
    /// <returns>The key pressed or the constant value o <c>NO_KEY_PRESSED<c> (0xFF)</returns>
    public byte AnyKeysIsPressed()
    {

        for (int i = 0; i < 16; i++)
        {
            if (_keys[i])
            {
                return (byte)i;
            }
        }

        return NO_KEY_PRESSED;

    }
}
