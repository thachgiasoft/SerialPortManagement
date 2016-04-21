using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        const string QuestionMark = "?";
        const string StarMark = "*";
        const string IndicateString = "........................";
        const string SeperateItem = "URISYS1100";
        readonly List<Urisys1100Dto> _urisys1100Items;
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
            _urisys1100Items = new List<Urisys1100Dto>();
        }

        public List<Urisys1100Dto> GetListResult()
        {
            if (_flag)
            {
                return _urisys1100Items;
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
                if (_data.Contains(IndicateString))
                {
                  //  EventLog.Log("Lấy thành công dữ liệu Urisys1100:\n" + _data);
                   // EventLog.Log(ParsingData() ? "Phân tích thành công Urisys1100!" : "Phân tích thất bại Urisys1100!");
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
                _data = _data.Replace(QuestionMark, "");
                int pos = _data.IndexOf(SeperateItem);
                if (pos > -1)
                {
                    _data = _data.Substring(pos);
                }
                _data = _data.Replace(IndicateString, "");
                _urisys1100Items.Clear();
                var items = Regex.Split(_data, SeperateItem);
                foreach (var item in items.Where(s => s != string.Empty))
                {
                    _data = _data.Replace(item, "");
                    var index = _data.IndexOf(SeperateItem) + SeperateItem.Length;
                    _data = _data.Substring(index, _data.Length - index);
                    var tmp = item.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Count() >= 17)
                        _urisys1100Items.Add(GetResult(tmp));
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
            var tmp = input[1].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //get ID
            var info = new Urisys1100Dto
            {
                ID = tmp[tmp.Length - 1]
            };
            //get date time
            tmp = Regex.Split(input[3], " ");
            info.Date = tmp[0];
            info.Time = tmp[tmp.Length - 1];
            //get pat.id
            tmp = Regex.Split(input[5].TrimEnd(), " ");
            if (input[5].TrimEnd().Length > 8)
            {
                info.Barcode = tmp[tmp.Length - 1];
            }
            else
            {
                info.Barcode = string.Empty;
            }
            //get order id
            tmp = Regex.Split(input[4].TrimEnd(), " ");
            info.TrayId = tmp[tmp.Length - 1];

            //Get list result
            for (var i = 7; i < input.Length; i++)
            {
                if (input[i].Count() < 4)
                    continue;
                var temp = input[i].TrimStart();
                tmp = temp.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                info.Result.Add(new Urisys1100ItemDto
                {
                    Code = tmp[0].Contains("*") ? tmp[1] : tmp[0],
                    Value = tmp[0].Contains("*") ? tmp[2] : tmp[1]
                });
            }
            return info;
        }
    }
}
