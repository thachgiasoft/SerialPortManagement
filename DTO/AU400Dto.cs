using System.Collections.Generic;

namespace ComManagement.Dto
{
    public class AU400Dto
    {
        //Trường nhận biết xem dữ liệu có toàn vẹn hay không?
        public bool IsIndicateError { set; get; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public AU400Dto()
        {
            Result = new List<AU400ItemDto>();
        }
        public List<AU400ItemDto> Result;
    }
    public class AU400ItemDto
    {
        public string Code { set; get; }
        public string Value { get; set; }
    }
}
