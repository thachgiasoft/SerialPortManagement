using System.Collections.Generic;

namespace ComManagement.Dto
{
    public class Urisys1100Dto
    {
         public bool IsIndicateError { set; get; }
         public string Barcode { get; set; }
         public string TrayId { get; set; }
         public string ID { get; set; }
         public string Date { set; get; }
         public string Time { set; get; }

        public Urisys1100Dto()
        {
            Result = new List<Urisys1100ItemDto>();
        }
        public List<Urisys1100ItemDto> Result;
    }
    public class Urisys1100ItemDto
    {
        public string Code { set; get; }
        public string Value { get; set; }
    }
}
