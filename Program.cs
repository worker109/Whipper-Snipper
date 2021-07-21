using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Whipper_Snipper
{
    internal static class Program
    {
        public static bool consolemode = false;
        public static bool consolemoderun = true;
        public static bool consolemodewait = false;
        public static MainForm mainform;
        private static string configfoldername = "WhipperSnipper";
        private static string configfilename = "WhipperSnipperConfig.xml";
        private static string devconfilename32 = "devcon32.exe";
        private static string devconfilename64 = "devcon64.exe";
        public static Devcon devcon;
        public static ServiceController servicecontroller;
        public static ProcessController processcontroller;
        public static ExecutableController executablecontroller;
        public static ProfileList profilelist = new ProfileList();

        [STAThread]
        private static void Main(string[] args)
        {
            foreach (string str in args)
            {
                if (str == "-run" || str == "run") 
                {
                    Program.consolemode = true;
                    Program.consolemoderun = true;
                }
                if (str == "-revert" || str == "revert")
                {
                    Program.consolemode = true;
                    Program.consolemoderun = false;
                }
                if (str == "-wait" || str == "wait")
                {
                    Program.consolemodewait = true;
                }

            }
            if (Program.consolemode)
            {
                Program.AllocConsole();
                Program.loadConfigurationFile();
                Program.instantiateDevcon();
                Program.instantiateServiceController();
                Program.instantiateProcessController();
                Program.instantiateExecutableController();
                foreach (string str in args)
                {
                    foreach (Profile profile in Program.profilelist.profiles)
                    {
                        if(str.ToLower().Trim() == profile.guid.ToLower().Trim())
                        {
                            if (Program.consolemoderun)
                                profile.run();
                            else
                                profile.revert();
                        }
                    }
                }
                if(Program.consolemodewait)
                    Console.Read();
                Application.Exit();
            }
            else
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.mainform = new MainForm();
            Application.Run((Form) Program.mainform);
            }
        }

        public static void writeLog(string message, string textcolor = "Black")
        {
            message = DateTime.Now.ToString("H:mm:ss") + "\t" + message;
            if (Program.consolemode)
                Console.WriteLine(message);
            else
                Program.mainform.writeLogTextBox(message, textcolor);
        }

        public static void writeLogError(string message) => Program.writeLog(message, "Red");

        public static void loadConfigurationFile()
        {
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Program.configfoldername;
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Program.configfoldername + "\\" + Program.configfilename;
            if (!File.Exists(path2))
            {
            Program.writeLog("Configuration file not found at: " + path2);
            try
            {
                Program.writeLog("Creating configuration directory: " + path1);
                Directory.CreateDirectory(path1);
                Program.saveConfigurationFile();
            }
            catch (Exception ex)
            {
                Program.writeLogError(ex.Message);
            }
            }
            else
            {
            Program.writeLog("Loading configuration file from: " + path2);
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof (ProfileList));
                StreamReader streamReader1 = new StreamReader(path2);
                StreamReader streamReader2 = streamReader1;
                Program.profilelist = (ProfileList) xmlSerializer.Deserialize((TextReader) streamReader2);
                streamReader1.Close();
            }
            catch (Exception ex)
            {
                Program.writeLogError(ex.Message);
            }
            }
        }

        public static void saveConfigurationFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Program.configfoldername + "\\" + Program.configfilename;
            Program.writeLog("Saving configuration file");
            try
            {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (ProfileList));
            StringWriter stringWriter = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create((TextWriter) stringWriter);
            xmlSerializer.Serialize(xmlWriter, (object) Program.profilelist);
            string contents = stringWriter.ToString();
            File.WriteAllText(path, contents);
            }
            catch (Exception ex)
            {
            Program.writeLogError("Error saving configuration file to: " + path);
            Program.writeLogError(ex.Message);
            }
        }

        public static void instantiateDevcon()
        {
            if (Environment.Is64BitOperatingSystem)
            Program.devcon = new Devcon(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Program.configfoldername + "\\" + Program.devconfilename64);
            else
            Program.devcon = new Devcon(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Program.configfoldername + "\\" + Program.devconfilename32);
        }

        public static void instantiateServiceController() => Program.servicecontroller = new ServiceController();

        public static void instantiateProcessController() => Program.processcontroller = new ProcessController();

        public static void instantiateExecutableController() => Program.executablecontroller = new ExecutableController();

        private static void consoleMain() => Program.loadConfigurationFile();

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
