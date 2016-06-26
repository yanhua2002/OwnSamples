using Microsoft.Win32;
using System.IO;

namespace ComSecurity
{
    class Program
    {
        static void Main(string[] args)
        {
            string temp = null, splitter = " , ", tempUninstall = null;
            object displayName = null, uninstallString = null;
            RegistryKey currentKey = null;
            RegistryKey pregKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");

            try
            {
                foreach (string item in pregKey.GetSubKeyNames())
                {
                    currentKey = pregKey.OpenSubKey(item);
                    displayName = currentKey.GetValue("DisplayName");
                    uninstallString = currentKey.GetValue("UninstallString");
                    if (displayName!=null)
                    {
                        tempUninstall = (uninstallString == null) ? "Null" : uninstallString.ToString();
                        temp += System.Environment.MachineName + splitter + displayName.ToString() + splitter + tempUninstall + System.Environment.NewLine;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            // string currentDir = System.Environment.CurrentDirectory + @"\aaa.txt";
            string currentDir = @"\\10.164.125.7\OtherSofts\Temp\" + System.Environment.MachineName + @".txt";
            FileStream fs = new FileStream(currentDir, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(temp);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
