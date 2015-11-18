using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.DTO;

namespace ComManagement.Bo
{
    public class SysmexXP100Bo : ComHelperBase
    {
        public string _data;
        readonly string eot = ((char)04).ToString();
        readonly string enq = ((char)05).ToString();
        readonly string stx = ((char)02).ToString();
        readonly string etx = ((char)03).ToString();
        readonly string etb = ((char)17).ToString();
        readonly string eof = ((char)13).ToString() + ((char)10);
        readonly List<SysmexXP100Dto> _auItems;
        private bool _flag;


        public event EventHandler ReceiveDataComplelted;
        private void OnReceiveDataComplelted(bool p)
        {
            var handler = ReceiveDataComplelted;
            if (handler != null)
            {
                _flag = true;
                handler(this, new PropertyChangedEventArgs(p.ToString()));
            }
        }

        public SysmexXP100Bo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<SysmexXP100Dto>();
        }

        public List<SysmexXP100Dto> GetListResult()
        {
            if (_flag)
            {
                return _auItems;
            }
            return null;
        }
        public void WriteACKData()
        {
            try
            {
                if (portInstance.IsOpen != true) portInstance.Open();
                //convert the message to byte array
                byte[] newMsg =
                {
                    0x06
                };
                //send the message to the port
                portInstance.Write(newMsg, 0, newMsg.Length);
                Logger.Log("Ghi ack!");
            }
            catch (FormatException ex)
            {
                //display error message
                Logger.Log(ex.Message);
            }
        }
        private void portInstance_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var data = portInstance.ReadExisting();
                // Display the text to the user in the tmpinal
                _data += data;
                Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                if (data.Contains(eot))
                {
                    Logger.Log("Completed!" + "\n");
                    Logger.Log(ParsingData() ? "Phân tích thành công!" : "Phân tích thất bại!");
                    _flag = true;
                    OnReceiveDataComplelted(true);
                    _data = "";
                }
                else if (data.Contains(enq) | data.Contains(eof) | data.Contains(etx))
                {
                    WriteACKData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool ParsingData()
        {
            try
            {
                _data = Regex.Replace(_data, '\n'.ToString(), "");
                _data = Regex.Replace(_data, '\x0D'.ToString(), "");
                var records = Regex.Split(_data, stx);
                var item = new SysmexXP100Dto();
                foreach (var record in records)
                {
                    if (record.Count() > 50 & record.Contains(@"R|"))
                    {
                        var blocks = record.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        item.Name = blocks[6].TrimEnd();
                        item.TestTime = item.TestTime = blocks[7].Substring(0,14).AsDateTime();
                        var result = new SysmexXP100DtoResult
                        {
                            Result = blocks[3],
                            Unit = blocks[4],
                            Flag = blocks[5],
                        };
                        var nameStr = blocks[2].Split(new[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                        result.Code = nameStr[0];
                        item.Results.Add(result);
                    }
                   
                }
                _auItems.Add(item);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}

