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
            m.setMemoryAtLocation(0, 8000000000001); // A = 1
            m.setMemoryAtLocation(1, 8000100000001); // B = 1
            m.setMemoryAtLocation(2, 8000200000001); // C = 1
            m.setMemoryAtLocation(3, 8000300000144); // D = 144

            m.setMemoryAtLocation(4, 2000100000001); // A = A + B
            m.setMemoryAtLocation(5, 1000100020001); // C = A;
            m.setMemoryAtLocation(6, 1000100040001); // E = C
            m.setMemoryAtLocation(7, 3000400030000); // E = E - D
            m.setMemoryAtLocation(8, 7000400170002); // Jump to line 17 if E > 0
            m.setMemoryAtLocation(9, 8000400000000); // E = 0

            m.setMemoryAtLocation(10, 2000000010001); // B = B + A 
            m.setMemoryAtLocation(11, 1000000020001); // C = B;
            m.setMemoryAtLocation(12, 1000100040001); // E = C
            m.setMemoryAtLocation(13, 3000400030000); // E = E - D
            m.setMemoryAtLocation(14, 7000400170002); // Jump to line 17 if E > 0
            m.setMemoryAtLocation(15, 8000400000000); // E = 0

            m.setMemoryAtLocation(16, 7000100049999); // Jump to 4
            m.setMemoryAtLocation(17, 7000300009999); // goto line 3

            Console.WriteLine("MutableVM by overlordnyaldee\n");
            Console.WriteLine("Commands: ");
            Console.WriteLine("printreg, printmem, quit\n");
            Console.WriteLine("Current Memory:");
            Console.WriteLine(m.printMemory());
            Console.WriteLine(m.printRegisters());

            while (true)
            {

                String[] input = Console.ReadLine().Split(' ');


                if (input[0].Equals("printmem"))
                {
                    if (input.Length > 1)
                    {
                        int pos = Convert.ToInt32(input[1]);
                        Console.WriteLine("\nMemory at position " + pos + ":");
                        Console.WriteLine(m.printMemory(pos));
                        continue;
                    }
                    else
                    {
                        int pos = (int)m.getRegister((int)Machine.RegisterEnum.PC);
                        Console.WriteLine("\nMemory at position " + pos + ":");
                        Console.WriteLine(m.printMemory(pos));
                        continue;
                    }

                }
                else if (input[0].Equals("printreg"))
                {
                    Console.WriteLine(m.printRegisters());
                    continue;
                }
                else if (input[0].Equals("quit"))
                {
                    break;
                }

                Int64 instruction = m.getMemoryAtLocation((int)m.getRegister((int)Machine.RegisterEnum.PC));
                Console.WriteLine("instruction: " + instruction.ToString("0000000000000"));
                try
                {
                    c = parseInstructionFromInteger(instruction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("instruction error: " + e.ToString());
                    break;
                }
                

                type = c[0];
                register = c[1];
                memory = c[2];
                constant = c[3];

                switch (type)
                {
                    case 0: // Load
                        if (constant == 0)
                        {
                            m.setRegister(register, m.getMemoryAtLocation(memory));
                        }
                        else
                        {
                            m.setRegister(register, m.getRegister(memory));
                        }
                        break;
                    case 1: // Store

                        if (constant == 0)
                        {
                            m.setMemoryFromRegister(register, memory);
                        }
                        else
                        {
                            m.setRegister(memory, m.getRegister(register));
                        }
                        break;
                    case 2: // Add 
                        m.setRegister(register, m.getRegister(register) + m.getRegister(memory));
                        break;
                    case 3: // Subtract
                        m.setRegister(register, m.getRegister(register) - m.getRegister(memory));
                        break;
                    case 4: // Multiply
                        m.setRegister(register, m.getRegister(register) * m.getRegister(memory));
                        break;
                    case 5: // Divide
                        m.setRegister(register, m.getRegister(register) / m.getRegister(memory));
                        break;
                    case 6: // Remainder
                        m.setRegister(register, m.getRegister(register) % m.getRegister(memory));
                        break;
                    case 7: // Jump
                        // Check Flags
                        if (constant == 0)
                        {
                            // Jump if zero
                            if (m.getRegister(register) == 0)
                            {
                                m.setRegister((int)Machine.RegisterEnum.PC, memory - 1);
                            }
                        }
                        else if (constant == 1)
                        {
                            // Jump if less than zero
                            if (m.getRegister(register) < 0)
                            {
                                m.setRegister((int)Machine.RegisterEnum.PC, memory - 1);
                            }
                        }
                        else if (constant == 2)
                        {
                            // Jump if greater than zero
                            if (m.getRegister(register) > 0)
                            {
                                m.setRegister((int)Machine.RegisterEnum.PC, memory - 1);
                            }
                        }
                        else
                        {
                            // Jump always
                            m.setRegister((int)Machine.RegisterEnum.PC, memory - 1);
                        }
                        break;
                    case 8: // Set register to constant
                        m.setRegister(register, constant);
                        break;
                    case 9: // No operation
                        break;
                }

                // Increment program counter
                m.setRegister((int)Machine.RegisterEnum.PC, m.getRegister((int)Machine.RegisterEnum.PC) + 1);

                Console.WriteLine(m.printRegisters());

            }

            Console.ReadLine();

        }

        static List<int> parseInstructionFromString(String a)
        {
            List<int> c = new List<int>(4);
            c.Add(Convert.ToInt32(a.Substring(0, 1)));
            c.Add(Convert.ToInt32(a.Substring(1, 4)));
            c.Add(Convert.ToInt32(a.Substring(5, 4)));
            c.Add(Convert.ToInt32(a.Substring(9)));
            

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
        private Int64 RegisterD;
        private Int64 RegisterE;
        private Int64 RegisterF;
        private Int64 RegisterPC;

        public enum RegisterEnum {A, B, C, D, E, F, PC};

        int[] Registers = new int[5] { 1, 2, 3, 4, 5 };

        private List<Int64> Memory;

        public Machine()
        {
            clearAllRegisters();
            Memory = new List<Int64>(MemorySize);
            clearMemory();
        }

        public Int64 getRegister(int register)
        {
            RegisterEnum e = (RegisterEnum)register;
            switch (e)
            {
                case RegisterEnum.A:
                    return RegisterA;
                case RegisterEnum.B:
                    return RegisterB;
                case RegisterEnum.C:
                    return RegisterC;
                case RegisterEnum.D:
                    return RegisterD;
                case RegisterEnum.E:
                    return RegisterE;
                case RegisterEnum.F:
                    return RegisterF;
                case RegisterEnum.PC:
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
                    RegisterD = data;
                    break;
                case 4:
                    RegisterE = data;
                    break;
                case 5:
                    RegisterF = data;
                    break;
                case 6:
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
            sb.Append(RegisterPC.ToString(""));
            sb.Append(" A: ");
            sb.Append(RegisterA.ToString(""));
            sb.Append(" B: ");
            sb.Append(RegisterB.ToString(""));
            sb.Append(" C: ");
            sb.Append(RegisterC.ToString(""));
            sb.Append(" D: ");
            sb.Append(RegisterD.ToString(""));
            sb.Append(" E: ");
            sb.Append(RegisterE.ToString(""));
            sb.Append(" F: ");
            sb.Append(RegisterF.ToString(""));
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
                case 3:
                    RegisterD = 0;
                    break;
                case 4:
                    RegisterE = 0;
                    break;
                case 5:
                    RegisterF = 0;
                    break;
                case 6:
                    RegisterPC = 0;
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
            RegisterD = 0;
            RegisterE = 0;
            RegisterF = 0;
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

        public String printMemory()
        {
            return printMemory(0);
        }

        public String printMemory(int location)
        {
            return printMemory(location, 16);
        }

        public String printMemory(int location, int length)
        {
            StringBuilder sb = new StringBuilder(length * 32);
            String currentMem;
            for (int i = location; i < length; i++)
            {
                currentMem = Memory[i].ToString("0000000000000");
                sb.AppendLine(currentMem);
            }

            return sb.ToString();
        }

    }

}
