/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs  
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Entry class for the running of the parser from an exe. The program takes two types of command line
 *                  arguments - a singular text file containing code to be parsed into a c file in the same location,
 *                  and a c file as well as an output file path. 
 */

using System;
using System.Diagnostics;
using System.IO;

namespace GymBrah
{
    class Program
    {
        /// <summary>
        /// Entry point for the EsoLang compiler, taking command line arguments for the language code to be parsed into
        /// c and the c code to be compiled.
        /// </summary>
        /// <param name="args"> Command line arguments. </param>
        static void Main(string[] args)
        {
            if (args.Length == 1) 
            {
                if (args[0].Substring(args[0].Length - 3) == "txt") // Ensure text file input
                {
                    try
                    {
                        // Build parser
                        GymBrah x = new GymBrah(args[0]);
                        int line = 0;
                        string parse = x.Parse(ref line); // Get output
                        
                        // Wrap in main method
                        parse = "#include <stdio.h>\nint main()\n{\n" + parse + "}";
                        Console.Out.Write(parse);
                    
                        string file = args[0].Substring(0, args[0].Length - 4);
                
                        // Write to c file
                        File.WriteAllText( file + ".c", parse);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
                else if (args[0].Substring(args[0].Length - 1) == "c") // C file input
                {
                    // Ensure file path exists
                    try
                    {
                        StreamReader x = new StreamReader(args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.Write("Error: Compile error - " + e.Message);
                        return;
                    }
                    
                    // Get file path
                    string file = "\"" + args[0].Substring(0, args[0].Length - 2);
                    
                    // Compile into C executable
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = @"/C gcc " + file + ".c\" -o " + file + "\" & " + file + ".exe\"";
                    startInfo.Verb = "runas";
                    startInfo.RedirectStandardOutput = true;
                    process.StartInfo = startInfo;
                    process.Start();
                    
                    // Print any errors with compilation
                    Console.Out.Write(process.StandardOutput.ReadToEnd());
                }
                else
                {
                    Console.Out.Write("Error: Input file must be a c or txt file.");
                }
            }
            else
            {
                Console.Out.Write("Error: Program cannot be run like this.");
            }
        }
    }
}