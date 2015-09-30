using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;

namespace ComManagement.Bo
{
    public class ComHelperBase
    {
        private ComPortSetting _setting;
        protected SerialPort portInstance;
        protected System.Timers.Timer timer;

        public ComHelperBase(ComPortSetting set)
        {
            _setting = set;
            portInstance = new SerialPort();
            portInstance.BaudRate = _setting.BaudRate;
            portInstance.PortName = _setting.PortName;
            portInstance.DataBits = _setting.DataBits;
            portInstance.StopBits =  _setting.StopBits;
            portInstance.Parity = _setting.Parity;
            portInstance.DtrEnable = _setting.Dtr;
            portInstance.RtsEnable = _setting.Rts;
            timer = new System.Timers.Timer(_setting.ReadTimeout);
        }
        public void Open()
        {
            if (portInstance.IsOpen) portInstance.Close();
            else portInstance.Open();                 
        }
        public void Close()
        {
            if (portInstance.IsOpen) portInstance.Close();
        }
        protected string StringProcess(string str)
        {
            string s1, s2;
            s1 = str.Substring(0, 3);
            s2 = str.Substring(3, 2);
            IFormatProvider culture = new CultureInfo("vi-VN", true);
            IFormatProvider culture1 = new CultureInfo("en-US", true);
            double temp;
            if (!Double.TryParse(s1 + "," + s2, NumberStyles.Float, culture, out temp))
            {
                Double.TryParse(s1 + "," + s2, NumberStyles.Float, culture1, out temp);
            }
            return temp.ToString();

        }
        protected static string StringProcess(string str, int i)
        {
            if (i == 0)
            {
                string s1 = str.Substring(0, 2);
                string s2 = str.Substring(2, 3);
                IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                IFormatProvider culture1 = new System.Globalization.CultureInfo("en-US", true);
                double temp;
                if (!Double.TryParse(s1 + "," + s2, NumberStyles.Float, culture, out temp))
                {
                    Double.TryParse(s1 + "," + s2, NumberStyles.Float, culture1, out temp);
                }
                return temp.ToString();
            }
            else
            {
                IFormatProvider culture = new System.Globalization.CultureInfo("vi-VN", true);
                IFormatProvider culture1 = new System.Globalization.CultureInfo("en-US", true);
                double temp;
                if (!Double.TryParse(str, NumberStyles.Float, culture, out temp))
                {
                    Double.TryParse(str, NumberStyles.Float, culture1, out temp);
                }
                return temp.ToString();
            }

        }
        public static List<string> GetListPortName()
        {
            //lấy danh sách cổng COM có trên máy
            List<string> portNames = new List<string>();
            foreach (string c in SerialPort.GetPortNames())
            {
                portNames.Add(c);
            }
            return portNames;
        }

        public static List<string> GetListStopBit()
        {
            //lấy danh sách Stopbits
            List<string> stopBits = new List<string>();
            foreach (string c in Enum.GetNames(typeof(StopBits)))
            {
                if (c != "None")
                    stopBits.Add(c);
            }
            return stopBits;
        }

        public static List<string> GetListParity()
        {
            //lấy danh sách Parity
            List<string> parity = new List<string>();
            foreach (string c in Enum.GetNames(typeof(Parity)))
            {
                parity.Add(c);
            }
            return parity;
        }

        public static List<string> GetListBaudrate()
        {
            string[] lstBaudRate = { "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            List<string> baudRates = new List<string>();
            foreach (string c in lstBaudRate)
            {
                baudRates.Add(c);
            }
            return baudRates;
        }

        public static List<string> GetListDataBits()
        {
            string[] lstDataBits = { "7", "8", "9" };
            List<string> dataBits = new List<string>();
            foreach (string c in lstDataBits)
            {
                dataBits.Add(c);
            }
            return dataBits;
        }
    }
    
    public class ComPortSetting
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public bool Rts { get; set; }
        public bool Dtr { get; set; }
        public int ReadTimeout { get; set; }
    }
}
