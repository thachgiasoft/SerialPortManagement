using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComManagement.Dto
{
    public class KX21iDto
    {
        /// <summary>
        /// Parameter: Text distinction code I
        /// No. of Characters: 1
        /// Example:   "D"
        /// 
        /// </summary>
        public string Code1
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: RDW select information
        /// No. of Characters: 1
        /// Example:   "C"
        /// </summary>
        public string RdwSelectInfo
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Not Use
        /// No. of Characters: 1
        /// Example:   "0"
        /// </summary>
        public string NotUse
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: QC File Number
        /// No. of Characters: 1
        /// Example:   "5"
        /// </summary>
        public string QcNo
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Text distinction code II
        /// No. of Characters: 1
        /// Example:   "1"
        /// </summary>
        public string Code2
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Sample distinction code
        /// No. of Characters: 1
        /// Example:   "C"
        /// </summary>
        public string SampleCode
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Data distinction code
        /// No. of Characters: 1
        /// Example:   "X"
        /// </summary>
        public string DataCode
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Date tim
        /// No. of Characters: 6 with 121 and 10 with 99
        /// Example:   "12/06/12 09:54"
        /// </summary>
        public string Date
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Analysis information
        /// No. of Characters: 1
        /// Example:   "O"
        /// </summary>
        public string AnalysInfo
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: Sample ID No
        /// No. of Characters: 12
        /// Example:   "OOOOOOOOOOOO"
        /// </summary>
        public string SampleID
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: PDA information

        /// No. of Characters: 6
        /// Example:   "OOOOOO"
        /// </summary>
        public string PdaInfo
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: WBC [x10^3/microL]
        /// No. of Characters: 4 or 5
        /// Example:   "xxx.x"
        /// </summary>
        public string WBC
        {
            set;
            get;
        }
        /// <summary>
        ///Parameter: RBC [´106/mL]
        /// No. of Characters: 4 or 5
        /// Example:   "xxx.x"
        /// </summary>
        public string RBC
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: HGB
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string HGB
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: HCT
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string HCT
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MCV [fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MCV
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MCH [pg]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MCH
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MCHC [g/dL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MCHC
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: PLT [´103/mL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string PLT
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: LYM% (W-SCR) [%]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string LYM_SCR
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MXD% (W-MCR) [%]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MXD_MCR
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: NEUT% (W-LCR) [%]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string NEUT_LCR
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: NEUT# (W-LCC) [´103/mL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string NEUT_LCC
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MXD# (W-MCC) [´103/mL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MXD_MCC
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: LYM# (W-SCC) [´103/mL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string LYM_SCC
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: RDW-(CV/SD) [%/fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string RDW_SD
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter:PDW [fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string PDW
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: MPV [fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string MPV
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: P-LCR [%]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string P_LCR
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: W-SMV [fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string W_SMV
        {
            set;
            get;
        }
        /// <summary>
        /// Parameter: W-LMV [fL]
        /// No. of Characters: 4
        /// Example:   "xxx.x"
        /// </summary>
        public string W_LMV
        {
            set;
            get;
        }
    }
}
