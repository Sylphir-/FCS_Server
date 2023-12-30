using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public class ConditionType
    {
        public const byte kCT_None = 0x00; //±âº»°ª         
        public const byte kCT_ConflictConfigurationVersion = 0x14; //¼³Á¤Á¤º¸ º¯°æµÊ
        public const byte kCT_ConflictProductVersion = 0x15; //»óÇ°Á¤º¸ º¯°æµÊ
        public const byte kCT_Running = 0x64; //¼­¹ö Á¤»ó °¡µ¿ 
        public const byte kCT_Stop = 0x65; //¼­¹ö ÁßÁö      
        public const byte kCT_Maintenance = 0xFF; //Á¡°Ë Áß        

    }
}