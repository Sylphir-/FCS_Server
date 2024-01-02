using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public static class PacketProcess
    {

        /*********************************************************************************/
        /*                                               PACKET TYPE 0x01 - Initialize   */
        /*********************************************************************************/
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
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.RESULT_CODE_LENGTH + 1 + serviceCode.Length + PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH];

            // Copy header and echo content into response
            response[0] = PacketType.HEADER;
            int iPacketLength = response.Length - PacketStructure.HEADER_LENGTH;
            byte[] packetLength = BitConverter.GetBytes( iPacketLength );
            if( BitConverter.IsLittleEndian )
                Array.Reverse( packetLength );

            Buffer.BlockCopy( packetLength, 0, response , PacketStructure.PACKET_LENGTH_OFFSET , PacketStructure.PACKET_LENGTH_LENGTH );
            Buffer.BlockCopy( _packet , PacketStructure.ECHO_CONTENT_OFFSET , response , PacketStructure.ECHO_CONTENT_OFFSET , PacketStructure.ECHO_CONTENT_LENGTH );

            // Copy Result Code into response
            Buffer.BlockCopy( resultCode , 0 , response , PacketStructure.INITIALIZE_RESULT_CODE_OFFSET , resultCode.Length );

            // Copy condition type into response
            response[PacketStructure.INITIALIZE_CONDITION_TYPE_OFFSET] = conditionType;

            // Copy Service Code into response
            Buffer.BlockCopy( serviceCode , 0 , response , PacketStructure.INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET , serviceCode.Length );
            Buffer.BlockCopy( worldNo , 0 , response , PacketStructure.INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET + serviceCode.Length , worldNo.Length );

            return response;
        }

        /*********************************************************************************/
        /*                                               PACKET TYPE 0x02 - Keep-Alive   */
        /*********************************************************************************/
        public static Byte[] KeepAlive( Byte[] _packet )
        {
            // Get Result Code
            int iResultCode = ResultCode.kRCSuccess;
            byte[] resultCode = BitConverter.GetBytes( iResultCode );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( resultCode );

            // Get Condition Type
            byte conditionType = ConditionType.kCT_Running;

            // Create Response block
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.RESULT_CODE_LENGTH + PacketStructure.CONDITION_TYPE_LENGTH];

            // Calculate packet length
            int iPacketLength = response.Length - PacketStructure.HEADER_LENGTH;
            byte[] packetLength = BitConverter.GetBytes( iPacketLength );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( packetLength );

            // Set Reserved byte
            response[0] = PacketType.HEADER;

            // Set packet length
            Buffer.BlockCopy( packetLength , 0 , response , PacketStructure.PACKET_LENGTH_OFFSET , PacketStructure.PACKET_LENGTH_LENGTH );

            // Set echo content
            Buffer.BlockCopy( _packet , PacketStructure.ECHO_CONTENT_OFFSET , response , PacketStructure.ECHO_CONTENT_OFFSET , PacketStructure.ECHO_CONTENT_LENGTH );

            // Set Keep Alive response
            Buffer.BlockCopy( resultCode , 0 , response , PacketStructure.KEEPALIVE_RESPONSE_RESULT_CODE_OFFSET , PacketStructure.RESULT_CODE_LENGTH );
            response[PacketStructure.KEEPALIVE_RESPONSE_CONDITION_TYPE_OFFSET] = conditionType;

            return response;
        }

        /*********************************************************************************/
        /*                                               PACKET TYPE 0x11 - Game Login   */
        /*********************************************************************************/
        public static Byte[] GameLogin( Byte[] _packet )
        {
            // extract Structure Type
            byte[] structureType = new byte[PacketStructure.STRUCTURE_TYPE_LENGTH];
            Buffer.BlockCopy( _packet , PacketStructure.PACKET_DATA_OFFSET , structureType , 0 , PacketStructure.STRUCTURE_TYPE_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( structureType );

            // extract User Number
            byte[] _userNumberLength = new byte[PacketStructure.GAMELOGIN_USER_NUMBER_LENGTH];
            Buffer.BlockCopy( _packet , PacketStructure.GAMELOGIN_USER_NUMBER_OFFSET , _userNumberLength , 0 , PacketStructure.GAMELOGIN_USER_NUMBER_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _userNumberLength );
            int userNumberLength = BitConverter.ToInt32( _userNumberLength , 0 );

            byte[] userNumber = new byte[userNumberLength];
            Buffer.BlockCopy( _packet , PacketStructure.GAMELOGIN_USER_NUMBER_OFFSET + PacketStructure.GAMELOGIN_USER_NUMBER_LENGTH , userNumber , 0 , userNumberLength );

            // extract Authentication Key
            int authKeyOffset = PacketStructure.GAMELOGIN_USER_NUMBER_OFFSET + PacketStructure.GAMELOGIN_USER_NUMBER_LENGTH + userNumberLength;
            byte[] _authKeyLength = new byte[PacketStructure.GAMELOGIN_AUTH_KEY_LENGTH];
            Buffer.BlockCopy( _packet , authKeyOffset , _authKeyLength , 0 , PacketStructure.GAMELOGIN_AUTH_KEY_LENGTH );
            if ( BitConverter.IsLittleEndian)
                Array.Reverse( _authKeyLength );
            int authKeyLength = BitConverter.ToInt32(_authKeyLength , 0 );

            byte[] authKey = new byte[authKeyLength];
            Buffer.BlockCopy( _packet , authKeyOffset + PacketStructure.GAMELOGIN_AUTH_KEY_LENGTH , authKey , 0 , authKeyLength );

            // extract Client IP
            int clientIPOffset = authKeyOffset + PacketStructure.GAMELOGIN_AUTH_KEY_LENGTH + authKeyLength;
            byte[] _clientIPLength = new byte[PacketStructure.GAMELOGIN_CLIENT_IP_LENGTH];
            Buffer.BlockCopy( _packet , clientIPOffset , _clientIPLength , 0 , PacketStructure.GAMELOGIN_CLIENT_IP_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _clientIPLength );
            int clientIPLength = BitConverter.ToInt32( _clientIPLength , 0 );

            byte[] clientIP = new byte[clientIPLength];
            Buffer.BlockCopy( _packet , clientIPOffset + PacketStructure.GAMELOGIN_CLIENT_IP_LENGTH , clientIP , 0 , clientIPLength );

            Console.WriteLine( "================ PROCESS =================" );
            Console.WriteLine( "Packet: " + BitConverter.ToString( _packet ) );
            Console.WriteLine( "Structure Type: " + BitConverter.ToInt32( structureType , 0 ) );
            Console.WriteLine( "User Number: " + System.Text.Encoding.ASCII.GetString( userNumber ) );
            Console.WriteLine( "Auth Key: " + System.Text.Encoding.ASCII.GetString( authKey ) );
            Console.WriteLine( "Client IP: " + System.Text.Encoding.ASCII.GetString( clientIP ) );

            // Build response packet


            // Make packet
            byte[] response = new byte[
                PacketStructure.HEADER_LENGTH                       +
                PacketStructure.ECHO_CONTENT_LENGTH                 +
                PacketStructure.RESULT_CODE_LENGTH                  +
                PacketStructure.CONDITION_TYPE_LENGTH               +
                PacketStructure.STRUCTURE_TYPE_LENGTH               +
                PacketStructure.GAMELOGIN_FLAT_RATE_LENGTH          +
                PacketStructure.GAMELOGIN_USER_TYPE_LENGTH          +
                PacketStructure.GAMELOGIN_STRUCTURE_ARR_LEN_LENGTH  ];

            // Set reserved byte
            response[0] = PacketType.HEADER;

            // Set packet length
            int _packetLength = response.Length - 5;
            byte[] packetLength = BitConverter.GetBytes( _packetLength );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( packetLength );
            Buffer.BlockCopy( packetLength , 0 , response , PacketStructure.PACKET_LENGTH_OFFSET , PacketStructure.PACKET_LENGTH_LENGTH );

            // Set echo content
            Buffer.BlockCopy( _packet , PacketStructure.ECHO_CONTENT_OFFSET , response , PacketStructure.ECHO_CONTENT_OFFSET , PacketStructure.ECHO_CONTENT_LENGTH );

            return new byte[5];
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
            byte[] svcCodLgth = new byte[4];
            svcCodLgth[0] = _packet[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET];
            svcCodLgth[1] = _packet[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET+1];
            svcCodLgth[2] = _packet[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET+2];
            svcCodLgth[3] = _packet[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET+3];
            if( BitConverter.IsLittleEndian )
            {
                Array.Reverse( svcCodLgth );
            }
            int serviceCodeLength = BitConverter.ToInt32( svcCodLgth , 0 );
            
            // Return the entire Service Code block ( Length + Code )
            Byte[] serviceCode = new byte[PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH + serviceCodeLength];
            Buffer.BlockCopy( _packet , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET , serviceCode , 0 , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH + serviceCodeLength );

            return serviceCode;
        }
    }
}
