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

        public static byte[] WShopCheckBalance(  byte[] _packet )
        {
            /*
            * #===============================================================================================================
            reserved                    = self.data[self.__pos]
            self.__pos                  += sizeVar.reserved
            #===============================================================================================================
            packet_length               = self.data[self.__pos:self.__pos + sizeVar.packet_length]
            self.__pos                  += sizeVar.packet_length
            #===============================================================================================================
            packet_type                 = self.data[self.__pos:self.__pos + sizeVar.packet_type]
            self.__pos                  += sizeVar.packet_type
            #===============================================================================================================
            transaction_id              = self.data[self.__pos:self.__pos + sizeVar.transaction_id]
            self.__pos                  += sizeVar.transaction_id
            #===============================================================================================================
            callback_attribute          = self.data[self.__pos:self.__pos + sizeVar.callback_attribute]
            self.__pos                  += sizeVar.callback_attribute
            callback_attribute_string   = self.data[self.__pos:self.__pos + callback_attribute[-1]]
            self.__pos                  += callback_attribute[-1]
            #===============================================================================================================
            return_structure            = self.data[self.__pos:self.__pos + sizeVar.return_structure]
            self.__pos                  += sizeVar.return_structure
            #===============================================================================================================
            provider_code               = self.data[self.__pos:self.__pos + sizeVar.provider_code]
            self.__pos                  += sizeVar.provider_code
            provider_code_string        = self.data[self.__pos:self.__pos + provider_code[-1]]
            self.__pos                  += provider_code[-1]
            #===============================================================================================================
            user_no                     = self.data[self.__pos:self.__pos + sizeVar.user_no]
            self.__pos                  += sizeVar.user_no
            #===============================================================================================================
            user_id                     = self.data[self.__pos:self.__pos + sizeVar.user_id]
            self.__pos                  += sizeVar.user_id
            user_id_string              = self.data[self.__pos:self.__pos + user_id[-1]]
            self.__pos                  += user_id[-1]
            #===============================================================================================================
            account_no                  = self.data[self.__pos:self.__pos + sizeVar.account_no]
            self.__pos                  += sizeVar.account_no
            #===============================================================================================================
            account_id                  = self.data[self.__pos:self.__pos + sizeVar.account_id]
            self.__pos                  += sizeVar.account_id
            account_id_string           = self.data[self.__pos:self.__pos + account_id[-1]]
            self.__pos                  += account_id[-1]
            #===============================================================================================================
            world_key                   = self.data[self.__pos:self.__pos + sizeVar.world_key]
            self.__pos                  += sizeVar.world_key
            world_key_string            = self.data[self.__pos:self.__pos + world_key[-1]]
            self.__pos                  += world_key[-1]
            #===============================================================================================================
            character_no                = self.data[self.__pos:self.__pos + sizeVar.character_no]
            self.__pos                  += sizeVar.character_no
            #===============================================================================================================
            character_id                = self.data[self.__pos:self.__pos + sizeVar.character_id]
            self.__pos                  += sizeVar.character_id
            character_id_string         = self.data[self.__pos:self.__pos + character_id[-1]]
            self.__pos                  += character_id[-1]
            #===============================================================================================================
            client_ip                   = self.data[self.__pos:self.__pos + sizeVar.client_ip]
            self.__pos                  += sizeVar.client_ip
            client_ip_string            = self.data[self.__pos:self.__pos + client_ip[-1]]
            self.__pos                  += client_ip[-1]
            #===============================================================================================================
            send = (reserved, packet_length, packet_type, transaction_id, callback_attribute, callback_attribute_string, return_structure, provider_code, provider_code_string, user_no, user_id, user_id_string, account_no, account_id, account_id_string, world_key, world_key_string, character_no, character_id, character_id_string, client_ip, client_ip_string)
            return send_wshop_billing.RequestWShopCheckBalance(send)
            */

            return new byte[5];
        }

        public static byte[] GetVariableLengthBlock( byte[] _packet, int offset )
        {
            int length = ByteToInt(  _packet , offset );

            byte[] b = new byte[length];
            Buffer.BlockCopy( _packet , offset + PacketStructure.INT_LENGTH , b , 0 , length );

            byte[] r = new byte[length + PacketStructure.INT_LENGTH];
            Buffer.BlockCopy( _packet , offset , r , 0 , PacketStructure.INT_LENGTH );
            Buffer.BlockCopy( b , 0 , r , PacketStructure.INT_LENGTH , b.Length );

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
