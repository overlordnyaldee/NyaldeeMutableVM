using System;
using System.Collections.Generic;
using System.Text;

namespace MutableVM
{

    class Program
    {



        static void Main(string[] args)
        {

            // 0 1 2000 3000
            // | | |
            // | | Memory address
            // | Register to use
            // Type of instruction

            // 0 = load
            // 1 = store
            // 2 = add
            // 3 = subtract
            // 4 = multiply
            // 5 = divide
            // 6 = rem
            // 7 = jump-if-zero
            // 8 = jump-if equal
            // 9 = set register to constant

            // registers:
            // 0 1 2 3

            // Memory:
            // 0000
            // 4 digits. Addressable from 0 to 8192.

            // Constant:
            // 0000

            List<int> c = new List<int>(4);

            Machine m = new Machine();

            int type;
            int register;
            int memory;
            int constant;

            // Simple Fibonacci number calculator
            m.setMemoryAtLocation(0, 9000000001); // A = 1
            m.setMemoryAtLocation(1, 9100000001); // B = 1
            m.setMemoryAtLocation(2, 9200000001); // C = 1

            m.setMemoryAtLocation(3, 2000010000); // A = A + B
            m.setMemoryAtLocation(4, 1200009999); // C = A;

            m.setMemoryAtLocation(5, 2100000000); // B = B + A 
            m.setMemoryAtLocation(6, 1200019999); // C = B;

            m.setMemoryAtLocation(7, 8100000144); // if (B == 144) goto line 0
            m.setMemoryAtLocation(8, 7300030000); // goto line 3

            while (true)
            {
                Int64 instruction = m.getMemoryAtLocation((int)m.getRegister(3));
                Console.WriteLine("instruction: " + instruction);
                c = parseInstructionFromInteger(instruction);

                type = c[0];
                register = c[1];
                memory = c[2];
                constant = c[3];

                switch (c[0])
                {
                    case 0: // Load
                        m.setRegister(register, memory);
                        break;
                    case 1: // Store

                        if (constant == 0)
                        {
                            m.setMemoryFromRegister(register, memory);
                        }
                        else
                        {
                            m.setRegister(register, m.getRegister(memory));
                        }
                        break;
                    case 2:
                        // Add 
                        if (constant != 0)
                        {
                            m.setRegister(register, m.getRegister(register) + constant);
                        }
                        else
                        {
                            m.setRegister(register, m.getRegister(register) + m.getRegister(memory));
                        }
                        break;
                    case 3: // Subtract
                        m.setRegister(register, m.getRegister(register) - constant);
                        break;
                    case 4: // Multiply
                        m.setRegister(register, m.getRegister(register) * constant);
                        break;
                    case 5: // Divide
                        m.setRegister(register, m.getRegister(register) / constant);
                        break;
                    case 6: // Remainder
                        m.setRegister(register, m.getRegister(register) % constant);
                        break;
                    case 7: // jump
                        if (register == 3)
                        {
                            m.setRegister(3, memory - 1);
                        }
                        else
                        {
                            m.setRegister(3, m.getRegister(register - 1));
                        }

                        break;
                    case 8: // jump if equal
                        if (register != 3)
                        {
                            if (m.getRegister(register) == constant)
                            {
                                m.setRegister(3, memory - 1);
                            }
                        }
                        else
                        {
                            if (m.getRegister(register) == m.getRegister(constant))
                            {
                                m.setRegister(3, memory - 1);
                            }
                        }

                        break;
                    case 9: // Set register to constant
                        m.setRegister(register, constant);
                        break;
                }

                // Increment program counter
                m.setRegister(3, m.getRegister(3) + 1);

                Console.WriteLine(m.printRegisters());
                Console.ReadLine();
            }


        }

        static List<int> parseInstructionFromString(String a)
        {
            List<int> c = new List<int>(4);
            for (int i = 0; i < 3; i++)
            {
                if (i < 2)
                {
                    c.Add(Convert.ToInt32(a.Substring(i, 1)));
                }
                else
                {
                    c.Add(Convert.ToInt32(a.Substring(2, 4)));
                    c.Add(Convert.ToInt32(a.Substring(6)));
                }
            }

            return c;
        }

        static List<int> parseInstructionFromInteger(Int64 instruction)
        {
            String a = instruction.ToString("G");

            return parseInstructionFromString(a);
        }
    }

    class Machine
    {
        public const int MemorySize = 8192;

        private Int64 RegisterA;
        private Int64 RegisterB;
        private Int64 RegisterC;
        private Int64 RegisterPC;

        private List<Int64> Memory;

        public Machine()
        {
            clearAllRegisters();
            Memory = new List<Int64>(MemorySize);
            clearMemory();
        }

        public Int64 getRegister(int register)
        {
            switch (register)
            {
                case 0:
                    return RegisterA;
                case 1:
                    return RegisterB;
                case 2:
                    return RegisterC;
                case 3:
                    return RegisterPC;
                default:
                    throw new ArgumentException();
            }
        }

        public void setRegister(int register, Int64 data)
        {
            switch (register)
            {
                case 0:
                    RegisterA = data;
                    break;
                case 1:
                    RegisterB = data;
                    break;
                case 2:
                    RegisterC = data;
                    break;
                case 3:
                    RegisterPC = data;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public String printRegisters()
        {
            StringBuilder sb = new StringBuilder(12);
            sb.Append("PC: ");
            sb.Append(RegisterPC);
            sb.Append(" A: ");
            sb.Append(RegisterA);
            sb.Append(" B: ");
            sb.Append(RegisterB);
            sb.Append(" C: ");
            sb.Append(RegisterC);
            return sb.ToString();
        }

        public void clearRegister(int register)
        {
            switch (register)
            {
                case 0:
                    RegisterA = 0;
                    break;
                case 1:
                    RegisterB = 0;
                    break;
                case 2:
                    RegisterC = 0;
                    break;
                default:
                    throw new ArgumentException("register");
            }
        }

        public void clearAllRegisters()
        {
            RegisterA = 0;
            RegisterB = 0;
            RegisterC = 0;
        }

        public void setMemoryFromRegister(int register, int location)
        {
            Memory[location] = (int)getRegister(register);
        }

        public Int64 getMemoryAtLocation(int location)
        {
            return Memory[location];
        }

        public void setMemoryAtLocation(int location, Int64 data)
        {
            Memory[location] = data;
        }

        public void clearMemory()
        {
            Memory.Clear();
            for (int i = 0; i < MemorySize; i++)
            {
                Memory.Add((Int64)0);
            }
        }

    }

}
