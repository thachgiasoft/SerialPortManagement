using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.Dto;

namespace ComManagement.Bo
{
    public class Au400Bo : ComHelperBase
    {
        private readonly bool _isContinueTransfer = false;
        string _data;
        private const string EndResult = "r";
        const string StartRecord = "\x02" + "D";
        const string StartResult = "E0";
        readonly List<AU400Dto> _auItems;
        readonly string stx = ((char)02).ToString();
        readonly string etx = ((char)03).ToString();
        private bool _flag;

        public event EventHandler ReceiveDataComplelted;
        private void OnReceiveDataComplelted(bool p)
        {
            var handler = ReceiveDataComplelted;
            if (handler == null) return;
            _flag = true;
            handler(this, new PropertyChangedEventArgs(p.ToString()));
        }


        public Au400Bo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<AU400Dto>();
        }
        public Au400Bo(ComPortSetting set, bool isContinueTransfer)
            : base(set)
        {
            _isContinueTransfer = isContinueTransfer;
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<AU400Dto>();
        }
        public List<AU400Dto> GetListResult()
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
                string data = portInstance.ReadExisting();
                // Display the text to the user in the tmpinal
                _data += data;
                var index = _data.LastIndexOf(etx + etx + stx + stx + "D", StringComparison.Ordinal);
                if (data.IndexOf("DE", StringComparison.Ordinal) != -1)
                {
                    Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                    Logger.Log(ParsingData() ? "Phân tích thành công!" : "Phân tích thất bại!");
                    _flag = true;
                    OnReceiveDataComplelted(true);
                    _data = "";
                }
                else if (index != -1
                    & _isContinueTransfer)
                {
                    Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                    Logger.Log(ParsingData() ? "Phân tích thành công!" : "Phân tích thất bại!");
                    _flag = true;
                    OnReceiveDataComplelted(true);
                    _data = _data.Substring(index + 5);
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
                //_buffer.Clear();
                string[] slices;
                if (_data.Contains('r'))
                {
                    _data = Regex.Replace(_data, "\t", "");
                    _data = Regex.Replace(_data, @"\s", "");
                    _data = Regex.Replace(_data, @"\n", "");
                }

                slices = Regex.Split(_data, StartRecord);//cắt theo pattern bắt đầu bản ghi
                _auItems.Clear();
                var isError = false;
                foreach (string item in slices)
                {
                    if (!item.Contains(StartResult)) continue;
                    var tmp = Regex.Split(item, StartResult);
                    if (tmp.Length <= 1) continue;
                    var pa = GetPatientInfo(tmp[0]);
                    if (_data.Contains('r'))
                    {


                        var result = GetResult(tmp[1], true, er => isError = er).ToList();
                        _auItems.Add(new AU400Dto
                        {
                            Barcode = pa.BarCode,
                            Name = pa.Name,
                            Result = result,
                            IsIndicateError = isError
                        });
                    }
                    else
                    {
                        var result = GetResult(tmp[1], false, er => isError = er).ToList();
                        _auItems.Add(new AU400Dto
                        {
                            Barcode = pa.BarCode,
                            Name = pa.Name,
                            Result = result,
                            IsIndicateError = isError
                        });
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        static IEnumerable<AU400ItemDto> GetResult(string input, bool isRChar, Action<bool> isError)
        {
            isError(false);
            if (isRChar)//Có kí tự 'r' phân cách
            {
                var results = Regex.Split(input, EndResult);
                foreach (var result in results)
                {
                    if (result.Length >= 4)
                    {
                        yield return new AU400ItemDto
                        {
                            Code = result.Substring(0, 2),
                            Value = result.Substring(2, result.Length - 2)
                        };
                    }
                }
            }
            else//Không có kí tự 'r' phân cách
            {
                input = input.TrimStart();
                input = Regex.Replace(input, "\x02", "");
                input = Regex.Replace(input, "\x03", "");
                if (input.Contains("\x28"))
                    input = Regex.Replace(input, "\x28", "");
                if (input.Contains("\x29"))
                    input = Regex.Replace(input, "\x29", "");
                var results = Regex.Split(input, " ");
                var isCode = false;
                var code = "";
                foreach (var result in results.Where(result => result != ""))
                {
                    if (result.Contains("J"))
                    {
                        isError(true);
                        break;
                    }

                    if (!isCode)
                    {
                        code = result;
                        isCode = true;
                    }
                    else
                    {
                        isCode = false;
                        yield return new AU400ItemDto
                        {
                            Code = code,
                            Value = result
                        };
                    }
                }
            }
        }

        static PatientInfo GetPatientInfo(string input)
        {
            input = Regex.Replace(input, "\t", "");
            input = Regex.Replace(input, @"\s", "");
            input = Regex.Replace(input, @"\n", "");
            return new PatientInfo
            {
                BarCode = input.Substring(0, 6),
                ID = input.Substring(6, 4),
                Name = input.Substring(10, input.Length - 10)
            };
        }

        public static float ToFloat(string input)
        {
            float tmp;
            float.TryParse(input, out tmp);
            return tmp;
        }
    }
    public class PatientInfo
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string TrayId { get; set; }
        public string ID { get; set; }
        public string Date { set; get; }
        public string Time { set; get; }
    }
}
