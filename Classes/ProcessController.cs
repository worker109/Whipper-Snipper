using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;

namespace Whipper_Snipper
{
    internal class ProcessController
    {
        private List<ProcessInfo> processlist = new List<ProcessInfo>();

        public ProcessController() => this.loadProcesses();

        public List<ProcessInfo> getProcessList(Profile profile = null, ProcessInfo processinfo = null)
        {
            if (profile == null)
            return this.processlist;
            List<ProcessInfo> processInfoList = new List<ProcessInfo>();
            foreach (ProcessInfo processInfo in this.processlist)
            {
            bool flag = false;
            foreach (Action actionitem in profile.actionitems)
            {
                if (actionitem.GetType() == typeof (ActionProcess) && ((ActionProcess) actionitem).processinfo.exename == processInfo.exename)
                flag = true;
            }
            if (processinfo != null && processinfo.exename == processInfo.exename)
                flag = false;
            if (!flag)
                processInfoList.Add(processInfo);
            }
            return processInfoList;
        }
    
        private void loadProcesses()
        {
            this.processlist = new List<ProcessInfo>();
                        
            List<string> pids = new List<string>();
            List<string> stringList = new List<string>();
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
                    ProcessInfo processInfo = new ProcessInfo();
                    processInfo.exename = Path.GetFileName(item.Path.ToString());
                    processInfo.exepath = item.Path.ToString();
                    if (!stringList.Contains(processInfo.exepath))
                    {
                        stringList.Add(processInfo.exepath);
                        this.processlist.Add(processInfo);
                    };                    
                }
            }
                            
            this.processlist.Sort((Comparison<ProcessInfo>) ((c1, c2) => c1.exename.CompareTo(c2.exename)));
        }

        public void killProcess(ProcessInfo processinfo)
        {
            Program.writeLog("Killing process: " + processinfo.ToString());
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
                        if (item.Path.ToLower().Trim() == processinfo.exepath.ToLower().Trim())
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
                Program.writeLogError("Error killing process: " + ex.Message);
            }
        }

        public void startProcess(ProcessInfo processinfo)
        {
            Program.writeLog("Starting process: " + processinfo.exepath);
            try
            {
                Process.Start(processinfo.exepath);
            }
            catch (Exception ex)
            {
            Program.writeLogError("Error starting process: " + ex.Message);
            }
        }
    }
}
