# **Esoteric Programming Language Compiler/Interpreter + IDE**

## Table of Contents
1. [General Info](#general-info)
2. [Technologies Used](#technologies-used)
3. [Features](#features)
4. [Getting Started](#getting-started)
    1. [Pre-requisites](#pre-requisites)
    2. [Usage](#usage)
5. [Authors](#authors)
6. [Status](#status)

## General Info
An esoteric programming language (esolang) is a programming language
designed to try new and interesting programming
techniques without having to build a fully fledged compiler. We have 
created our esolang using C# by converting our esolang code into C. We have 
implemented a gym colloquialism based language to test and learn about
the theory of programming language grammar.

Our esolang is called "GymBrah" due to it being based off of gym related
terms and "brah" being a common colloquialism and greeting - coming from
an abbreviation of "bro" being an abbreviation of "brother".

The language offers the unique mathematical multiplier keyword of 
"Plates". In gym related exercises the word "plates" is used to 
reference the weight a person is lifting - for example, the 
sentence "I can bench press 2 plates". A singular plate is a 
standard olympic 20kg weight, and when used in terms of an exercise
it is used to reference a single side of a standard olympic 
bar - which also weighs 20kg. So the sentence "I can bench press
2 plates" actually refers to the users ability to bench 4 plates
total - two either side of the bar - plus the weight of the bar, 
which totals to four 20kg plates plus a 20kg bar which is a total 
weight of 100kg. Our language also aims to provide a minimal level
of sentence structure following the gym related premise allowing 
the language to have some flow when written down, such as:
`can x bench 2 plates?` Which then evaluates to the corresponding 
C code: `int x = 100;`

In this project a simple IDE was implemented using C# to allow users to 
write and compile our code easily, however, the base compiler uses 
command line arguments to parse our esolang code into C to then be
compiled using the GCC.

## Technologies Used
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - C# Language
* [Rider](https://www.jetbrains.com/rider/) - JetBrains Rider IDE

## Features
* Compiling basic mathematical operations
* Basic selection and repetition
* Compilation of assignment using basic data types
* Function definitions and calls
* Compilation of basic output
* Parsing of our esolang into valid C code
* Compilation of C code through GCC
* Command line argument to pass a text file for parsing
* Command line argument to pass a C file for compilation

## Getting Started
These instructions will get you a copy of the project up and running
on your local machine for development and testing purposes. To begin
you must first ensure you have installed the necessary prerequisites.
Afterwards, pull the git repository and publish the GymBrah project 
into an executable. You can now run the program in the command line 
by running this executable with the necessary arguments. To use the 
IDE you must also publish the project to an executable. Finally, 
move the published files from the GymBrah project into a folder called
"publish" and place this folder in the same location as the IDE 
executable files, as seen below:

`../IDE/IDE.exe`

`../IDE/publish/GymBrah.exe`

### Pre-requisites
Users must install .NET 5 to run our program which can be gotten through
either Rider or VisualStudio 2022. Additionally, we used the MinGW GCC, 
although others should work the same provided they are added to your PATH variable. 

* [.Net SDK 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* [.Net 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* [GCC](https://www.mingw-w64.org/)

### Usage

When publishing the projects ensure in each of the `csproj` files the 
executable windows runtime is set to the one of your system (the 
current runtime is set to winx64). To run the GymBrah project call the
executable with either a `txt` or `c` file for the different forms
of compilation.

The command line argument below will create a C code file with the
same name as the `txt` file in the same location with valid C code, 
unless any errors are thrown.

`./GymBrah.exe code.txt`

The second command line argument below will compile the C code and 
produce an executable with the same name in the same location. The
program will also run the executable and print out the output.

`./GymBrah.exe code.c`

To use the IDE run the executable and enter GymBrah code into the 
input terminal (left) and press parse to generate the C code, shown
in the output terminal (right). Finally, pressing compile will compile
the C code and print the output to the output terminal. To run through
command line simply run:

`./IDE.exe`

This code example demonstrates the usage of our language and the
different structures we have implemented. This code, when passed
to the compiler, will be runnable as the compiler will only 
generate valid C code which can be always be run.

```
workout bench function(bench i)
lightweight
   can x bench 3?
   can y squat "hello"?
   can z deadlift 3.1?
   
   dropset y == "hello"
   lightweight
      x is <= 3
      lightweight
         can y "goodbye"?
         scream i?
      baby
   baby
   
   gain i?
baby
```

Below is the equivalent C code from the above program, so that you
can see line by line the structure of our language.

```
int function(int i)
{
   int x = 3;
   char* y = "hello";
   double z = 3.1;
   
   while(y == "hello")
   {
      if (x <= 3)
      {
         y = "goodbye";
         printf("%d",i);
      }
   }
   
   return i;
}
```

## Authors

* Jamie Grant
* Pawel Bielinski

## Status

Last updated 17/12/2021

Version 1.4
