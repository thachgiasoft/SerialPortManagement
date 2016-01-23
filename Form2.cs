using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComManagement
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private SerialPort comm;
        private void button1_Click(object sender, EventArgs e)
        {
            comm = new SerialPort();

            comm.Parity = Parity.None;
            comm.PortName = "COM2";
            comm.StopBits = StopBits.One;
            comm.DataBits = 8;
            comm.BaudRate = 9600;
            comm.Open();
            comm.DataReceived += comm_DataReceived;
            var settingJson = Newtonsoft.Json.JsonConvert.SerializeObject(comm);
            Logger.Log(settingJson);
            label1.Text = "Thành công";
        }

        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var msg = comm.ReadExisting();
            DisplayData(msg);
        }
        [STAThread]
        private void DisplayData( string msg)
        {
            richTextBox1.Invoke(new EventHandler(delegate
            {
                richTextBox1.SelectedText = string.Empty;
                richTextBox1.AppendText(msg);
                richTextBox1.ScrollToCaret();
            }));
        }
    }
}
