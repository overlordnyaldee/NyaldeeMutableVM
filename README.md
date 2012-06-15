Nyaldee's MutableVM
====================

A simple virtual machine implementation using a custom machine code. Very early alpha, but it works. It currently can calculate the Fibonacci sequence with a program written in the VM machine code.

This will eventually used as a part of a complete project.

Usage:
---------------------

Download the latest version or compile the application with Visual Studio 2010. To change the running program, one currently needs to edit the source.

Commands:
---------------------

- none (press enter)
	Executes the next instruction
- printmem [location]
	Displays the memory contents, either at the program counter or at an optional parameter
- printreg
	Shows the contents of all registers
- quit
	Ends the simulation

MutableVM Instruction Documentation:
---------------------

######Registers:
- General Purpose
	- A - 0
	- B - 1
	- C - 2
	- D - 3
	- E - 4
	- F - 5
- Program Counter
	- PC- 6

Memory Addressable from 0 to 8191 (2^13-1)

######Instruction Formats:

- Load/Store Format:
	00 0000 0000 0000
	
	- Instruction Type
	- Register To Use
	- Memory/Register Address
	- Flags
		- 0000 - Load/Store data from register from/to memory address
		- 0001 - Load/Store data from register from/to register (memory address used as register number)


- Mathematics Format:
	02 0000 0000 0000

	- Instruction Type
	- Register (that will be operated on, result of calculation)
	- Register (that will be copied from, unchanged)

- Jump Format:
	07 0000 0000 0000

	- Instruction Type
	- Register To Use
	- Memory/Register Address
	- Flags
		- 0000 - Jump if register is zero
		- 0001 - Jump if register is less than zero
		- 0002 - Jump if register is greater than zero
		- 0003 - Jump always to memory address


- Set Register to Constant Format:
	08 0000 00000000

	- Instruction Type
	- Register To Use
	- Constant


- Generic Format:
	00 0000 0000 0000

	- Instruction Type
	- Register To Use
	- Memory/Register Address
	- Flags/Constant


######Instruction Types:

- 00
	load
- 01
	store
- 02
	add
- 03
	subtract
- 04
	multiply
- 05
	divide
- 06
	rem
- 07
	jump
- 08
	set register to constant
- 09
	no operation


Version History:
---------------------

### Version 0.02 ([Download] (https://github.com/downloads/overlordnyaldee/NyaldeeMutableVM/MutableVM-0.02.zip))
- Updated the instruction set, added additional registers, added commands to print registers/memory

### Version 0.01
- Initial version, successfully runs a program to calculate the Fibonacci sequence.

Credits:
---------------------

- None
