using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            // Get Result Code
            Byte[] resultCode = IntToByte( ResultCode.kRCSuccess );

            // Get Condition Type
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
            byte[] packetLength = IntToByte( iPacketLength );

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
            byte[] resultCode = IntToByte( iResultCode );

            // Get Condition Type
            byte conditionType = ConditionType.kCT_Running;

            // Create Response block
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.RESULT_CODE_LENGTH + PacketStructure.CONDITION_TYPE_LENGTH];

            // Calculate packet length
            int iPacketLength = response.Length - PacketStructure.HEADER_LENGTH;
            byte[] packetLength = IntToByte( iPacketLength );

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
            byte[] r_callbackAttribute = GetVariableLengthBlock( _packet , cursor );
            cursor += r_callbackAttribute.Length;

            // Extract Account Number
            int r_accountNumber = ByteToInt( _packet , cursor );
            cursor += PacketStructure.INT_LENGTH;

            // Extract Authentication Key
            byte[] r_authKey = GetVariableLengthBlock( _packet , cursor );
            cursor += r_authKey.Length;

            // Extract Extension
            byte[] r_extension = GetVariableLengthBlock( _packet , cursor );
            cursor += r_extension.Length;

            // Extract Client IP
            byte[] r_clientIP = GetVariableLengthBlock( _packet , cursor );
            cursor += r_clientIP.Length;

            /********************** BUILD RESPONSE */

            // Build Echo Content
            byte[] echoContent = new byte[PacketStructure.ECHO_CONTENT_LENGTH];
            Buffer.BlockCopy( _packet , PacketStructure.ECHO_CONTENT_OFFSET , echoContent , 0 , PacketStructure.ECHO_CONTENT_LENGTH );

            // Build Result Code
            byte[] resultCode = PacketProcess.IntToByte( ResultCode.kRCSuccess );

            // Build Condition Type
            byte conditionType = ConditionType.kCT_Running;

            // Callback Attribute
            // // Callback attribute is the same as received package, r_callbackAttribute

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
            int packetLength = PacketStructure.HEADER_LENGTH + echoContent.Length + resultCode.Length + 1 + r_callbackAttribute.Length + providerCode.Length + userNumber.Length +
                userID.Length + _rAccountNumber.Length + accountID.Length + extension.Length;

            // Make Response Packet
            byte[] response = new byte[packetLength];

            // Build Header
            response[0] = PacketType.HEADER;

            byte[] lengthBlock = PacketProcess.IntToByte( packetLength - PacketStructure.HEADER_LENGTH );
            Buffer.BlockCopy( lengthBlock , 0 , response , PacketStructure.HEADER_RESERVED_LENGTH , PacketStructure.INT_LENGTH );

            // Copy Echo Content
            Buffer.BlockCopy( echoContent , 0 , response , PacketStructure.ECHO_CONTENT_OFFSET , echoContent.Length );

            // Copy data
            cursor = PacketStructure.PACKET_DATA_OFFSET;

            Buffer.BlockCopy( resultCode , 0 , response , cursor , resultCode.Length );
            cursor += resultCode.Length;

            response[cursor] = conditionType;
            cursor += 1;

            Buffer.BlockCopy( r_callbackAttribute , 0 , response , cursor , r_callbackAttribute.Length );
            cursor += r_callbackAttribute.Length;

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

            Console.WriteLine( BitConverter.ToString( response ) );
            return response;
        }

        public static byte[] WShopCheckBalance( byte[] _packet )
        {
            //////////// READ PACKET
            int cursor = PacketStructure.PACKET_DATA_OFFSET;
            // callback attribute   (4+var)
            byte[] r_callbackAttribute = GetVariableLengthBlock( _packet , cursor );
            cursor += r_callbackAttribute.Length;
            // return structure     (1)
            byte r_returnStructure = _packet[cursor];
            cursor += 1;
            // provider code        (4+var)
            byte[] r_providerCode = GetVariableLengthBlock( _packet , cursor );
            cursor += r_providerCode.Length;
            // user number          (4)
            int r_userNumber = ByteToInt( _packet , cursor );
            cursor += PacketStructure.INT_LENGTH;
            // user id              (4+var)
            byte[] r_userID = GetVariableLengthBlock( _packet , cursor );
            cursor += r_userID.Length;
            // account number       (4)
            int r_accountNumber = ByteToInt( _packet , cursor );
            cursor += PacketStructure.INT_LENGTH;
            // world key            (4+var)
            byte[] r_worldKey = GetVariableLengthBlock( _packet , cursor );
            cursor += r_worldKey.Length;
            // character number     (8)
            long r_characterNumber = ByteToLong( _packet , cursor );
            cursor = PacketStructure.LONG_LENGTH;
            // character id         (4+var)
            byte[] r_characterID = GetVariableLengthBlock( _packet , cursor );
            cursor += r_characterID.Length;
            // client ip            (4+var)
            byte[] r_clientIP = GetVariableLengthBlock(_packet , cursor );

            //////////// RESPONSE PACKET
            // Result Code (int)
            byte[] resultCode = IntToByte( ResultCode.kRCSuccess );
            // Condition Type (int)
            byte[] conditionType = IntToByte( ConditionType.kCT_Running );
            // Callback Attribute (int+var)
            byte[] callbackAttribute = r_callbackAttribute;
            // Return structure (int)
            byte returnStructure = r_returnStructure;
            // User Number (int)
            byte[] userNumber = IntToByte( r_userNumber );
            // Account Number (int)
            byte[] accountNumber = IntToByte( r_accountNumber );
            // Character Number
            byte[] characterNumber = LongToByte( r_characterNumber );

            // Coin Balance
            /** DB CONN **/
            byte[] wcoin = IntToByte( 50000 );
            byte[] redgen = IntToByte( 40000 );
            byte[] imp_redgen = IntToByte( 30000 );
            byte[] type_wcoin = IntToByte( CashType.WCoin );
            byte[] type_redgen = IntToByte( CashType.Redgen );
            byte[] type_imp_redgen = IntToByte( CashType.ImputationRedgen );

            // Jewel Balance
            byte[] jewelBalanceItemCount = IntToByte( 3 );
            byte[] jewelBalanceItemArray = new byte[wcoin.Length + type_wcoin.Length + redgen.Length + type_redgen.Length + imp_redgen.Length + type_imp_redgen.Length];

            cursor = 0;
            Buffer.BlockCopy( wcoin , 0 , jewelBalanceItemArray , cursor , wcoin.Length );
            cursor += wcoin.Length;
            Buffer.BlockCopy( type_wcoin , 0 , jewelBalanceItemArray , cursor , type_wcoin.Length );
            cursor += type_wcoin.Length;
            Buffer.BlockCopy( redgen , 0 , jewelBalanceItemArray , cursor , redgen.Length );
            cursor += redgen.Length;
            Buffer.BlockCopy( type_redgen , 0 , jewelBalanceItemArray , cursor , type_redgen.Length );
            cursor += type_redgen.Length;
            Buffer.BlockCopy( imp_redgen, 0, jewelBalanceItemArray, cursor , imp_redgen.Length );
            cursor += imp_redgen.Length;
            Buffer.BlockCopy( type_imp_redgen , 0 , jewelBalanceItemArray , cursor , type_imp_redgen.Length );

            byte[] response = new byte[PacketStructure.HEADER_LENGTH +
                                        PacketStructure.ECHO_CONTENT_LENGTH +
                                        resultCode.Length +
                                        conditionType.Length +
                                        callbackAttribute.Length +
                                        PacketStructure.BYTE_LENGTH +
                                        userNumber.Length +
                                        accountNumber.Length +
                                        characterNumber.Length +
                                        jewelBalanceItemCount.Length +
                                        jewelBalanceItemArray.Length];

            // Header
            response[0] = PacketType.HEADER;
            // Packet Length
            byte[] length = IntToByte( response.Length - PacketStructure.HEADER_LENGTH );
            Buffer.BlockCopy( length , 0 , response , PacketStructure.PACKET_LENGTH_LENGTH , PacketStructure.INT_LENGTH );
            // Packet Type
            response[PacketStructure.PACKET_TYPE_OFFSET] = _packet[PacketStructure.PACKET_TYPE_OFFSET];
            // Transaction ID
            Buffer.BlockCopy( _packet , PacketStructure.TRANSACTION_ID_OFFSET , response , PacketStructure.TRANSACTION_ID_OFFSET , PacketStructure.TRANSACTION_ID_LENGTH );
            // Result Code
            cursor = PacketStructure.PACKET_DATA_OFFSET;
            Buffer.BlockCopy( resultCode , 0 , response , cursor , resultCode.Length );
            cursor += resultCode.Length;
            // Condition Type
            Buffer.BlockCopy( conditionType , 0 , response , cursor , conditionType.Length );
            cursor += conditionType.Length;
            // Callback Attribute
            Buffer.BlockCopy( callbackAttribute , 0 , response , cursor , callbackAttribute.Length );
            cursor += callbackAttribute.Length;
            // Return Structure
            response[cursor] = returnStructure;
            cursor += PacketStructure.BYTE_LENGTH;
            // User Number
            Buffer.BlockCopy( userNumber, 0, response, cursor, userNumber.Length );
            cursor += userNumber.Length;
            // Account Number
            Buffer.BlockCopy( accountNumber, 0, response, cursor, accountNumber.Length );
            cursor += accountNumber.Length;
            // Character Number
            Buffer.BlockCopy( characterNumber , 0 , response , cursor , characterNumber.Length );
            cursor += characterNumber.Length;
            // Jewel Balance Item Count
            Buffer.BlockCopy( jewelBalanceItemCount , 0 , response , cursor , jewelBalanceItemCount.Length );
            cursor += jewelBalanceItemCount.Length;
            //Jewel Balance Item Array
            Buffer.BlockCopy( jewelBalanceItemArray , 0 , response , cursor , jewelBalanceItemArray.Length );

            return response;
        }

        public static byte[] GetVariableLengthBlock( byte[] _packet, int offset )
        {
            int length = ByteToInt(  _packet , offset );

            byte[] r = new byte[PacketStructure.INT_LENGTH + length];
            Buffer.BlockCopy( _packet , offset , r , 0 , r.Length );

            return r;
        }

        public static byte[] BuildVariableLengthBlock( byte[] _data )
        {
            byte[] length = new byte[PacketStructure.INT_LENGTH];

            length = IntToByte( _data.Length );

            byte[] r = new byte[PacketStructure.INT_LENGTH + _data.Length];
            Buffer.BlockCopy( length , 0 , r , 0 , PacketStructure.INT_LENGTH );
            Buffer.BlockCopy( _data , 0 , r , PacketStructure.INT_LENGTH , _data.Length );

            return r;
        }

        public static int ByteToInt( byte[] _packet , int offset )
        {
            byte[] b = new byte[PacketStructure.INT_LENGTH];
            Buffer.BlockCopy(_packet, offset, b, 0, PacketStructure.INT_LENGTH );

            if (BitConverter.IsLittleEndian)
                Array.Reverse( b );

            return BitConverter.ToInt32( b , 0 );
        }
        public static byte[] IntToByte( int number )
        {
            byte[] _n = BitConverter.GetBytes( number );
            if (BitConverter.IsLittleEndian)
                Array.Reverse( _n );
            return _n;
        }
        public static byte[] LongToByte ( long number )
        {
            byte[] _n = BitConverter.GetBytes(number );
            if( BitConverter.IsLittleEndian ) Array.Reverse( _n );
            return _n;
        }

        public static long ByteToLong( byte[] _packet, int offset )
        {
            byte[] b = new byte[PacketStructure.LONG_LENGTH];
            Buffer.BlockCopy( _packet , offset , b , 0 , PacketStructure.LONG_LENGTH );

            if (BitConverter.IsLittleEndian)
                Array.Reverse( b );

            return BitConverter.ToInt64( b , 0 );
        }

        public static byte[] BuildWorldNo( byte[] _packet, int ServiceCodeLength )
        {
            // Get World No Offset from Packet
            int worldNoOffset = PacketStructure.INITIALIZE_SERVICE_CODE_LENGTH_OFFSET + ServiceCodeLength;

            // Create the Byte array
            Byte[] worldNo = new byte[PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH];

            Buffer.BlockCopy( _packet , worldNoOffset , worldNo , 0 , PacketStructure.INITIALIZE_WORLD_NUMBER_LENGTH );

            return worldNo;
        }
        public static byte[] BuildServiceCode( byte[] _packet )
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
