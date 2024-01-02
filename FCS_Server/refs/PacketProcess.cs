using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            Byte[] worldNo = BuildWorldNo( _packet , serviceCode.Length );


            /*****************/
            // Build response byte array
            // Response: Header - Echo Content - Result Code - Condition Type - Service Code - World Number
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.RESULT_CODE_LENGTH + 1 + serviceCode.Length + PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH];

            // Copy header and echo content into response
            response[0] = PacketType.HEADER;
            int iPacketLength = response.Length - PacketStructure.HEADER_LENGTH;
            byte[] packetLength = BitConverter.GetBytes( iPacketLength );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( packetLength );

            Buffer.BlockCopy( packetLength , 0 , response , PacketStructure.PACKET_LENGTH_OFFSET , PacketStructure.PACKET_LENGTH_LENGTH );
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
        public static Byte[] ValidateAuthenticationKeyWithUserInfo( Byte[] _packet )
        {

            /*********************** READ PACKET */
            int cursor = PacketStructure.PACKET_DATA_OFFSET;

            // Extract Callback Attribute
            byte[] _callbackAttributeLength = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , cursor , _callbackAttributeLength , 0 , PacketStructure.INT_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _callbackAttributeLength );
            int callbackAttributeLength = BitConverter.ToInt32( _callbackAttributeLength , 0 );
            cursor += PacketStructure.INT_LENGTH;

            byte[] callbackAttribute = new byte[callbackAttributeLength];
            Buffer.BlockCopy( _packet , cursor , callbackAttribute , 0 , callbackAttributeLength );
            cursor += callbackAttributeLength;

            // Extract Account Number
            byte[] _accountNumber = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , cursor , _accountNumber , 0 , PacketStructure.INT_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _accountNumber );
            int accountNumber = BitConverter.ToInt32( _accountNumber , 0 );
            cursor += PacketStructure.INT_LENGTH;

            // Extract Authentication Key
            byte[] _authKeyLength = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , cursor , _authKeyLength , 0 , PacketStructure.INT_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _authKeyLength );
            int authKeyLength = BitConverter.ToInt32( _authKeyLength , 0 );
            cursor += PacketStructure.INT_LENGTH;

            byte[] _authKey = new byte[authKeyLength];
            Buffer.BlockCopy( _packet , cursor , _authKey , 0 , authKeyLength );
            cursor += authKeyLength;

            // Extract Extension
            byte[] _extLength = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , cursor , _extLength , 0 , PacketStructure.INT_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _extLength );
            int extLength = BitConverter.ToInt32( _extLength , 0 );
            cursor += PacketStructure.INT_LENGTH;

            byte[] _extension = new byte[extLength];
            Buffer.BlockCopy( _packet , cursor , _extension , 0 , extLength );
            cursor += extLength;

            // Extract Client IP
            byte[] _clientIPLength = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , cursor , _clientIPLength , 0 , PacketStructure.INT_LENGTH );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _clientIPLength );
            int clientIPLength = BitConverter.ToInt32( _clientIPLength , 0 );
            cursor += PacketStructure.INT_LENGTH;

            byte[] _clientIP = new byte[clientIPLength];
            Buffer.BlockCopy( _packet , cursor , _clientIP , 0 , clientIPLength );
            cursor += clientIPLength;

            Console.WriteLine( "========= USER INFO AUTHENTICATION ==========" );
            Console.WriteLine( "Callback Attribute: " + System.Text.Encoding.ASCII.GetString( callbackAttribute ) );
            Console.WriteLine( "Account Number: " + accountNumber );
            Console.WriteLine( "Authentication Key: " + System.Text.Encoding.ASCII.GetString( _authKey ) );
            Console.WriteLine( "Extension: " + BitConverter.ToString( _extension ) );
            Console.WriteLine( "Client IP: " + System.Text.Encoding.ASCII.GetString( _clientIP ) );

            /*********************** BUILD RESPONSE */

            // Response
            // .-------------.----------------.--------------------.---------------.----------.---------.------------.------------.-----------.
            // | Result Code | Condition Type | Callback Attribute | Provider Code |  User No | User ID | Account No | Account ID | Extension |
            // |-------------+----------------+--------------------+---------------+----------+---------+------------+------------+-----------|
            // | 10 11 12 13 |       14       | 15                 | ~             | ~        | ~       | ~          | ~          | ~         |
            // |   Integer   |      Byte      | Int+Str            | Int+Str       | Int      | Str     | Int        | Str        | JSON      |
            // '-------------'----------------'--------------------'---------------'----------'---------'------------'------------'-----------'

            // Build Echo Content
            byte[] echoContent = new byte[PacketStructure.ECHO_CONTENT_LENGTH];
            Buffer.BlockCopy( _packet , PacketStructure.ECHO_CONTENT_OFFSET , echoContent , 0 , PacketStructure.ECHO_CONTENT_LENGTH );

            // Build Result Code
            byte[] resultCode = PacketProcess.IntToByte( ResultCode.kRCSuccess );

            // Build Condition Type
            byte conditionType = ConditionType.kCT_Running;

            // Callback Attribute
            byte[] rCallbackAttribute = new byte[callbackAttributeLength + PacketStructure.INT_LENGTH];
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _callbackAttributeLength );
            Buffer.BlockCopy( _callbackAttributeLength , 0 , rCallbackAttribute , 0 , PacketStructure.INT_LENGTH );
            Buffer.BlockCopy( callbackAttribute , 0 , rCallbackAttribute , PacketStructure.INT_LENGTH , callbackAttribute.Length );

            // Provider Code
            byte[] _providerCode = System.Text.Encoding.ASCII.GetBytes( ProviderCode.SCT001 );
            byte[] providerCodeLength = PacketProcess.IntToByte( _providerCode.Length );
            byte[] providerCode = new byte[PacketStructure.INT_LENGTH + _providerCode.Length];
            Buffer.BlockCopy( providerCodeLength , 0 , providerCode , 0 , PacketStructure.INT_LENGTH );
            Buffer.BlockCopy( _providerCode , 0 , providerCode , PacketStructure.INT_LENGTH , _providerCode.Length );

            // User Number
            int _userNumber = 1;
            byte[] userNumber = PacketProcess.IntToByte( _userNumber );

            // User ID
            String __userID = "dezner";
            byte[] _userID = System.Text.Encoding.ASCII.GetBytes( __userID ); // 6 bytes
            byte[] userIDLength = PacketProcess.IntToByte( _userID.Length ); // 6 - 00 00 00 06
            byte[] userID = new byte[PacketStructure.INT_LENGTH + _userID.Length]; //  4 + 6 = 10
            Buffer.BlockCopy( userIDLength , 0 , userID , 0 , PacketStructure.INT_LENGTH ); // 0-4 -> [0-4]
            Buffer.BlockCopy( _userID , 0 , userID , PacketStructure.INT_LENGTH , _userID.Length ); // 0-6 -> [4-10]

            // Account Number
            int __accountNumber = 1;
            byte[] _rAccountNumber = PacketProcess.IntToByte( __accountNumber );

            // Account ID
            String __accountID = "dezner";
            byte[] _accountID = System.Text.Encoding.ASCII.GetBytes( __accountID );
            byte[] accountIDLength = PacketProcess.IntToByte( _accountID.Length );
            byte[] accountID = new byte[PacketStructure.INT_LENGTH + _accountID.Length];
            Buffer.BlockCopy( accountIDLength , 0 , accountID , 0 , PacketStructure.INT_LENGTH );
            Buffer.BlockCopy( _accountID , 0 , accountID , PacketStructure.INT_LENGTH , _accountID.Length );

            // Extension
            byte[] extension = new byte[] { 0x00 , 0x00 , 0x00 , 0x00 , 0x00 };

            // Total Packet Length
            int packetLength = PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + resultCode.Length + 1 + rCallbackAttribute.Length + providerCode.Length + userNumber.Length +
                userID.Length + _rAccountNumber.Length + accountID.Length + extension.Length;
            
            // Make Response Packet
            byte[] response = new byte[packetLength];

            // Build Header
            response[0] = PacketType.HEADER;

            byte[] lengthBlock = PacketProcess.IntToByte( packetLength - 5 );
            Buffer.BlockCopy( lengthBlock , 0 , response , PacketStructure.HEADER_RESERVED_LENGTH , PacketStructure.INT_LENGTH );

            // Copy Echo Content
            Buffer.BlockCopy( echoContent , 0 , response , PacketStructure.ECHO_CONTENT_OFFSET , echoContent.Length );

            // Copy data
            cursor = PacketStructure.PACKET_DATA_OFFSET;

            Buffer.BlockCopy( resultCode , 0 , response , cursor , resultCode.Length );
            cursor += resultCode.Length;

            response[cursor] = conditionType;
            cursor += 1;

            Buffer.BlockCopy( rCallbackAttribute , 0 , response , cursor , rCallbackAttribute.Length );
            cursor += rCallbackAttribute.Length;

            Buffer.BlockCopy( providerCode , 0 , response , cursor , providerCode.Length );
            cursor += providerCode.Length;

            Buffer.BlockCopy( userNumber , 0 , response , cursor , userNumber.Length );
            cursor += userNumber.Length;

            Buffer.BlockCopy( userID , 0 , response , cursor , userID.Length );
            cursor += userID.Length;

            Buffer.BlockCopy( _rAccountNumber , 0 , response , cursor , _rAccountNumber.Length );
            cursor += _rAccountNumber.Length;

            Buffer.BlockCopy( accountID , 0 , response , cursor , accountID.Length );
            cursor += accountID.Length;
            
            Buffer.BlockCopy( extension , 0 , response , cursor , extension.Length );
            cursor += extension.Length;

            Console.WriteLine( "\n=============== RESPONSE ============" );
            Console.WriteLine( BitConverter.ToString( response , 0 ) );

            return response;
        }

        public static byte[] IntToByte( int number )
        {
            byte[] _n = BitConverter.GetBytes( number );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _n );
            return _n;
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
            Byte[] serviceCode = new byte[PacketStructure.SERVICE_CODE_LENGTH + serviceCodeLength];
            Buffer.BlockCopy( _packet , PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET , serviceCode , 0 , PacketStructure.SERVICE_CODE_LENGTH + serviceCodeLength );

            return serviceCode;
        }
    }
}
