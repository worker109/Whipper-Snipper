using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
    internal class Devcon
    {
        public string path;
        private List<HardwareInfo> hardware = new List<HardwareInfo>();

        public Devcon(string path)
        {
            this.path = path;
            this.extractExe();
            this.loadHardware();
        }

        private void extractExe()
        {
            if (File.Exists(this.path))
            return;
            try
            {
            Program.writeLog("Extracting devcon.exe to: " + this.path);
            if (Environment.Is64BitOperatingSystem)
                File.WriteAllBytes(this.path, Resources.devcon64);
            else
                File.WriteAllBytes(this.path, Resources.devcon32);
            }
            catch (Exception ex)
            {
            Program.writeLogError(ex.Message);
            }
        }

        public void changeHardwareState(HardwareInfo hardwareinfo, bool enable)
        {
            List<string> stringList = new List<string>();
            foreach (string hardwareid in hardwareinfo.hardwareids)
            stringList.Add("\"" + hardwareid + "\"");
            string str;
            if (enable)
            {
            Program.writeLog("Enabling hardware: " + hardwareinfo.name);
            str = "enable " + string.Join(" ", stringList.ToArray());
            }
            else
            {
            Program.writeLog("Disabling hardware: " + hardwareinfo.name);
            str = "disable " + string.Join(" ", stringList.ToArray());
            }
            try
            {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                FileName = this.path,
                Arguments = str,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
                }
            };
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                string message = process.StandardOutput.ReadLine();
                if (message.Contains("Access is denied.") || message.Contains("No matching devices found."))
                throw new Exception(message);
            }
            }
            catch (Exception ex)
            {
            Program.writeLogError("Error changing hardware state: " + ex.Message);
            }
        }

        public List<HardwareInfo> getHardwareList(
            Profile profile = null,
            HardwareInfo hardwareinfo = null)
        {
            if (profile == null)
            return this.hardware;
            List<HardwareInfo> hardwareInfoList = new List<HardwareInfo>();
            foreach (HardwareInfo hardwareInfo in this.hardware)
            {
            bool flag = false;
            foreach (Action actionitem in profile.actionitems)
            {
                if (actionitem.GetType() == typeof (ActionHardware) && ((ActionHardware) actionitem).hardwareinfo.deviceid == hardwareInfo.deviceid)
                flag = true;
            }
            if (hardwareinfo != null && hardwareinfo.deviceid == hardwareInfo.deviceid)
                flag = false;
            if (!flag)
                hardwareInfoList.Add(hardwareInfo);
            }
            return hardwareInfoList;
        }

        private void loadHardware()
        {
            this.hardware = new List<HardwareInfo>();
            Program.writeLog("Getting hardware list");
            try
            {
                Process process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = this.path,
                        Arguments = "hwids *",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                int index = 0;
                bool flag = false;
                while (!process.StandardOutput.EndOfStream)
                {
                    string message = process.StandardOutput.ReadLine();
                    if (message.Contains("Access is denied."))
                        throw new Exception(message);
                    if (!message.Contains(" matching device(s) found."))
                    {
                        if (!message.StartsWith("    ") && message.Trim() != "HTREE\\ROOT\\0")
                        {
                            this.hardware.Add(new HardwareInfo());
                            index = this.hardware.Count<HardwareInfo>() - 1;
                            this.hardware[index].deviceid = message.Trim();
                        }
                        else if (message.StartsWith("    Name: "))
                        {
                            string str = message.Replace("    Name: ", "");
                            this.hardware[index].name = str.Trim();
                        }
                        else if (message.StartsWith("    Hardware IDs:"))
                        {
                            message.Replace("    Hardware IDs:", "");
                            flag = true;
                        }
                        else if (flag && !message.StartsWith("        ") && message.Trim() != "")
                            flag = false;
                        else if (flag)
                        {
                            string str = message.Replace("        ", "");
                            this.hardware[index].hardwareids.Add(str.Trim());
                        }
                    }
                }
                this.hardware.Sort((Comparison<HardwareInfo>) ((c1, c2) => c1.name.CompareTo(c2.name)));
            }
            catch (Exception ex)
            {
                Program.writeLogError("Error getting hardware list: " + ex.Message);
            }
            Program.writeLog(this.hardware.Count.ToString() + " devices found");
        }
    }
}