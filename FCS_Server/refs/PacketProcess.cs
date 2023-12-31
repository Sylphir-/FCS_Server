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
            // Response: Header - Echo Content - Result Code - Condition Type - Service Code - World Number
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.INITIALIZE_RESULT_CODE_LENGTH + 1 + serviceCode.Length + PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH];

            // Copy header and echo content into response
            Buffer.BlockCopy( _packet , PacketStructure.HEADER_OFFSET , response , PacketStructure.HEADER_OFFSET , PacketStructure.HEADER_LENGTH+PacketStructure.ECHO_CONTENT_LENGTH );

            // Copy Result Code into response
            Buffer.BlockCopy( resultCode , 0 , response , PacketStructure.INITIALIZE_RESULT_CODE_OFFSET , resultCode.Length );

            // Copy condition type into response
            response[PacketStructure.INITIALIZE_CONDITION_TYPE_OFFSET] = conditionType;

            // Copy Service Code into response
            Buffer.BlockCopy( serviceCode , 0 , response , PacketStructure.INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET , serviceCode.Length );
            Buffer.BlockCopy( worldNo , 0 , response , PacketStructure.INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET + serviceCode.Length , worldNo.Length );

            return response;
        }

        public static Byte[] KeepAlive( Byte[] _packet )
        {
            // Copy Caching Product Version to memory
            long CachingProductVersion = BitConverter.ToInt64( _packet , PacketStructure.KEEPALIVE_CACHING_PRODUCT_VERSION_OFFSET );

            // Get Result Code
            int resultCode = ResultCode.kRCSuccess;

            // Get Condition Type
            byte conditionType = ConditionType.kCT_Running;

            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.INITIALIZE_RESULT_CODE_LENGTH + PacketStructure.INITIALIZE_CONDITION_TYPE_LENGTH];

            Buffer.BlockCopy( _packet , 0 , response , 0 , PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH );
            Buffer.BlockCopy( BitConverter.GetBytes( resultCode ) , 0 , response , PacketStructure.KEEPALIVE_RESPONSE_RESULT_CODE_OFFSET , PacketStructure.INITIALIZE_RESULT_CODE_LENGTH );
            response[PacketStructure.KEEPALIVE_RESPONSE_CONDITION_TYPE_OFFSET] = conditionType;

            return response;
        }

        public static Byte[] BuildWorldNo( Byte[] _packet, int ServiceCodeLength )
        {
            // Get World No Offset from Packet
            int worldNoOffset = PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET + ServiceCodeLength;

            // Create the Byte array
            Byte[] worldNo = new byte[PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH];

            Buffer.BlockCopy( _packet , worldNoOffset , worldNo , 0 , PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH );

            return worldNo;
        }
        public static Byte[] BuildServiceCode( Byte[] _packet )
        {
            // Get size of Service Code string
            int serviceCodeLength = BitConverter.ToInt32( _packet , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET );
            
            // Return the entire Service Code block ( Length + Code )
            Byte[] serviceCode = new byte[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH + serviceCodeLength];
            Buffer.BlockCopy( _packet , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET , serviceCode , 0 , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH + serviceCodeLength );

            return serviceCode;
        }
    }
}
