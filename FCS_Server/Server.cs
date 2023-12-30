using FCS_Server.refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FCS_Server
{
    class Server
    {
        /**
         * Entry Point
         */
        public static void Main( string[] args )
        {
            Server.start();
        }

        /**
         * Server startup
         */
        public static void start()
        {
            try
            {
                // Starts a new Listener for this thread
                TcpListener listener = new TcpListener( IPAddress.Any , Constants.port );
                listener.Start();

                try
                {
                    Console.WriteLine( Constants.SERVER_CONN_WAITING );
                    while (true)
                    {
                        if (listener.Pending())
                        {
                            // If there is a pending connection, accept it
                            TcpClient client = listener.AcceptTcpClient();
                            NetworkStream stream = client.GetStream();

                            if (stream.CanRead)
                            {
                                if( client.Connected)
                                {
                                    Console.WriteLine( "Connected to " + client.Client.RemoteEndPoint );
                                }

                                // While there is data to be read
                                while (stream.DataAvailable)
                                {
                                    // Creates a Buffer for data packets
                                    Byte[] bytesBuffer = new byte[PacketStructure.PACKET_HEADER_STRUCTURE_LENGTH];
                                    int bytesRead = 0;

                                    // Reads 1024 bytes of data
                                    bytesRead = stream.Read( bytesBuffer , 0 , bytesBuffer.Length );
                                        
                                    if(bytesBuffer[0] == PacketType.HEADER)
                                    {
                                        // Cria 4 bytes para converter em int
                                        Byte[] pktLength = new byte[PacketStructure.PACKET_HEADER_PACKET_LENGTH];

                                        // Popula os bytes
                                        pktLength[0] = bytesBuffer[1];
                                        pktLength[1] = bytesBuffer[2];
                                        pktLength[2] = bytesBuffer[3];
                                        pktLength[3] = bytesBuffer[4];

                                        if (BitConverter.IsLittleEndian)
                                            Array.Reverse( pktLength );

                                        // Converte os 4 bytes do Packet Length para ler o resto dos dados
                                        Int16 packetLength = BitConverter.ToInt16( pktLength , 0 );

                                        // Le o resto do packet
                                        Byte[] packet = new byte[packetLength];
                                        bytesRead += stream.Read( packet , 0 , packetLength );

                                        // Processa o packet
                                        new Thread( () =>
                                        {
                                            Packet p = new Packet( packet, client );

                                        }).Start();
                                        
                                    }
                                }
                            } else
                            {
                                Console.WriteLine( Constants.SERVER_STREAM_UNREADABLE );
                            }
                        } else
                        {
                            //Console.WriteLine( Constants.SERVER_CONN_WAITING );
                        }
                    }
                } catch (Exception e)
                {
                    // Connection failed
                    Console.WriteLine( e.ToString() );
                }

            } catch (Exception e)
            {
                // Starting a listener failed
                Console.WriteLine( e.ToString() );
            }
        }
    }
}
