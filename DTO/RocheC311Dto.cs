using System;
using System.Collections.Generic;

namespace ComManagement.DTO
{
    public class RocheC311Dto
    {
        public RocheC311Dto()
        {
            Results = new List<RocheC311DtoResult>();
        }
        public string Name { get; set; }
        public string TestNo { get; set; }
        public string Barcode { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime TestTime { get; set; }
        public List<RocheC311DtoResult> Results;
    }

    public class RocheC311DtoResult
    {
        public RocheC311Enum Code { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public string Flag { get; set; }
    }
    public enum RocheC311Enum
    {
        Unknow = 0,
        CRPLX = 19,
        LDL_C = 59,
        GGTI2 = 220,
        ALB2 = 413,
        URELU = 417,
        UREAL = 418,
        HDLC3 = 435,
        TP2 = 678,
        ALTL = 685,
        ASTL = 687,
        CREJ2 = 690,
        UA2 = 700,
        UA2_U = 702,
        GLUC3 = 717,
        BILD2 = 734,
        TRIGL = 781,
        CHO2I = 798,
        Na = 989,
        K = 990,
        CL = 991,
        L = 992,
        H = 993,
        I = 994
    }
}
