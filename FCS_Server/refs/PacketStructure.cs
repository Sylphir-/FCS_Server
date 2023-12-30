using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public class PacketStructure
    {
        public const short PACKET_HEADER_STRUCTURE_LENGTH = 5;
        public const short PACKET_HEADER_PACKET_LENGTH = 4;
        public const short PACKET_ECHO_CONTENT_OFFSET = 0;
        public const short PACKET_ECHO_CONTENT_LENGTH = 5;
        public const short PACKET_ECHO_CONTENT_PACKET_TYPE_OFFSET = 0;
        public const short PACKET_ECHO_CONTENT_TRANSACTION_ID_OFFSET = 1;
        public const short PACKET_ECHO_CONTENT_TRANSACTION_ID_LENGTH = 4;
        public const short INITIALIZE_RESPONSE_LENGTH = 13;
        public const short PACKET_SERVICE_CODE_OFFSET = 5;
        public const short PACKET_SERVICE_CODE_LENGTH = 4;
        public const short PACKET_WORLD_NO_OFFSET = 8;
        public const short PACKET_WORLD_NO_LENGTH = 4;
    }
}
