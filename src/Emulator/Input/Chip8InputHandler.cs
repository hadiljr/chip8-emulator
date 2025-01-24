using System;
using System.Collections.Generic;
using Hardware;
using Hardware.Input.Keyboard;
using Microsoft.Xna.Framework.Input;

namespace Emulator.Input;

public class Chip8InputHandler
{
    private readonly Dictionary<Keys, Chip8Keys> _keyboardMap = new()
    {
        { Keys.D1, Chip8Keys.KEY_1 },
        { Keys.D2, Chip8Keys.KEY_2 },
        { Keys.D3, Chip8Keys.KEY_3 },
        { Keys.D4, Chip8Keys.KEY_C },
        { Keys.Q, Chip8Keys.KEY_4 },
        { Keys.W, Chip8Keys.KEY_5 },
        { Keys.E, Chip8Keys.KEY_6 },
        { Keys.R, Chip8Keys.KEY_D },
        { Keys.A, Chip8Keys.KEY_7 },
        { Keys.S, Chip8Keys.KEY_8 },
        { Keys.D, Chip8Keys.KEY_9 },
        { Keys.F, Chip8Keys.KEY_E },
        { Keys.Z, Chip8Keys.KEY_A },
        { Keys.X, Chip8Keys.KEY_0 },
        { Keys.C, Chip8Keys.KEY_B },
        { Keys.V, Chip8Keys.KEY_F }
    };

    public void HandleInput(Chip8 chip8)
    {
        foreach (var key in _keyboardMap)
        {
            if (Keyboard.GetState().IsKeyDown(key.Key))
            {
                chip8.Keyboard.KeyDown(key.Value);
            }
            else chip8.Keyboard.KeyUp(key.Value);
        }
    }
}
