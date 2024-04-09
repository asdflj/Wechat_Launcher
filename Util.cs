using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace Wechart_LauncherGui
{
    internal static class Util
    {
        public static bool CheckWechatPath(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path).Equals(".exe"))
            {
                return true;
            }
            return false;
        }
        public static void RunWechat(string path)
        {
            Process myProcess = new Process();
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(path);
            myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.Start();

        }
        public static string GetWechatPathFromReg()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser;
                RegistryKey wechat = key.OpenSubKey("Software\\Tencent\\WeChat");
                return Path.Combine(wechat.GetValue("InstallPath").ToString(), "WeChat.exe");
            }
            catch (Exception)
            {

            }
            return null;
        }
        public static void CloseOtherWeChatProgram() {
            foreach (var process in Process.GetProcessesByName("WeChat")) {
                process.Kill();
            }

        }
    }
}
