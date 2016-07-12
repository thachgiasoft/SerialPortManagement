using System;
using System.Collections.Generic;

namespace ComManagement.DTO
{
    public class Roche4111Dto
    {
        public Roche4111Dto()
        {
            Results = new List<Roche4111DtoResult>();
        }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime TestTime { get; set; }
        public List<Roche4111DtoResult> Results;
    }

    public class Roche4111DtoResult
    {
        public Roche4111Enum Code { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public string Flag { get; set; }
    }
    public enum Roche4111Enum
    {
        Unknow = 0,
        /// <summary>
        /// TSH 0
        /// </summary>
        TSH0 = 10,
        /// <summary>
        /// T3 0
        /// </summary>
        T30 = 50,
        /// <summary>
        /// FT4-II 0
        /// </summary>
        FT4_II0 = 125,
        /// <summary>
        /// CEA 1
        /// </summary>
        CEA_1 = 301,
        /// <summary>
        /// AFP 1
        /// </summary>
        AFP_1 = 311,
        TPSA_1 = 321,
        /// <summary>
        /// CA15-3 2
        /// </summary>
        CA15_3_2 = 332,
        /// <summary>
        /// CA125 1
        /// </summary>
        CA125_1 = 341,
        CA19_91 = 351,
        CA72_40 = 360,
        CYFRA_0 = 370,
    }
}
