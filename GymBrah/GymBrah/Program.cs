/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs  
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

        static void Main(string[] args)
        {
            // TODO Check that arg paths exist
        
            if (args.Length == 1) 
            {
                if (args[0].Substring(args[0].Length - 3) == "txt") // Ensure text file input
                {
                    try
                    {
                        GymBrah x = new GymBrah(args[0]);
                        int line = 0;
                        string parse = x.Parse(ref line);
                        
                        parse = "#include <stdio.h>\nint main()\n{\n" + parse + "}";
                        Console.Out.Write(parse);
                    
                        string file = args[0].Substring(0, args[0].Length - 4);
                
                        File.WriteAllText( file + ".c", parse);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else if (args[0].Substring(args[0].Length - 1) == "c") // C file input
                {
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