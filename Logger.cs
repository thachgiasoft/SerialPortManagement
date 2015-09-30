using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComManagement
{
    public static class Logger
    {
        private static string path = Application.StartupPath+"\\log.txt";
        
        public static void Log(string meg)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                Logging(meg,w);
            }
        }
        private static void Logging(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
