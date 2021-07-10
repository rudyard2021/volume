using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace VolumeControl
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Appi.handle = Appi.GetConsoleWindow();
            Dictionary<string, string> data = GetJson();

            if (data.ContainsKey("name") == false)
                return;

            string name = data["name"];

            switch (name)
            {
                case "volume.up":
                    Appi.VolUp();
                    Appi.VolUp();
                    break;
                case "volume.down":
                    Appi.VolDown();
                    Appi.VolDown();
                    break;
                case "volume.close":
                    Appi.Mute();
                    break;
            }
        }

        public static Dictionary<string, string> GetJson()
        {
            string fileRequest = "request.data";
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (File.Exists(fileRequest) == false)
                return data;

            using (StreamReader stream = new StreamReader(fileRequest))
            {
                string json = stream.ReadToEnd();
                data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            return data;
        }

        public static void SetJson(object value)
        {
            string fileResponse = "response.data";
            using (StreamWriter stream = File.CreateText(fileResponse))
            {
                string json = JsonConvert.SerializeObject(value);
                stream.Write(json);
            }
        }
    }

    public class Appi
    {
        public static IntPtr handle;

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        public static void Mute()
        {
            SendMessageW(handle, WM_APPCOMMAND, handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        public static void VolDown()
        {
            SendMessageW(handle, WM_APPCOMMAND, handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        public static void VolUp()
        {
            SendMessageW(handle, WM_APPCOMMAND, handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }
    }
}
