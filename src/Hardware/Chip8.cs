using Hardware.CPU;
using Hardware.Input.Keyboard;
using Hardware.Output.Display;
using Hardware.RAM;
using Hardware.Util;
using Microsoft.Extensions.Logging;

namespace Hardware;

/// <summary>
/// Represents the Chip8 hardware. Use this class to interact with the Chip8.
/// </summary>
public class Chip8
{
    /// <summary>
    /// The CPU of the Chip8.
    /// </summary>
    private Cpu _cpu;

    /// <summary>
    /// The Keyboard adapter of the Chip8.
    /// </summary>
    private KeyboardAdapter _keyboardAdapter;

    /// <summary>
    /// The Display adapter of the Chip8.
    /// </summary>
    private DisplayAdapter _displayAdapter;

    /// <summary>
    /// The Memory RAM of the Chip8.
    /// </summary>
    private MemoryRam _memoryRam;

    /// <summary>
    /// Flag to enable or disable the CPU.
    /// </summary>
    private bool _isRunning;

    private readonly ILogger<Chip8> _logger;

    public Chip8()
    {
        _logger = Logger.Instance.GetLogger<Chip8>();

        _memoryRam = new MemoryRam();
        _displayAdapter = new DisplayAdapter();
        _keyboardAdapter = new KeyboardAdapter();
        _cpu = new Cpu(_memoryRam, _displayAdapter, _keyboardAdapter);
    }

    public void LoadProgram(byte[] program)
    {
        try
        {
            _memoryRam.LoadProgram(program);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error loading program");
            throw;
        }

    }

    public void SetMemory(ushort address, byte value)
    {
        try
        {
            _memoryRam[address] = value;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error changing memory");
        }
    }

    public void Start()
    {
        try
        {
            _isRunning = true;
            while (_isRunning)
                _cpu.Step();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Error running the CPU");
            throw;
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public DisplayAdapter Display => _displayAdapter;

    public KeyboardAdapter Keyboard => _keyboardAdapter;

}
