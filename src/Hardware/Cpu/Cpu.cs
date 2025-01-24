using Hardware.Input.Keyboard;
using Hardware.Output.Display;
using Hardware.RAM;
using Hardware.Util;
using Microsoft.Extensions.Logging;

namespace Hardware.CPU;

public class Cpu
{

    /// <summary>
    /// The timer is used to decrease the delay timer and sound timer registers at a rate of 60Hz (60 times per second).
    /// </summary>
    private readonly System.Timers.Timer timer = new(1000 / 60);

    /// <summary>
    /// The registers of the CPU.
    /// </summary>
    private readonly Registers _registers = new();

    /// <summary>
    /// The instructions abstraction of the CPU.
    /// </summary>
    private readonly Instructions _instructions;

    /// <summary>
    /// The stack of the CPU.
    /// </summary>
    private readonly Stack _stack;

    /// <summary>
    /// The memory ram of the CPU.
    /// </summary>
    private readonly MemoryRam _memoryRam;

    /// <summary>
    /// The display adapter of the CPU.
    /// </summary>
    private readonly DisplayAdapter _displayAdapter;

    /// <summary>
    /// The keyboard adapter of the CPU.
    /// </summary>
    private readonly KeyboardAdapter _keyboardAdapter;

    public Cpu(MemoryRam memoryRam, DisplayAdapter displayAdapter, KeyboardAdapter keyboardAdapter)
    {
        _memoryRam = memoryRam;
        _displayAdapter = displayAdapter;
        _keyboardAdapter = keyboardAdapter;

        _registers = new Registers();
        _stack = new Stack(ref _registers.SP);
        _instructions = new Instructions(_registers, _stack, _memoryRam, _displayAdapter, _keyboardAdapter);

        Init();
    }

    /// <summary>
    /// Initialize the CPU.
    /// </summary>
    private void Init()
    {
        _registers.PC = MemoryRam.PROGRAM_START_ADDRESS;

        // Set the timer to decrease the delay timer and sound timer registers at a rate of 60Hz (60 times per second).
        timer.Elapsed += (sender, e) => HandleTimersRegisters();
        timer.Start();
    }

    /// <summary>
    /// One cycle of the CPU.
    /// </summary>
    public void Step()
    {
        // Fetch Instruction
        var opcode = FetchOpcode();

        // Execute Instruction
        _instructions.Execute(opcode);

        // Sleep for 2ms to simulate the 500Hz clock speed
        Thread.Sleep(2);
    }

    /// <summary>
    /// Fetches the opcode from the memory at the location specified by the program counter (pc).
    /// In our Chip 8 emulator, data is stored in an array in which each address contains one byte. 
    /// As one opcode is 2 bytes long, we will need to fetch two successive bytes and merge them to get the actual opcode.
    /// Also we increment the program counter by 2, setting it to the next instruction.
    /// </summary>
    /// <returns>opcode</returns>
    private ushort FetchOpcode()
    {
        byte firstByte = _memoryRam[_registers.PC];
        byte secondByte = _memoryRam[(ushort)(_registers.PC + 1)];
        ushort opcode = (ushort)(firstByte << 8 | secondByte);
        _registers.PC += 2;
        return opcode;
    }

    /// <summary>
    /// Decrease the delay timer and sound timer registers at a rate of 60Hz (60 times per second).
    /// </summary>
    private void HandleTimersRegisters()
    {
        _registers.DT = _registers.DT > 0 ? (byte)(_registers.DT - 1) : (byte)0;
        _registers.ST = _registers.ST > 0 ? (byte)(_registers.ST - 1) : (byte)0;
    }
}
