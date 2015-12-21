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
    public class Celdyn3200 : ComHelperBase
    {
        string _data;
        const string BeginPattern = "\x02";
        const string EndPattern = "\x03";
        readonly List<Celdyn3200Dto> _cendynItems;
        private bool _flag = false;

        public event EventHandler ReceiveDataComplelted;
        private void OnReceiveDataComplelted(bool p)
        {
            EventHandler handler = ReceiveDataComplelted;
            if (handler != null)
            {
                _flag = true;
                handler(this, new PropertyChangedEventArgs(p.ToString()));
            }
        }
        public Celdyn3200(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _cendynItems = new List<Celdyn3200Dto>();
        }
        public List<Celdyn3200Dto> GetListResult()
        {
            if (_flag)
            {
                return _cendynItems;
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
                if (data.Contains(EndPattern))
                {
                    Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                    if (ParsingPata())
                    {
                        Logger.Log("Phân tích thành công!");
                    }
                    else
                    {
                        Logger.Log("Phân tích thất bại!");
                    }
                    _flag = true;
                    OnReceiveDataComplelted(true);
                    _data = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        private bool ParsingPata()
        {
            try
            {
                string[] slices;
                _data = Regex.Replace(_data, "\t", "");
                _data = Regex.Replace(_data, @"\s", "");
                _data = Regex.Replace(_data, @"\n", "");
                slices = Regex.Split(_data, EndPattern);//cắt theo pattern bắt đầu bản ghi
                foreach (var slice in slices)
                {
                    var input = Regex.Split(slice, ",");
                    if (slice.Length>10)
                    {
                        _cendynItems.Add(new Celdyn3200Dto
                        {
                            Barcode = input[3],
                            Result = GetResult(input)
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

        static List<TestItem> GetResult(string[] input)
        {
            var temp = new List<TestItem>();
            //hồng cầu
            temp.Add(new TestItem
            {
                Code = "HC",
                Value = input[24].ToFloat()
            });
            //huyết sắc tố
            temp.Add(new TestItem
            {
                Code = "HST",
                Value = input[25].ToFloat() * 10
            });
            //Hematocrite
            temp.Add(new TestItem
            {
                Code = "HEM",
                Value = (float)Math.Round((input[26].ToFloat() / 100), 3)
            });
            //Thể tích trung bình Hồng cầu
            temp.Add(new TestItem
            {
                Code = "TCHC",
                Value = input[27].ToFloat()
            });
            //Lượng HST trung bình Hồng cầu
            temp.Add(new TestItem
            {
                Code = "LHST",
                Value = input[28].ToFloat()
            });
            //Nồng độ HST trung bình Hồng cầu
            temp.Add(new TestItem
            {
                Code = "NHST",
                Value = input[29].ToFloat() * 10
            });
            //Dải phân bố Hồng cầu
            temp.Add(new TestItem
            {
                Code = "PBHC",
                Value = input[30].ToFloat()
            });
            //Số lượng Bạch cầu (BC)
            temp.Add(new TestItem
            {
                Code = "SLBC",
                Value = input[18].ToFloat()
            });
            //Tỷ lệ % Bạch cầu trung tính 
            temp.Add(new TestItem
            {
                Code = "TLBC",
                Value = input[35].ToFloat()
            });
            //Tỷ lệ % Bạch cầu lympho 
            temp.Add(new TestItem
            {
                Code = "TLBCL",
                Value = input[36].ToFloat()
            });
            //Tỷ lệ % Bạch cầu Mono
            temp.Add(new TestItem
            {
                Code = "TLBCM",
                Value = input[37].ToFloat()
            });
            //Tỷ lệ % Bạch cầu ưa acid
            temp.Add(new TestItem
            {
                Code = "TLBCA",
                Value = input[38].ToFloat()
            });
            //Tỷ lệ % Bạch cầu ưa bazơ
            temp.Add(new TestItem
            {
                Code = "TLBCB",
                Value = input[39].ToFloat()
            });
            //Số lượng BC trung tính
            temp.Add(new TestItem
            {
                Code = "BCTT",
                Value = input[19].ToFloat()
            });
            //Số lượng Bạch cầu lympho 
            temp.Add(new TestItem
            {
                Code = "BCL",
                Value = input[20].ToFloat()
            });
            //Số lượng Bạch cầu Mono
            temp.Add(new TestItem
            {
                Code = "BCM",
                Value = input[21].ToFloat()
            });
            //Số lượng Bạch cầu ưa acid
            temp.Add(new TestItem
            {
                Code = "BCA",
                Value = input[22].ToFloat()
            });
            //Số lượng Bạch cầu ưa bazơ
            temp.Add(new TestItem
            {
                Code = "BCB",
                Value = input[23].ToFloat()
            });
            //Số lượng Tiểu cầu
            temp.Add(new TestItem
            {
                Code = "TC",
                Value = input[31].ToFloat()
            });
            //Thể tích trung bình Tiểu cầu
            temp.Add(new TestItem
            {
                Code = "TTTC",
                Value = input[32].ToFloat()
            });
            //Thể tích khối tiểu cầu
            temp.Add(new TestItem
            {
                Code = "TTKTC",
                Value = input[33].ToFloat() * 10
            });
            //Dải phân bố Tiểu cầu
            temp.Add(new TestItem
            {
                Code = "PBTC",
                Value = input[34].ToFloat()
            });
            return temp;
        }

        PatientInfo GetPatientInfo(string input)
        {
            return new PatientInfo()
            {
                BarCode = input.Substring(0, 6),
                ID = input.Substring(6, 4),
                Name = input.Substring(10, input.Length - 10)
            };
        }
    }
  
}
