Nyaldee's MutableVM
====================

A simple virtual machine implementation using a custom machine code. Very early alpha, but it works. It currently can calculate the Fibonacci sequence with a program written in the VM machine code.

This will eventually used as a part of a complete project.

Usage:
---------------------

Compile the application with Visual Studio 2010. To change the running program, one currently needs to edit the source.

MutableVM Instruction Documentation:
---------------------

##Registers:
- A - 0 - General
- B - 1 - General
- C - 2 - General
- D - 3 - General
- E - 4 - General
- F - 5 - General
- PC- 6 - Program Counter

##Memory Addressable from 0 to 8191 (2^13-1)

##Instruction Formats:

- Load/Store Format:
	00 0000 0000 0000
	|    |    |    |
	|    |    |    Flags
	|    |    Memory/Register Address
	|    Register To Use
	Instruction Type
	
	Flags:
	0000 - Load/Store data from register from/to memory address
	0001 - Load/Store data from register from/to register (memory address used as register number)


- Mathematics Format:
	02 0000 0000 0000
	|    |    |    |
	|    |    |    Constant/Flags
	|    |    Register (that will be copied from, unchanged)
	|    Register (that will be operated on, result of calculation)
	Instruction Type


- Jump Format:
	07 0000 0000 0000
	|    |    |    |
	|    |    |    Flags
	|    |    Memory Address
	|    Register To Use
	Instruction Type
	
	Flags:
	0000 - Jump if register is zero
	0001 - Jump if register is less than zero
	0002 - Jump if register is greater than zero
	0003 - Jump always to memory address


- Set Register to Constant Format:
	08 0000 00000000
	|    |    |    
	|    |    |    
	|    |    Constant
	|    Register To Use
	Instruction Type


- Generic Format:
	00 0000 0000 0000
	|    |    |    |
	|    |    |    Constant/Flags
	|    |    Memory Address
	|    Register To Use
	Instruction Type


##Instruction Types:

- 00 = load
- 01 = store
- 02 = add
- 03 = subtract
- 04 = multiply
- 05 = divide
- 06 = rem
- 07 = jump
- 08 = set register to constant
- 09 = no operation


Version History:
---------------------

### Version 0.01
- Initial version, successfully runs a program to calculate the Fibonacci sequence.

Credits:
---------------------

- None
