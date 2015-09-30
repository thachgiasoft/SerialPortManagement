using System;
using System.IO.Ports;
using ComManagement.Bo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestComManagement
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
             //setting
                var setting = new ComPortSetting();
            var roche = new RocheE4111Bo(setting);
            roche._data = @"
06 
1H|\^&|
||cobas-
e411^1||
|||host|
RSUPL^BA
TCH|P|1
P|1
O|1|
ngoc 14|
151^0001
^2^^S1^S
C|^^^125
^1\^^^10
^1\^^^50
^1|R||20
15091817
0041||||
N||||1||
|||||201
50918172
428|||F
R|1|^^^1
25/1/not
|13.87|p
mol/l||N
||F||bms
erv|||E1

R|2|^^^
10/1/not
|1.86|uI
U/ml||N|
|F51

";
            roche.ParsingData();
        }
    }
}
