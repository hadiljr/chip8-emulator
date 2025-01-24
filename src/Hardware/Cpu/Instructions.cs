using Hardware.Input.Keyboard;
using Hardware.Output.Display;
using Hardware.RAM;

namespace Hardware.CPU;

/// <summary>
/// Abstraction for the instructions of the CPU.
/// </summary>
public class Instructions
{
    private readonly Registers _registers;
    private readonly Stack _stack;
    private readonly MemoryRam _memory;
    private readonly DisplayAdapter _display;
    private readonly KeyboardAdapter _keyboard;

    public Instructions(Registers registers, Stack stack, MemoryRam memory, DisplayAdapter display, KeyboardAdapter keyboard)
    {
        _registers = registers;
        _stack = stack;
        _memory = memory;
        _display = display;
        _keyboard = keyboard;
    }

    /// <summary>
    /// Execute the opcode.
    /// </summary>
    /// <param name="opcode">16-bit opcode</param>
    public void Execute(ushort opcode)
    {
        byte firstNibble = (byte)((opcode & 0xF000) >> 12);

        switch (firstNibble)
        {
            case 0x0:
                switch (opcode)
                {
                    case 0x00E0:
                        processCLS();
                        break;
                    case 0x00EE:
                        processRet();
                        break;
                    default:
                        processSysAddr(opcode);
                        break;
                }
                break;
            case 0x1:
                processJump(opcode);
                break;
            case 0x2:
                processCall(opcode);
                break;
            case 0x3:
                processSeVxByte(opcode);
                break;
            case 0x4:
                processSneVxByte(opcode);
                break;
            case 0x5:
                processSeVxVy(opcode);
                break;
            case 0x6:
                processLdVxByte(opcode);
                break;
            case 0x7:
                processAddVxByte(opcode);
                break;
            case 0x8:
                switch (opcode & 0x000F)
                {
                    case 0x0:
                        processLdVxVy(opcode);
                        break;
                    case 0x1:
                        processOrVxVy(opcode);
                        break;
                    case 0x2:
                        processAndVxVy(opcode);
                        break;
                    case 0x3:
                        processXorVxVy(opcode);
                        break;
                    case 0x4:
                        processAddVxVyCarry(opcode);
                        break;
                    case 0x5:
                        processSubVxVy(opcode);
                        break;
                    case 0x6:
                        processShrVx(opcode);
                        break;
                    case 0x7:
                        processSubnVxVy(opcode);
                        break;
                    case 0xE:
                        processShlVx(opcode);
                        break;
                }
                break;
            case 0x9:
                processSneVxVy(opcode);
                break;
            case 0xA:
                processLdIAddr(opcode);
                break;
            case 0xB:
                processJpV0Addr(opcode);
                break;
            case 0xC:
                processRndVxByte(opcode);
                break;
            case 0xD:
                processDrwVxVyNibble(opcode);
                break;
            case 0xE:
                switch (opcode & 0x00FF)
                {
                    case 0x9E:
                        processSkpVx(opcode);
                        break;
                    case 0xA1:
                        processSknpVx(opcode);
                        break;
                }
                break;
            case 0xF:
                switch (opcode & 0x00FF)
                {
                    case 0x07:
                        processLdVxDT(opcode);
                        break;
                    case 0x0A:
                        processLdVxK(opcode);
                        break;
                    case 0x15:
                        processLdDTVx(opcode);
                        break;
                    case 0x18:
                        processLdSTVx(opcode);
                        break;
                    case 0x1E:
                        processAddIVx(opcode);
                        break;
                    case 0x29:
                        processLdFVx(opcode);
                        break;
                    case 0x33:
                        processLdBVx(opcode);
                        break;
                    case 0x55:
                        processLdIVx(opcode);
                        break;
                    case 0x65:
                        processLdVxI(opcode);
                        break;
                }
                break;
        }
    }


    private void processSysAddr(ushort opcode)
    {
        processJump(opcode);
    }

    private void processCLS()
    {
        _display.Clear();
    }

    private void processRet()
    {
        _registers.PC = _stack.Pop();
    }

    private void processJump(ushort opcode)
    {
        ushort address = (ushort)(opcode & 0x0FFF);
        _registers.PC = address;
    }

    private void processCall(ushort opcode)
    {
        ushort address = (ushort)(opcode & 0x0FFF);
        _stack.Push(_registers.PC);
        _registers.PC = address;
    }

    private void processSeVxByte(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte value = (byte)(opcode & 0x00FF);
        if (_registers[register] == value)
        {
            _registers.PC += 2;
        }
    }

    private void processSneVxByte(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte value = (byte)(opcode & 0x00FF);
        if (_registers[register] != value)
        {
            _registers.PC += 2;
        }

    }

    private void processSeVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        if (_registers[registerX] == _registers[registerY])
        {
            _registers.PC += 2;
        }
    }

    private void processLdVxByte(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte value = (byte)(opcode & 0x00FF);
        _registers[register] = value;
    }

    private void processAddVxByte(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte value = (byte)(opcode & 0x00FF);
        if (_registers[register] + value > 255)
        {
            _registers[0xF] = 1;
        }

        _registers[register] += value;
    }

    private void processLdVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] = _registers[registerY];
    }

    private void processOrVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] |= _registers[registerY];
        _registers[0xF] = 0;
    }

    private void processAndVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] &= _registers[registerY];
        _registers[0xF] = 0;
    }

    private void processXorVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] ^= _registers[registerY];
        _registers[0xF] = 0;
    }

    private void processAddVxVyCarry(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        ushort result = (ushort)(_registers[registerX] + _registers[registerY]);
        _registers[registerX] = (byte)result;
        _registers[0xF] = (byte)(result > 0xFF ? 1 : 0);
    }

    private void processSubVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        byte vF = (byte)(_registers[registerX] >= _registers[registerY] ? 1 : 0);
        _registers[registerX] -= _registers[registerY];
        _registers[0xF] = vF;
    }

    private void processShrVx(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] = _registers[registerY];
        byte vF = (byte)(_registers[registerX] & 0x1);
        _registers[registerX] >>= 1;
        _registers[0xF] = vF;
    }

    private void processSubnVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        byte vF = (byte)(_registers[registerY] >= _registers[registerX] ? 1 : 0);
        _registers[registerX] = (byte)(_registers[registerY] - _registers[registerX]);
        _registers[0xF] = vF;
    }

    private void processShlVx(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        _registers[registerX] = _registers[registerY];
        byte vF = (byte)(_registers[registerX] >> 7);
        _registers[registerX] <<= 1;
        _registers[0xF] = vF;
    }

    private void processSneVxVy(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        if (_registers[registerX] != _registers[registerY])
        {
            _registers.PC += 2;
        }
    }

    private void processLdIAddr(ushort opcode)
    {
        ushort address = (ushort)(opcode & 0x0FFF);
        _registers.I = address;
    }

    private void processJpV0Addr(ushort opcode)
    {
        ushort address = (ushort)(opcode & 0x0FFF);
        _registers.PC = (ushort)(_registers[0] + address);
    }

    private void processRndVxByte(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte value = (byte)(opcode & 0x00FF);
        Random random = new Random();
        _registers[register] = (byte)(random.Next(0, 255) & value);
    }

    private void processDrwVxVyNibble(ushort opcode)
    {
        byte registerX = (byte)((opcode & 0x0F00) >> 8);
        byte registerY = (byte)((opcode & 0x00F0) >> 4);
        byte height = (byte)(opcode & 0x000F);
        byte x = _registers[registerX];
        byte y = _registers[registerY];
        _registers[0xF] = 0;


        // If the sprite is positioned outside the screen, wrap it around
        if (x >= 64 || y >= 32)
        {
            for (int yline = 0; yline < height; yline++)
            {
                byte pixel = _memory[(ushort)(_registers.I + yline)];

                for (int xline = 0; xline < 8; xline++)
                {
                    if ((pixel & (0x80 >> xline)) != 0)
                    {


                        var xCoord = (x + xline) % 64;
                        var yCoord = (y + yline) % 32;

                        if (_display[xCoord, yCoord] == 1)
                        {
                            _registers[0xF] = 1;
                        }

                        _display[xCoord, yCoord] ^= 1;
                    }
                }
            }
        }
        // Else the sprite is positioned inside the screen, clipping if necessary
        else
        {
            for (int yline = 0; yline < height; yline++)
            {
                byte pixel = _memory[(ushort)(_registers.I + yline)];

                for (int xline = 0; xline < 8; xline++)
                {
                    if ((pixel & (0x80 >> xline)) != 0)
                    {
                        if (x + xline >= 64 || y + yline >= 32)
                        {
                            continue;
                        }

                        if (_display[x + xline, y + yline] == 1)
                        {
                            _registers[0xF] = 1;
                        }

                        _display[x + xline, y + yline] ^= 1;
                    }
                }
            }
        }


    }

    private void processSkpVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        if (_keyboard.IsKeyPressed(_registers[register]))
        {
            _registers.PC += 2;
        }
    }

    private void processSknpVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        if (!_keyboard.IsKeyPressed(_registers[register]))
        {
            _registers.PC += 2;
        }
    }

    private void processLdVxDT(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _registers[register] = _registers.DT;
    }

    private void processLdVxK(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        byte key = _keyboard.AnyKeysIsPressed();
        if (key != KeyboardAdapter.NO_KEY_PRESSED)
            _registers[register] = key;
        else
            _registers.PC -= 2;
    }

    private void processLdDTVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _registers.DT = _registers[register];
    }

    private void processLdSTVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _registers.ST = _registers[register];
    }

    private void processAddIVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _registers.I += _registers[register];
    }

    private void processLdFVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _registers.I = (ushort)(_registers[register] * 5);
    }

    private void processLdBVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        _memory[_registers.I] = (byte)(_registers[register] / 100);
        _memory[(ushort)(_registers.I + 1)] = (byte)((_registers[register] / 10) % 10);
        _memory[(ushort)(_registers.I + 2)] = (byte)((_registers[register] % 100) % 10);
    }

    private void processLdIVx(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        for (ushort i = 0; i <= register; i++)
        {
            _memory[_registers.I] = _registers[i];
            _registers.I++;
        }
    }

    private void processLdVxI(ushort opcode)
    {
        byte register = (byte)((opcode & 0x0F00) >> 8);
        for (int i = 0; i <= register; i++)
        {
            _registers[i] = _memory[_registers.I];
            _registers.I++;
        }
    }

}
