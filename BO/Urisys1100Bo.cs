using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.Bo;
using ComManagement.Dto;

namespace ComManagement.BO
{
    public class Urisys1100Bo : ComHelperBase
    {
        string _data;
        const string IndicateString = "........................";
        const string SeperateItem = "URISYS1100";
        readonly List<Urisys1100Dto> _auItems;
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

        public Urisys1100Bo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<Urisys1100Dto>();
        }

        public List<Urisys1100Dto> GetListResult()
        {
            if (_flag)
            {
                return _auItems;
            }
            return null;
        }

        private void portInstance_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var data = portInstance.ReadExisting();
                // Display the text to the user in the tmpinal
                _data += data;
                Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                if (_data.Contains(IndicateString))
                {
                    Logger.Log(ParsingData() ? "Phân tích thành công!" : "Phân tích thất bại!");
                    _flag = true;
                    OnReceiveDataComplelted(true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        private bool ParsingData()
        {
            try
            {
                Logger.Log("data: " + _data);
                _data = _data.Replace(IndicateString, "");
                _auItems.Clear();
                var items = Regex.Split(_data, SeperateItem);
                foreach (var item in items.Where(s=>s!=string.Empty))
                {
                    _data = _data.Replace(item, "");
                    var index = _data.IndexOf(SeperateItem)+SeperateItem.Length;
                    _data = _data.Substring(index,_data.Length-index);
                    var tmp=Regex.Split(item,"\n");
                    if (tmp.Count() > 17)
                        _auItems.Add(GetResult(tmp));
                }
              
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        Urisys1100Dto GetResult(string[] input)
        {
            var tmp = Regex.Split(input[1], " ");
            //get barcode
            var info = new Urisys1100Dto
            {
                Barcode = tmp[tmp.Length - 1]
            };
            //get date time
            tmp = Regex.Split(input[3], " ");
            info.Date = tmp[0];
            info.Time = tmp[tmp.Length - 1];
            //get id
            tmp = Regex.Split(input[5], " ");
            info.ID = tmp[tmp.Length - 1];
            //get order id
            tmp = Regex.Split(input[4], " ");
            info.TrayId = tmp[tmp.Length - 1];
            //Get list result
            for (var i = 7; i < input.Length; i++)
            {
                if (input[i].Count()<4)
                    break;
                var temp = input[i].TrimStart();
                tmp = temp.Split(" ".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                info.Result.Add(new Urisys1100ItemDto
                {
                    Code = tmp[0].Contains("*")?tmp[1]:tmp[0],
                    Value = tmp[tmp.Length - 1]
                });
            }
            return info;
        }
    }
}
