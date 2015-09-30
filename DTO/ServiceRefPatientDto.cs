using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComManagement.Dto
{
    public class ServiceRefPatientDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string TestId { get; set; }
        public string TestValue { get; set; }
    }
}
