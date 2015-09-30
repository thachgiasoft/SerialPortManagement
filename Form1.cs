using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using ComManagement.Bo;
using ComManagement.DTO;

namespace ComManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ListPortCom();

        }

        private RocheE4111Bo au;
        public void ListPortCom()
        {

            foreach (string cong in SerialPort.GetPortNames())
            {
                cbPortName.Items.Add(cong);
            }

        }
        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                //setting
                var setting = new ComPortSetting()
                {
                    BaudRate = int.Parse(cbBaudRate.Text),
                    DataBits = int.Parse(cbDataBits.Text),
                    StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBits.Text),
                    Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.Text),
                    PortName = cbPortName.Text,
                    ReadTimeout = 1000,
                    Rts = chkRts.Checked,
                    Dtr = chkDtr.Checked
                };
                // khai báo và mở cổng 
                au = new RocheE4111Bo(setting);
                au.Open();
                Log("Mở cổng thành công");
                btnGet.Enabled = false;
                // khai báo sự kiện nhận được dữ liệu thành công 
                au.ReceiveDataComplelted += au_ReceiveDataComplelted;
            }
            catch (Exception ex)
            {
                Log("Mở cổng thất bại");
            }
        }

        private void au_ReceiveDataComplelted(object sender, EventArgs e)
        {
            var t = au.GetListResult();
            HT(t);

            //var dat = t.SelectMany(a => a.Results, (a, b) => new ServiceRefPatientDto
            //{
            //    Barcode = a.Name,
            //    TestId = b.Code.ToString(),
            //    TestValue = b.Result
            //}).ToList();
            ////DACommon db=new DACommon();
            ////db.InsertServiceRefPatients(dat);
        }


        public delegate void Dellog(string mes);
        public void Log(string meg)
        {
            if (statusStrip1.InvokeRequired)
            {
                this.Invoke(new Dellog(Log), meg);
            }
            else
            {
                toolStripStatusLabel1.Text = meg;
                Logger.Log(meg);
            }

        }
        public delegate void DelHT(List<Roche4111Dto> data);
        public void HT(List<Roche4111Dto> data)
        {
            if (dataGridView1.InvokeRequired)
            {
                this.Invoke(new DelHT(HT), data);
            }
            else
            {
                dataGridView1.DataSource = data.SelectMany(a => a.Results, (a, b) => new
                {
                    BenhNhan = a.Name,
                    TestCode = (int)b.Code,
                    TestName = b.Code,
                    b.Unit,
                    b.Result,
                    b.Status,
                    b.Flag
                }).ToList();

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // au.Test();
            //var a = au.GetListResult();
            //hienThi.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbBaudRate.DataSource = ComHelperBase.GetListBaudrate();
            cbBaudRate.SelectedIndex = 2;
            cbDataBits.DataSource = ComHelperBase.GetListDataBits();
            cbDataBits.SelectedIndex = 1;
            cbStopBits.DataSource = ComHelperBase.GetListStopBit();
            cbParity.DataSource = ComHelperBase.GetListParity();
            cbPortName.DataSource = ComHelperBase.GetListPortName();
        }
    }
}
