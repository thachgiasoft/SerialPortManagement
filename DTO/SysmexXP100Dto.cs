using System;
using System.Collections.Generic;

namespace ComManagement.DTO
{
    public class SysmexXP100Dto
    {
        public SysmexXP100Dto()
        {
            Results = new List<SysmexXP100DtoResult>();
        }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime TestTime { get; set; }
        public List<SysmexXP100DtoResult> Results;
    }

    public class SysmexXP100DtoResult
    {
        public string Code { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public string Flag { get; set; }
    }
}
