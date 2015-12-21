using System.Collections.Generic;
using ComManagement.BO;

namespace ComManagement.Dto
{
    public class Celdyn3200Dto
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public Celdyn3200Dto()
        {
            Result = new List<TestItem>();
        }
        public List<TestItem> Result;
    }
    public class TestItem
    {
        public string Code { set; get; }
        public float Value { get; set; }
    }
}
