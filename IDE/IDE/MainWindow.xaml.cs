using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Compile.Click += Compile_OnClick;
            Parse.Click += Parse_OnClick;
            Clear.Click += Clear_OnClick;
        }

        private static string _run(string input)
        {
            try
            {
                //TODO Use better path
                string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
                path += @"\publish\";
                
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
                return exception.Message;
            }
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            OutputTerminal.Text = "";
            Compile.Visibility = Visibility.Visible;
            Clear.Visibility = Visibility.Hidden;
        }
        
        private void Parse_OnClick(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            path += @"\publish\";
            
            File.WriteAllText(path + @"output.txt", InputTerminal.Text);

            OutputTerminal.Text = _run(@"output.txt");
            
            Compile.Visibility = Visibility.Visible;
            Clear.Visibility = Visibility.Hidden;
        }
        
        private void Compile_OnClick(object sender, RoutedEventArgs e)
        {
            if (OutputTerminal.Text == "")
            {
                MessageBox.Show("Please parse some code before trying to compile.");
            }
            else if (OutputTerminal.Text.Split("\n").Last().Contains("Error"))
            {
                MessageBox.Show("Error in code, cannot compile.");
            }
            else
            {
                OutputTerminal.Text = _run("output.c");
                
                Compile.Visibility = Visibility.Hidden;
                Clear.Visibility = Visibility.Visible;
            }
        }
    }
}