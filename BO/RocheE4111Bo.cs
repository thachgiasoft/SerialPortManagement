using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.DTO;

namespace ComManagement.Bo
{
    public class RocheE4111Bo : ComHelperBase
    {
        public string _data;
        readonly string eot = ((char)04).ToString();
        readonly string enq = ((char)05).ToString();
        //readonly string stx = ((char)02).ToString();
      //  readonly string etx = ((char)03).ToString();
        readonly string etb = ((char)17).ToString();
        readonly string eof = ((char)13).ToString() + ((char)10);
        readonly List<Roche4111Dto> _auItems;
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

        public RocheE4111Bo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<Roche4111Dto>();
        }

        public List<Roche4111Dto> GetListResult()
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
                _data = Regex.Replace(_data, '\x0D'.ToString(), "");
                var patients = Regex.Split(_data, @"H\|\\");
                foreach (var patient in patients)
                {
                    if (patient.Count() < 100) continue;
                    var item = new Roche4111Dto();
                    var blocks = Regex.Split(patient, etb);
                    foreach (var block in blocks)
                    {
                        if (block.Count() <= 1) continue;
                        var records = Regex.Split(block, @"R\|");
                        foreach (var record in records)
                        {
                            var frames = record.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            if (record.Contains("^&"))
                            {
                                item.Name = frames[8] ?? "";
                            }
                            else if (frames[0].Count() == 14)
                            {
                                item.OrderTime = frames[0].AsDateTime();
                                item.TestTime = frames[3].AsDateTime();
                            }
                            else if (frames[1].Contains("^"))
                            {
                                var result = new Roche4111DtoResult
                                {
                                    Result = frames[2],
                                    Unit = frames[3],
                                    Flag = frames[4],
                                    Status = frames[5].Substring(0, 1),
                                };
                                var str = Regex.Split(frames[1], "/");
                                var codeStr = str[0].Replace('^', '0');//Regex.Replace(str[0], "^", "");
                                result.Code = int.Parse(codeStr);
                                item.Results.Add(result);
                            }
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
    public static class Extensions
    {
        public static DateTime AsDateTime(this string input)
        {
            return DateTime.ParseExact(input, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

