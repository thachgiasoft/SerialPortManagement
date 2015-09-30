using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ComManagement.Dto;

namespace ComManagement.Bo
{
    public class KX21IBo : ComHelperBase
    {
        string _data;
        const char END_RECORD = '\x03';
        List<KX21iDto> _kxItems;
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
        public KX21IBo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _kxItems = new List<KX21iDto>();
        }

        //void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    ParsingData(_data);
        //}
        public bool ParsingData(string data)
        {
            try
            {
                //_buffer.Clear();
                string[] slices;
                data = Regex.Replace(data, "\x02", "");
                data = Regex.Replace(data, "\n", "");

                slices = Regex.Split(data, END_RECORD.ToString());//cắt theo pattern
                // i = 0;
                _kxItems.Clear();
                for (int i = 0; i < slices.Length; i++)
                {
                    int count = slices[i].Length;
                    if ((count % 119) == 0 && count != 0)
                    {
                        KX21iDto item = new KX21iDto();
                        item = MappingToKX21iBo(slices[i]);
                        if (item.Code1 != null)
                        {
                            _kxItems.Add(item);
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
        public List<KX21iDto> GetListResult()
        {
            if (_flag)
            {
                return _kxItems;
            }
            return null;
        }
        void portInstance_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var tmpData = portInstance.ReadExisting();
                _data += tmpData;
                if (_data.Contains(END_RECORD))
                {
                    Logger.Log("Lấy thành công dữ liệu:\n" + _data);
                    if (ParsingData(_data))
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
                Logger.Log(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        internal KX21iDto MappingToKX21iBo(string str)
        {
            return new KX21iDto()
            {
                Code1 = str.Substring(0, 1),
                Code2 = str.Substring(1, 1),
                SampleCode = str.Substring(2, 1),
                Date = str.Substring(3, 6),
                AnalysInfo = str.Substring(9, 1),
                SampleID = str.Substring(10, 12).TrimStart('0'),
                PdaInfo = str.Substring(22, 6),
                RdwSelectInfo = str.Substring(28, 1),
                WBC = StringProcess(str.Substring(29, 5)),
                RBC = StringProcess(str.Substring(34, 5), 0),
                HGB = StringProcess(str.Substring(39, 5)),
                HCT = StringProcess(str.Substring(44, 5)),
                MCV = StringProcess(str.Substring(49, 5)),
                MCH = StringProcess(str.Substring(54, 5)),
                MCHC = StringProcess(str.Substring(59, 5)),
                PLT = StringProcess(str.Substring(64, 4), 1),
                LYM_SCR = StringProcess(str.Substring(69, 5)),
                MXD_MCR = StringProcess(str.Substring(74, 5)),
                NEUT_LCR = StringProcess(str.Substring(79, 5)),
                LYM_SCC = StringProcess(str.Substring(84, 5)),
                MXD_MCC = StringProcess(str.Substring(89, 5)),
                NEUT_LCC = StringProcess(str.Substring(94, 5)),
                RDW_SD = StringProcess(str.Substring(99, 5)),
                PDW = StringProcess(str.Substring(104, 5)),
                MPV = StringProcess(str.Substring(109, 5)),
                P_LCR = StringProcess(str.Substring(114, 5)),
            };
        }
    }
}
