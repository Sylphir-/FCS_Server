using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    internal class ReturnStructures
    {
        public const byte Default = 0x00;
        public const byte XML = 0x01; //XML Type                       
        public const byte JSON = 0x02; //JSON Type(http://www.json.org/)
        public const byte Exclusion = 0xFF; //Return Structure Á¦¿Ü     
    }
}
