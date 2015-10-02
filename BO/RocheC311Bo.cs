using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.DTO;

namespace ComManagement.Bo
{
    public class RocheC311Bo : ComHelperBase
    {
        public string _data;
        readonly string eot = ((char)04).ToString();
        readonly string enq = ((char)05).ToString();
        readonly string stx = ((char)02).ToString();
        readonly string etx = ((char)03).ToString();
        readonly string etb = ((char)23).ToString();
        readonly string eof = ((char)13).ToString() + ((char)10);
        readonly List<RocheC311Dto> _auItems;
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

        public RocheC311Bo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<RocheC311Dto>();
        }

        public List<RocheC311Dto> GetListResult()
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
                else if (data.Contains(enq) | data.Contains(eof))
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
                _data = Regex.Replace(_data, '\t'.ToString(), "");
                _data = Regex.Replace(_data, '\x0D'.ToString(), "");
                _data = Regex.Replace(_data, '\x0D'.ToString(), "");
                _data = Regex.Replace(_data, '\x0D'.ToString(), "");
                var patients = Regex.Split(_data, @"H\|\\");
                foreach (var patient in patients)
                {
                    if (patient.Count() < 100) continue;
                    var item = new RocheC311Dto();
                    var records = Regex.Split(patient, @"R\|");
                    foreach (var record in records)
                    {
                        var temp = Regex.Replace(record, etb + "FA" + stx + "3", "");
                        var frames = temp.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        var rgx = new Regex(@"^[0-9]*$");
                        if (temp.Contains("^&"))
                        {
                            var nameStr = frames[8].Split(new[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                            item.Name = nameStr[1] ?? "";
                            item.TestNo = nameStr[0] ?? "";
                        }
                        else if (frames[0] == "N")
                        {
                            item.OrderTime = item.TestTime = frames[5].AsDateTime();
                        }
                        else if (frames[1].Contains("^") & rgx.IsMatch(frames[0]))
                        {
                            var result = new RocheC311DtoResult
                            {
                                Result = frames[2],
                                Unit = frames[3],
                                Flag = frames[4],
                                Status = frames[5].Substring(0, 1),
                            };
                            var str = Regex.Split(frames[1], "/");
                            var codeStr = str[0].Replace('^', '0');
                            result.Code = (RocheC311Enum)int.Parse(codeStr);
                            item.Results.Add(result);
                        }
                    }
                    //add to local list
                    _auItems.Add(item);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}

