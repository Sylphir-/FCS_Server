﻿using System;
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
            Byte[] response = new byte[PacketStructure.HEADER_LENGTH + PacketStructure.ECHO_CONTENT_LENGTH + PacketStructure.INITIALIZE_RESULT_CODE_LENGTH + PacketStructure.INITIALIZE_CONDITION_TYPE_LENGTH];

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
            Buffer.BlockCopy( resultCode , 0 , response , PacketStructure.KEEPALIVE_RESPONSE_RESULT_CODE_OFFSET , PacketStructure.INITIALIZE_RESULT_CODE_LENGTH );
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
