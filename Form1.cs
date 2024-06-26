﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary;
namespace Wechart_LauncherGui
{
    public partial class Form1 : Form
    {
        public Config config = GetConfig();
        public static string FILE_PATH = "config.json";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            textBox1.Text = config.size.ToString();
            label3.Text = config.path.ToString();
            
        }
        private static Config GetConfig()
        {

            if (System.IO.File.Exists(FILE_PATH))
            {
                return JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(FILE_PATH));
            }
            else { 
                return new Config();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "微信文件|WeChat.exe|快捷方式|*.lnk";
            dialog.Title = "请选择微信";
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel) {
                return;
            }
            if (Path.GetExtension(dialog.FileName).Equals(".lnk"))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = shell.CreateShortcut(dialog.FileName);
                config.path = shortcut.TargetPath;
            }
            else {
                config.path = dialog.FileName;
            }
            label3.Text = config.path;
            SaveConfig();
        }
        private void SaveConfig()
        {
            System.IO.File.WriteAllText(FILE_PATH, JsonConvert.SerializeObject(config));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = config.size.ToString();
            if (!config.path.Equals("") && Util.CheckWechatPath(this.config.path))
            {
                Util.CloseOtherWeChatProgram();
                for (int i = 0; i < config.size; i++)
                {
                    Util.RunWechat(config.path);
                }
            }
            else {
                MessageBox.Show("无效的微信程序路径,请手动指定程序");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int size;
            if (!int.TryParse(textBox1.Text, out size) || size <= 0)
            {
                return;
            }

            config.size = size;
            SaveConfig();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = Util.GetWechatPathFromReg();
            if (path.Equals(""))
            {
                MessageBox.Show("无法找到微信目录,请手动指定");
                return;
            }
            this.config.path = path;
            this.label3.Text = this.config.path;
            this.SaveConfig();
        }
    }
    public class Config {
        public string path { get; set; } = Util.GetWechatPathFromReg();
        public int size { get; set; } = 2;
        
        
    }
}
