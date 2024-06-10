using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SIT.Controller.Controllers
{
    public class ServerManager
    {
        public bool errorStartingServer { get; set; }


        public event Action<string>? OnOutputReceived;

        // Checks if the server process is currently running
        public bool IsServerRunning()
        {
            return Process.GetProcessesByName("Aki.Server").Any();
        }


        // Starts the server if it's not already running
        public void StartServer(string path)
        {
            if (IsServerRunning())
            {
                Console.WriteLine("Server is already running.");
                return;
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = $"{path}\\Aki.Server.exe",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            Process serverProcess = new Process { StartInfo = processStartInfo };
            AttachOutputHandlers(serverProcess);
            try
            {
                serverProcess.Start();
                BeginReadingOutput(serverProcess);
                errorStartingServer = false;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error selecting server folder: {ex.Message}");
                errorStartingServer = true;
            }
        }

        // Stops all running server processes
        public void StopServer()
        {
            var processes = Process.GetProcessesByName("Aki.Server");
            foreach (var process in processes)
            {
                StopSingleServerProcess(process);
            }
            OnOutputReceived?.Invoke("Server stopped");
        }

        // Attaches event handlers to capture output
        private void AttachOutputHandlers(Process process)
        {
            process.OutputDataReceived += (sender, args) => HandleProcessOutput(args.Data);
            process.ErrorDataReceived += (sender, args) => HandleProcessOutput(args.Data);
        }

        // Begins asynchronously reading output from the process
        private void BeginReadingOutput(Process process)
        {
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        // Handles stopping a single server process gracefully
        private void StopSingleServerProcess(Process process)
        {
            try
            {
                process.OutputDataReceived -= (sender, args) => HandleProcessOutput(args.Data);
                process.ErrorDataReceived -= (sender, args) => HandleProcessOutput(args.Data);

                if (!process.HasExited)
                {
                    if (!process.CloseMainWindow())
                    {
                        process.Kill();
                    }
                    process.WaitForExit(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping process: {ex.Message}");
            }
            finally
            {
                process.Dispose();
            }
        }

        // Handles output from the server process and cleans it before broadcasting
        private void HandleProcessOutput(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return;
            }
            string cleanOutput = CleanInput(output);
            OnOutputReceived?.Invoke(cleanOutput);
        }

        // Cleans the input to remove unwanted ANSI codes and non-ASCII characters
        public static string CleanInput(string input)
        {
            input = StripAnsiCodes(input);
            return new string(input.Where(c => c <= 127).ToArray());
        }

        // Strips ANSI escape sequences from the output
        public static string StripAnsiCodes(string input)
        {
            var ansiCodePattern = @"\e\[[^m]*m";
            return Regex.Replace(input, ansiCodePattern, string.Empty);
        }
    }
}
