﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using ComManagement.Dto;

namespace ComManagement.Bo
{
    public class SiemensBo : ComHelperBase
    {
        string _data;
        const string EndItem = ",,";
        const string SeperateItem = ",";
        const string SeperateRecord = ",,,,,,";
        const string AlterSeperateRecord = ",,,";
        readonly List<SiemensDto> _auItems;
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

        public SiemensBo(ComPortSetting set)
            : base(set)
        {
            portInstance.DataReceived += portInstance_DataReceived;
            _auItems = new List<SiemensDto>();
        }

        public List<SiemensDto> GetListResult()
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
                if (_data.Length > 264)
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
                //_buffer.Clear();
                string[] slices;

                _data = Regex.Replace(_data, "\t", "");
                _data = Regex.Replace(_data, @"\s", "");
                _data = Regex.Replace(_data, @"\n", "");
                //cắt theo từng bản ghi
                if (_data.Contains(SeperateRecord))
                {
                    slices = Regex.Split(_data, SeperateRecord);
                    _auItems.Clear();
                    var exceedString = _data;

                    for (int i = 0; i < slices.Length; i++)
                    {
                        Logger.Log("Bản ghi thứ " + i);
                        if (slices[i].Contains("Multistix"))
                        {
                            var pa = GetPatientInfo(slices[i - 1].GetLast(28));//lấy thông tin bệnh nhân theo bản ghi trước đó

                            exceedString = exceedString.Replace(slices[i - 1], "");
                            Logger.Log("Chuỗi còn lại: " + exceedString);
                            var index = slices[i].IndexOf("GLU");//tìm vị trí của bản ghi glu
                            var subResult = slices[i].Substring(index, slices[i].Length - index);//xóa đoạn trước
                            //nếu không phải bản ghi cuối xóa đoạn sau
                            if (i < slices.Length - 1)
                            {
                                subResult = subResult.Substring(0, subResult.Length - 28);
                            }
                            subResult = Regex.Replace(subResult, Regex.Escape("*"), "");
                            var result = GetResult(ref subResult).ToList();
                            if (result.Count == 10)
                            {
                                _auItems.Add(new SiemensDto
                                {
                                    Barcode = pa.BarCode,
                                    Result = result
                                });
                                exceedString = subResult;
                                Logger.Log("Chuỗi còn lại: " + exceedString);
                            }
                        }
                    }
                    _data = exceedString;
                    return true;
                }
                else
                {
                    slices = Regex.Split(_data, AlterSeperateRecord);
                    _auItems.Clear();
                    var exceedString = _data;

                    for (int i = 0; i < slices.Length; i++)
                    {
                        Logger.Log("Bản ghi thứ " + i);
                        if (slices[i].Contains("Multistix"))
                        {
                            var pa = GetPatientInfo(slices[i - 1].GetLast(39),true);//lấy thông tin bệnh nhân theo bản ghi trước đó

                            exceedString = exceedString.Replace(slices[i - 1], "");
                            Logger.Log("Chuỗi còn lại: " + exceedString);
                            var index = slices[i].IndexOf("GLU");//tìm vị trí của bản ghi glu
                            var subResult = slices[i].Substring(index, slices[i].Length - index);//xóa đoạn trước
                            //nếu không phải bản ghi cuối xóa đoạn sau
                            if (i < slices.Length - 1)
                            {
                                subResult = subResult.Substring(0, subResult.Length - 28);
                            }
                            subResult = Regex.Replace(subResult, Regex.Escape("*"), "");
                            var result = GetResult(ref subResult).ToList();
                            if (result.Count == 10)
                            {
                                _auItems.Add(new SiemensDto
                                {
                                    Barcode = pa.BarCode,
                                    Result = result
                                });
                                exceedString = subResult;
                                Logger.Log("Chuỗi còn lại: " + exceedString);
                            }
                        }
                    }
                    _data = exceedString;
                    return true;
                }
               
            }
            catch (Exception)
            {

                return false;
            }

        }

        IEnumerable<SiemensItemDto> GetResult(ref string input)
        {
            var results = Regex.Split(input, EndItem);
            var ret = from result in results
                      select Regex.Split(result, SeperateItem) into tmp
                      where (tmp.Count() > 1) & (results.Count() == 10)
                      select new SiemensItemDto
                      {
                          Code = tmp[0],
                          Value = tmp[1]
                      };
            var siemensItemDtos = ret as SiemensItemDto[] ?? ret.ToArray();
            var temp = "";
            foreach (var item in siemensItemDtos.ToList())
            {
                temp = temp + item.Code;
                temp = temp + ",";
                temp = temp + item.Value;
                temp = temp + ",,";

            }
            temp = temp.Remove(temp.Length - 2, 2);
            input = input.Replace(temp, "");
            return siemensItemDtos;
        }

        static PatientInfo GetPatientInfo(string input,bool isAlt=false)
        {
            if (isAlt)
            {
                var temp = Regex.Split(input, ",");
                return new PatientInfo
                {
                    ID = temp[0],
                    Date = temp[1],
                    Time = temp[2],
                    TrayId = temp[3],
                    BarCode = temp[4],
                    Name = temp[5]
                };
            }
            return new PatientInfo
            {
                BarCode = input.Substring(22, 6),
                ID = input.Substring(0, 4),
                //Name = input.Substring(10, input.Length - 10)
            };
        }
    }
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }

}
