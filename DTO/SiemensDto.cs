using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComManagement.Dto
{
    public class SiemensDto
    {
        public string Barcode { get; set; }
        public SiemensDto()
        {
            Result = new List<SiemensItemDto>();
        }
        public List<SiemensItemDto> Result;
    }
    public class SiemensItemDto
    {
        public string Code { set; get; }
        public string Value { get; set; }
    }
}
