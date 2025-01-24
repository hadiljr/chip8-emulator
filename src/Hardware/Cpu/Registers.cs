using System;

namespace Hardware.CPU;

public class Registers
{
        /// <summary>
        /// 16 8-bit registers
        /// </summary>
        public byte[] V = new byte[16];

        /// <summary>
        /// The delay timer is active whenever the delay timer register (DT) is non-zero.
        /// </summary>
        public byte DT;

        /// <summary>
        /// The sound timer is active whenever the sound timer register (ST) is non-zero.
        /// </summary>
        public byte ST;

        /// <summary>
        /// The program counter (PC) is a 16-bit register that holds the currently executing address.
        /// </summary>
        public ushort PC;

        /// <summary>
        /// The stack pointer (SP) is an 8-bit register that holds the address of the current level on the stack.
        /// </summary>
        public byte SP;

        /// <summary>
        /// The index register (I) is a 16-bit register that holds the memory address of the current operation.
        /// </summary>
        public ushort I;

        public byte this[int index]
        {
            get { return V[index]; }
            set { V[index] = value; }
        }
}
