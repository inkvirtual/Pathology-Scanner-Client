﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGuiClient
{
    public class Configuration
    {
        private IniFile Ini { get; set; }
        private readonly string Section = "config";
        public string Ip
        {
            get
            {
                return Ini.Read("ip", Section);
            }
            set
            {
                Ini.Write("ip", value, Section);
            }
        }
        public string Port
        {
            get
            {
                return Ini.Read("port", Section);
            }
            set
            {
                Ini.Write("port", value, Section);
            }
        }

        public Configuration()
        {
            Ini = new IniFile("RunConfig.ini");
        }

        public void ResetToDefault()
        {
            Ip = "192.168.0.2";
            Port = "8080";
        }
    }

    // http://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    class IniFile
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;
        // Or string exe = "R:\\test\\config.ini";

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
