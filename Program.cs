using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommandLine;

namespace sendkeys
{
    class Program
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed(RunOptions)
              .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
        }

        private static void RunOptions(Options options)
        {
            Console.WriteLine("Targeted window: {0}", options.WindowName);
            Console.WriteLine("Key to be sent: {0}", options.Key);
            try
            {
                Process process = new Process();
                Process[] proc = Process.GetProcesses();
                int i = 0;
                foreach (Process p in proc)
                {
                    
                    if (options.List)
                    {
                        Console.WriteLine("{0}: Process Name {1}; Process title: {2}", ++i, p.ProcessName, p.MainWindowTitle);
                    }

                    if (p.MainWindowTitle==options.WindowName)
                    {
                        process = p;
                        break;
                    }
                }
                if(process != null)
                {
                    IntPtr h = process.MainWindowHandle;
                    SetForegroundWindow(h);
                    SendKeys.SendWait(options.Key);
                }
                else
                {
                    Console.WriteLine("Window not found;");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public class Options
        {
            [Option('w',
                Required = true,
                HelpText = "Window name where the key will be sent.")]
            public string WindowName { get; set; }

            [Option('k',
                Required = true,
                HelpText = "The key to be sent.")]
            public string Key { get; set; }

            [Option('l',
                Required = false,
                HelpText = "List all open windows")]
            public bool List { get; set; }
        }
    }
}