/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          MainWindow.xaml.cs, MainWindow.xaml, App.xaml.cs, App.xaml 
 * Last Modified :  17/12/21
 * Version :        1.4
 * Description :    Entry class for the running of the IDE for our esolang. Allowing a user to enter code which can then
 *                  be parsed into C code, which can then be compiled and outputted to the user.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Entry class for the MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Build app
            
            // Set on click methods
            Compile.Click += Compile_OnClick;
            Parse.Click += Parse_OnClick;
            Clear.Click += Clear_OnClick;
            
            // Initialise buttons
            Parse.Visibility = Visibility.Visible;
            Compile.Visibility = Visibility.Hidden;
            Clear.Visibility = Visibility.Visible;
            
            
            // Set title, background colours, and font colours
            Title = "GymBrah IDE";
            Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));
            
            InputTerminal.Background = new SolidColorBrush(Color.FromRgb(61, 61, 61));
            OutputTerminal.Background = new SolidColorBrush(Color.FromRgb(61, 61, 61));

            InputTerminal.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
            OutputTerminal.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
        }

        /// <summary>
        /// General run method to run a given input through the GymBrah.exe. Depending on the input this will either
        /// parse a txt file or compile a c file.
        /// </summary>
        /// <param name="input"> Command line argument. </param>
        /// <returns> String of output from running the GymBrah.exe </returns>
        private static string _run(string input)
        {
            try
            {
                // Get execution file path (where everything is run from)
                string path = Directory.GetCurrentDirectory();
                path += @"\publish\"; // Where the txt, C, and exe files are
                
                // Run the exe file with command line arguments
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                
                startInfo.FileName = path + @"GymBrah.exe";
                startInfo.Arguments = " \"" + path + input + "\"";
                startInfo.Verb = "runas";
                startInfo.RedirectStandardOutput = true;
                process.StartInfo = startInfo;
                process.Start();
                
                return process.StandardOutput.ReadToEnd();
            }
            catch (Exception exception)
            {
                return "Error: " + exception.Message;
            }
        }

        /// <summary>
        /// Clear output terminal and show compilation button again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            // Clear output, and change button visibility
            OutputTerminal.Text = "";
        }
        
        /// <summary>
        /// Parse method that parses the user's written code, and shows the output in the terminal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parse_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get execution file path (where everything is run from)
                string path = Directory.GetCurrentDirectory();
                path += @"\publish\"; // Where the txt, C, and exe files are
                
                // Write code to be parsed to a txt file.
                File.WriteAllText(path + @"output.txt", InputTerminal.Text);

                // Parse said txt file, and show output in terminal
                OutputTerminal.Text = _run(@"output.txt");
            
                // Change button visibility
                Compile.Visibility = Visibility.Visible;
                Clear.Visibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        
        /// <summary>
        /// Compile method to compile the generated C code from the IDE.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compile_OnClick(object sender, RoutedEventArgs e)
        {
            if (OutputTerminal.Text == "") // No code to compile
            {
                MessageBox.Show("Please parse some code before trying to compile.");
            }
            if (OutputTerminal.Text.Split("\n").Last().Contains("Error")) // Error in parsing
            {
                MessageBox.Show("Error in code, cannot compile.");
                
            }
            else
            {
                // Compile c code
                OutputTerminal.Text = _run("output.c");
            }
            
            // Change button visibility
            Compile.Visibility = Visibility.Hidden;
            Clear.Visibility = Visibility.Visible;
        }
    }
}