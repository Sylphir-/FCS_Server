using FCS_Server.refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server
{
    public class Packet
    {
        private byte[] _packet;
        private TcpClient _client;
        public Packet( byte[] __packet, TcpClient __client )
        {
            _packet = __packet;
            _client = __client;

            Console.WriteLine( "Recebi " + BitConverter.ToString( _packet ) );

            processPacket();
        }

        private byte[] GetHeader()
        {
            Byte[] b = new byte[PacketStructure.PACKET_HEADER_STRUCTURE_LENGTH];
            b[0] = PacketType.HEADER;

            return b;
        }
        private byte[] GetEchoContent()
        {
            Byte[] b = new Byte[PacketStructure.PACKET_ECHO_CONTENT_LENGTH];

            for(int i=0; i<b.Length; i++)
            {
                b[i] = _packet[PacketStructure.PACKET_ECHO_CONTENT_OFFSET + i];
            }

            return b;
        }
        private byte GetPacketType()
        {
            return _packet[0];
        }

        private byte[] GetTransactionId()
        {
            Byte[] b = new Byte[PacketStructure.PACKET_ECHO_CONTENT_TRANSACTION_ID_LENGTH];

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = _packet[PacketStructure.PACKET_ECHO_CONTENT_TRANSACTION_ID_OFFSET + i];
            }

            return b;
        }

        private void processPacket()
        {
            Byte[] partialResponse;
            switch (this.GetPacketType())
            {
                case PacketType.Initialize:
                    partialResponse = PacketProcess.Initialize( _packet );
                    break;
                default:
                    partialResponse = new Byte[1];
                    partialResponse[0] = 0x00;
                    Console.WriteLine( "Packet Type is Invalid. (" + this.GetPacketType().ToString() + ")" );
                    break;
            }

            Console.WriteLine( "Partial Response: " + BitConverter.ToString( partialResponse ) );
            Console.WriteLine( "Response: " + BitConverter.ToString( Packet.AppendHeader( GetEchoContent(), partialResponse ) ) );

            //SendResponse( Packet.AppendHeader( partialResponse ) );
        }

        private static Byte[] AppendHeader( Byte[] echoContent, Byte[] toAppend )
        {
            Byte[] complete = new byte[PacketStructure.PACKET_HEADER_STRUCTURE_LENGTH + PacketStructure.PACKET_ECHO_CONTENT_LENGTH + toAppend.Length];
            complete[0] = PacketType.HEADER;

            Byte[] pktLength = BitConverter.GetBytes( toAppend.Length + PacketStructure.PACKET_ECHO_CONTENT_LENGTH );

            if (BitConverter.IsLittleEndian)
                Array.Reverse( pktLength );

            complete[1] = pktLength[0];
            complete[2] = pktLength[1];
            complete[3] = pktLength[2];
            complete[4] = pktLength[3];

            complete[5] = echoContent[0];
            complete[6] = echoContent[1];
            complete[7] = echoContent[2];
            complete[8] = echoContent[3];
            complete[9] = echoContent[4];

            int j = 10;
            for(int i=0; i < toAppend.Length; i++)
            {
                complete[j] = toAppend[i];
                j++;
            }

            return complete;
        }

        private void SendResponse( Byte[] response )
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                stream.Write(response,0,response.Length );
            }catch(Exception e)
            {
                Console.WriteLine( e.ToString() ); 
            }
        }
    }
}
