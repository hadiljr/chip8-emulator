namespace Hardware.CPU;

/// <summary>
/// The stack of the CPU.
/// </summary>
public class Stack
{

    /// <summary>
    /// The stack pointer register of the CPU.
    /// </summary>
    private byte _sp;

    /// <summary>
    /// The stack of the CPU. 16 levels of stack.
    /// </summary>
    private ushort[] _stack = new ushort[16];

    /// <summary>
    /// Constructor of the stack.
    /// </summary>
    /// <param name="spRegister">SP register from CPU</param>
    public Stack(ref byte spRegister)
    {
        _sp = spRegister;
    }

    /// <summary>
    /// Pop value from the stack.
    /// </summary>
    /// <returns></returns>
    public ushort Pop()
    {
        _sp--;
        return _stack[_sp];
    }

    /// <summary>
    /// Push value to the stack.
    /// </summary>
    /// <param name="data"></param>
    public void Push(ushort data)
    {
        _stack[_sp] = data;
        _sp++;
    }

}
