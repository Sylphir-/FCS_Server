using FCS_Server.refs;
using FCS_Server.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FCS_Server
{
    public class Packet
    {
        private readonly byte[] packet;
        private TcpClient client;
        public Packet( byte[] _packet, TcpClient _client )
        {
            packet = _packet;
            client = _client;

            Console.WriteLine( String.Format( "[{0:HH:mm:ss}][PACKET][RECEIVED] " , DateTime.Now ) + BitConverter.ToString( _packet ) );
            ProcessPacket();
        }

        private byte[] GetHeader()
        {
            Byte[] b = new byte[PacketStructure.HEADER_LENGTH];
            Buffer.BlockCopy( packet , 0 , b , 0 , PacketStructure.HEADER_LENGTH);

            return b;
        }
        private byte[] GetEchoContent()
        {
            Byte[] b = new Byte[PacketStructure.ECHO_CONTENT_LENGTH];
            Buffer.BlockCopy( packet , PacketStructure.ECHO_CONTENT_OFFSET , b , 0 , PacketStructure.ECHO_CONTENT_LENGTH );

            return b;
        }
        private byte GetPacketType()
        {
            return packet[PacketStructure.PACKET_TYPE_OFFSET];
        }

        private byte[] GetTransactionId()
        {
            Byte[] b = new Byte[PacketStructure.TRANSACTION_ID_LENGTH];
            Buffer.BlockCopy(packet, PacketStructure.TRANSACTION_ID_OFFSET, b, 0, PacketStructure.TRANSACTION_ID_LENGTH );

            return b;
        }

        private void ProcessPacket()
        {
            Byte[] _response;
            try
            {
                switch (this.GetPacketType())
                {
                    case PacketType.Initialize:
                        _response = PacketProcess.Initialize( packet );
                        break;
                    case PacketType.KeepAlive:
                        _response = PacketProcess.KeepAlive( packet );
                        break;
                    default:
                        _response = new Byte[1];
                        _response[0] = 0x00;
                        throw new PacketException( packet , PacketException.Codes.INVALID_PACKET_TYPE );
                }

                SendResponse( _response );
            }catch(PacketException e)
            {
                Console.Error.WriteLine( e.ToString() );
            }
        }
        private void SendResponse( Byte[] response )
        {
            Console.WriteLine( String.Format( "[{0:HH:mm:ss}][PACKET][RESPONSE] " , DateTime.Now ) + BitConverter.ToString( response ) );
            try
            {
                NetworkStream stream = client.GetStream();

                if (stream.CanWrite)
                {
                    stream.Write( response , 0 , response.Length );
                } else
                {
                    throw new PacketException( response , PacketException.Codes.CONNECTION_READ_ONLY );
                }
            }catch(Exception e)
            {
                Console.WriteLine( e.ToString() ); 
            }
        }
    }
}
