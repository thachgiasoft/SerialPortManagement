using System.IO.Ports;
using ComManagement.Bo;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //setting
            var setting = new ComPortSetting
            {
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                PortName = "Com1",
                StopBits = StopBits.One,
                ReadTimeout = 1000
            };
            var roche = new RocheE4111Bo(setting);
            roche._data = @"1H|\^&|
||cobas-
e411^1||
|||host|
RSUPL^BA
TCH|P|1
P|1
O|1|
phuc 12|
129^0001
^1^^S1^S
C|^^^125
^1\^^^10
^1\^^^50
^1|R||20
15091010
5229||||
N||||1||
|||||201
50910111
605|||F
R|1|^^^1
25/1/not
|15.18|p
mol/l||N
||F||bms
erv|||E1

R|2|^^^
10/1/not
|4.93|uI
U/ml||H|
|F3F

06 
2||bmse
rv|||E1
C|1|I|40
|I
R|3|^
^^50/1/n
ot|2.01|
nmol/l||
N||F||bm
serv|||E
1
L|1|N
D2

";
            roche.ParsingData();
        }
    }
}
