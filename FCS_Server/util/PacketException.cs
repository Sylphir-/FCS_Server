using FCS_Server.refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.util
{
    public class PacketException:Exception
    {
        public enum Codes {
            CONNECTION_READ_ONLY,
            INVALID_PACKET,
            INVALID_ECHO,
            INVALID_PACKET_TYPE,
            INVALID_PACKET_DATA
        }
        private Byte[] packet;
        private Codes code;

        /**
         * The packet sent to this class has to contain the 5-bit Header.
         */
        public PacketException( Byte[] _packet , Codes errorCode ) : base()
        {
            packet = _packet;
            code = errorCode;
        }

        private int GetTransactionID()
        {
            return BitConverter.ToInt32( packet , 1 );
        }

        private byte GetPacketType()
        {
            return packet[PacketStructure.PACKET_TYPE_OFFSET];
        }

        private String GetErrorMessage()
        {
            switch (code)
            {
                case Codes.CONNECTION_READ_ONLY:
                    return "Could not write to connection stream.";
                case Codes.INVALID_PACKET_DATA:
                    return "Packet data is invalid.";
                case Codes.INVALID_PACKET:
                    return "Packet format is invalid.";
                case Codes.INVALID_PACKET_TYPE:
                    return "Packet type is invalid.";
                case Codes.INVALID_ECHO:
                    return "Packet's Echo Content is invalid.";
                default:
                    return "Unidentified Exception has occurred.";
            }
        }

        private String GetPacketString()
        {
            return BitConverter.ToString( packet );
        }
        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "[{0:yyyyMMdd}][PACKET EXCEPTION][TRANSACTION ID: {1}][TRANSACTION TYPE: {2}]: {3}\n{4}" , DateTime.Now , GetTransactionID() , GetPacketType() , GetErrorMessage() , GetPacketString() );
            return sb.ToString();
        }
    }
}
