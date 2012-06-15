using System;
using System.Collections.Generic;
using System.Text;

namespace MutableVM
{

    class Program
    {
        static void Main(string[] args)
        {

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

            // Internal instruction representation
            List<int> CurrentInstruction = new List<int>(4);

            // Main execution loop
            while (true)
            {

                // Read input from console, split based on spaces
                String[] input = Console.ReadLine().Split(' ');

                // Manage commands
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

                // Retreive next instruction
                Int64 instruction = m.getMemoryAtLocation((int)m.getRegister((int)Machine.RegisterEnum.PC));
                Console.WriteLine("instruction: " + instruction.ToString("0000000000000"));

                // Attempt to parse raw string into internal instruction format
                try
                {
                    CurrentInstruction = parseInstructionFromInteger(instruction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("instruction error: " + e.ToString());
                    break;
                }
                
                // Rename for readability
                type = CurrentInstruction[0];
                register = CurrentInstruction[1];
                memory = CurrentInstruction[2];
                constant = CurrentInstruction[3];

                // Primary instruction switch
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

    
    /// <summary>
    /// Machine class that manages the overall machine
    /// </summary>
    class Machine
    {

        public enum RegisterEnum {A, B, C, D, E, F, PC};

        Register[] registers = new Register[7] { new Register(), new Register(), 
            new Register(), new Register(), new Register(), new Register(), new Register() };

        Memory Memory = new Memory();

        public Machine()
        {
            clearAllRegisters();
            clearMemory();
        }

        public Int64 getRegister(RegisterEnum index)
        {
            return registers[(int)index].register;
        }

        public Int64 getRegister(int index)
        {
            RegisterEnum e = (RegisterEnum)index;
            return getRegister(e);
        }

        public void setRegister(RegisterEnum index, Int64 data)
        {
            registers[(int)index].register = data;
        }

        public void setRegister(int index, Int64 data)
        {
            RegisterEnum e = (RegisterEnum)index;
            setRegister(e, data);
        }

        public String printRegisters()
        {
            StringBuilder sb = new StringBuilder(12);

            sb.Append("PC: ");
            sb.Append(registers[(int)RegisterEnum.PC].register);

            for (int i = 0; i < registers.Length-1; i++)
            {
                sb.Append(" " + (char)(i+65) + ": ");
                sb.Append(registers[i].register);
            }

            return sb.ToString();

        }

        public void clearRegister(RegisterEnum index)
        {
            registers[(int)index].register = 0;
        }

        public void clearRegister(int index)
        {
            RegisterEnum e = (RegisterEnum)index;
            clearRegister(e);
        }

        public void clearAllRegisters()
        {
            foreach (Register r in registers)
            {
                r.register = 0;
            }
        }

        public void setMemoryFromRegister(int register, int location)
        {
            Memory.setMemoryAtLocation(location, getRegister(register));
        }

        public Int64 getMemoryAtLocation(int location)
        {
            return Memory.getMemoryAtLocation(location);
        }

        public void setMemoryAtLocation(int location, Int64 data)
        {
            Memory.setMemoryAtLocation(location, data);
        }

        public void clearMemory()
        {
            Memory.clearMemory();
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
            return Memory.printMemory(location, length);
        }

    }
    /// <summary>
    /// Class to internally represent a register
    /// </summary>
    class Register
    {
        public Int64 register { get; set; }

        public Register()
        {
            clearRegister();
        }

        public void clearRegister()
        {
            register = 0;
        }
    }

    /// <summary>
    /// Class that manages the memory of a machine
    /// </summary>

    class Memory
    {
        private List<Int64> MemoryData;
        public const int MemorySize = 8192;

        public Memory()
        {
            MemoryData = new List<Int64>(MemorySize);
            clearMemory();
        }

        public Int64 getMemoryAtLocation(int location)
        {
            return MemoryData[location];
        }

        public void setMemoryAtLocation(int location, Int64 data)
        {
            MemoryData[location] = data;
        }

        public void clearMemory()
        {
            MemoryData.Clear();
            for (int i = 0; i < MemorySize; i++)
            {
                MemoryData.Add((Int64)0);
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
            for (int i = location; i < location + length; i++)
            {
                currentMem = MemoryData[i].ToString("0000000000000");
                sb.AppendLine(currentMem);
            }

            return sb.ToString();
        }
    }

}
