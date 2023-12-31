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
                                    Byte[] bytesBuffer = new byte[PacketStructure.HEADER_LENGTH];
                                    int bytesRead = 0;

                                    // Reads the HEADER_LENGTH bytes of data
                                    bytesRead = stream.Read( bytesBuffer , 0 , bytesBuffer.Length );

                                    Console.WriteLine( "Read " + bytesRead + " bytes: " + BitConverter.ToString( bytesBuffer ) );
                                        
                                    if(bytesBuffer[0] == PacketType.HEADER)
                                    {
                                        // Converte os 4 bytes do Packet Length para ler o resto dos dados
                                        int packetLength = BitConverter.ToInt32( bytesBuffer , PacketStructure.PACKET_LENGTH_OFFSET );

                                        // Cria packet final
                                        Byte[] packet = new byte[bytesBuffer.Length + packetLength];

                                        // Copia o buffer pro packet
                                        Buffer.BlockCopy( bytesBuffer , 0 , packet , 0 , bytesBuffer.Length );

                                        // Le o resto do packet
                                        bytesBuffer = new byte[packetLength];
                                        bytesRead += stream.Read( bytesBuffer , 0 , packetLength );

                                        // Copia pro packet
                                        Buffer.BlockCopy( bytesBuffer , 0 , packet , 0 , bytesBuffer.Length );

                                        Console.WriteLine( "Enviando pacote pra processamento => " + BitConverter.ToString( packet ) );

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
