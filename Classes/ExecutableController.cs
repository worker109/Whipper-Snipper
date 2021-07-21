using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;

namespace Whipper_Snipper
{
    internal class ExecutableController
    {   
        public void killExecutable(ExecutableInfo executableinfo)
        {
            Program.writeLog("Killing executable: " + executableinfo.ToString());
            try
            {
                //get pid from running process where path matches
                List<string> pids = new List<string>();
                string wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQueryString);
                ManagementObjectCollection results = searcher.Get();
                
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {                    
                    if (item.Path != null)
                    {
                        if (item.Path.ToLower().Trim() == executableinfo.exepath.ToLower().Trim())
                        {
                            pids.Add(item.Process.Id.ToString());
                        }
                    }                    
                }
                
                //kill processes with by pid
                Process[] processes = Process.GetProcesses();            
                foreach (string pid in pids)
                {
                    Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = "taskkill",
                            Arguments = "/pid \"" + pid + "\" /F",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string str = process.StandardOutput.ReadLine();
                        if (str.StartsWith("ERROR: "))
                            throw new Exception(str.Replace("ERROR: ", ""));
                    }
                }        
            }
            catch (Exception ex)
            {
            Program.writeLogError("Error killing executable: " + ex.Message);
            }
        }

        public void startExecutable(ExecutableInfo executableinfo)
        {
            Program.writeLog("Starting executable: " + executableinfo.ToString());
            try
            {
            Process.Start(executableinfo.exepath);
            }
            catch (Exception ex)
            {
            Program.writeLogError("Error starting executable: " + ex.Message);
            }
        }
    }
}
