
namespace Hardware.RAM;

/// <summary>
/// The memory ram of the Chip8.
/// </summary>
public class MemoryRam
{
    /// <summary>
    /// The start address of the program.
    /// </summary>
    public const ushort PROGRAM_START_ADDRESS = 0x200;

    private const ushort SIZE_OF_MEMORY = 0xFFF;//4095

    private const ushort MAX_PROGRAM_SIZE = SIZE_OF_MEMORY - PROGRAM_START_ADDRESS;

    public MemoryRam()
    {
        InitDefaultSprites();
    }

    private byte[] _memory = new byte[SIZE_OF_MEMORY]; //4kb of memory

    public byte this[ushort address]
    {
        get => _memory[address];
        set => _memory[address] = value;
    }

    /// <summary>
    /// Load the default sprites into the memory.
    /// </summary>
    private void InitDefaultSprites()
    {
        byte[] sprites =
        [
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        ];

        for (int i = 0; i < sprites.Length; i++)
        {
            _memory[i] = sprites[i];
        }


    }

    /// <summary>
    /// Load the program into the memory.
    /// </summary>
    /// <param name="program"></param>
    internal void LoadProgram(byte[] program)
    {
        if(program.Length > MAX_PROGRAM_SIZE)
        {
            throw new ArgumentException($"The program is too big. The maximum size is {MAX_PROGRAM_SIZE} bytes.");
        }
        
        for (int i = 0; i < program.Length; i++)
        {
            _memory[PROGRAM_START_ADDRESS + i] = program[i];
        }
    }
}
