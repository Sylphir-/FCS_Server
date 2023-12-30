using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public static class PacketProcess
    {
        public static Byte[] Initialize( Byte[] _packet )
        {

            /***************/
            // Get Condition Type
            Byte[] resultCode = BitConverter.GetBytes( ResultCode.kRCSuccess );

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse( resultCode );
            }

            Byte conditionType = ConditionType.kCT_Running;

            /**************/
            // Build Service Code
            Byte[] serviceCode = BuildServiceCode( _packet );

            /*****************/
            // Build World Number
            Byte[] worldNo = BuildWorldNo( _packet, serviceCode.Length );


            /*****************/
            // Build response byte array
            Byte[] response = new byte[resultCode.Length + 1 + serviceCode.Length + worldNo.Length];

            int pos = 0;
            for(int i=0; i<resultCode.Length; i++)
            {
                response[pos] = resultCode[i];
                pos++;
            }

            response[pos] = conditionType;
            pos++;

            for(int i=0; i < serviceCode.Length; i++)
            {
                response[pos] = serviceCode[i];
                pos++;
            }

            for(int i=0; i<worldNo.Length; i++)
            {
                response[pos] = worldNo[i];
                pos++;
            }

            return response;
        }

        public static Byte[] BuildWorldNo( Byte[] _packet, int ServiceCodeLength )
        {
            // Get World No Offset from Packet
            int worldNoOffset = PacketStructure.PACKET_SERVICE_CODE_OFFSET + ServiceCodeLength;

            // Create the Byte array
            Byte[] worldNo = new byte[PacketStructure.PACKET_WORLD_NO_LENGTH];

            // Populate Byte Array
            int j = worldNoOffset;
            for (int i = 0; i < PacketStructure.PACKET_WORLD_NO_LENGTH; i++)
            {
                worldNo[i] = _packet[j];
                j++;
            }

            return worldNo;
        }
        public static Byte[] BuildServiceCode( Byte[] _packet )
        {
            Byte[] serviceCode = new byte[PacketStructure.PACKET_SERVICE_CODE_LENGTH];
            serviceCode[0] = _packet[PacketStructure.PACKET_SERVICE_CODE_OFFSET];
            serviceCode[1] = _packet[PacketStructure.PACKET_SERVICE_CODE_OFFSET+1];
            serviceCode[2] = _packet[PacketStructure.PACKET_SERVICE_CODE_OFFSET+2];
            serviceCode[3] = _packet[PacketStructure.PACKET_SERVICE_CODE_OFFSET+3];

            Byte[] serviceCodeString = new byte[serviceCode[3]];

            Byte[] svcCode = new byte[serviceCode.Length + serviceCodeString.Length];

            int pos = PacketStructure.PACKET_SERVICE_CODE_OFFSET + serviceCode.Length;

            for (int i=0; i<svcCode.Length; i++)
            {
                if( i < serviceCode.Length)
                {
                    svcCode[i] = serviceCode[i];
                } else
                {
                    svcCode[i] = _packet[pos];
                    pos++;
                }
            }
            return svcCode;
        }
    }
}
