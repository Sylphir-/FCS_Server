﻿using FCS_Server.refs;
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
            Server.Start();

            Console.WriteLine( " \n\n" );
            Console.WriteLine( "Server is halted. Press any key to close the console." );
            Console.Read();
        }

        /**
         * Server startup
         */
        public static void Start()
        {
            Console.WriteLine( "Testing Game Login packets..." );
            Byte[] testPacket = new byte[] { 
                /* HEADER */
                PacketType.HEADER , 0x00 , 0x00 , 0x00 , 0x4f ,
                /* ECHO CONTENT */
                PacketType.GameLogin , 0x00 , 0x00 , 0x00 , 0x0C ,
                /* DATA */
                // Return Structure Type
                0x00 , 0x00 , 0x00 , 0x0A ,
                // User Number
                0x00 , 0x00 , 0x00 , 0x0A ,
                0x32 , 0x32 , 0x34 , 0x35 , 0x35 , 0x39 , 0x31 , 0x30 , 0x34 , 0x31,
                // Authentication Key
                0x00 , 0x00 , 0x00 , 0x24 ,
                //6A7E76A7-815F-49BF-94F8-FDF560DC0945
                0x36 , 0x41 , 0x37 , 0x45 , 0x37 , 0x36 , 0x41 , 0x37 , 0x2d , 0x38 , 0x31 , 0x35 , 0x46 , 0x2D , 0x34 ,  0x39 , 0x42 , 0x46 ,
                0x2d , 0x39 , 0x34 , 0x46 , 0x38 , 0x2d , 0x46 , 0x44 , 0x46 , 0x35 , 0x36 , 0x30 , 0x44 , 0x43 , 0x30 , 0x39 , 0x34 , 0x35 ,
                // Client IP
                0x00 , 0x00 , 0x00 , 0x08 ,
                0x31 , 0x30 , 0x2E , 0x30 , 0x2E , 0x30 , 0x2E , 0x31
            };

            PacketProcess.GameLogin( testPacket ); 
            /*
            try
            {
                // Starts a new Listener for this thread
                TcpListener listener = new TcpListener( IPAddress.Any , Constants.port );
                TcpClient client = new TcpClient();
                NetworkStream stream;
                listener.Start();

                do
                {
                    Console.WriteLine( String.Format( "[{0:HH:mm:ss}][SERVER] Waiting for connection..." , DateTime.Now ) );
                    client = listener.AcceptTcpClient();
                }
                while (!client.Connected);

                Console.WriteLine( String.Format( "[{0:HH:mm:ss}][SERVER] Connected to " + client.Client.RemoteEndPoint + ".", DateTime.Now ) );
                stream = client.GetStream();

                try
                {
                    while (true)
                    {
                        if (stream.CanRead)
                        {
                            do
                            {
                                // Creates a Buffer for data packets
                                Byte[] bytesBuffer = new byte[PacketStructure.HEADER_LENGTH];
                                int bytesRead = 0;

                                // Reads the HEADER_LENGTH bytes of data
                                bytesRead = stream.Read( bytesBuffer , 0 , bytesBuffer.Length );

                                if (bytesBuffer[0] == PacketType.HEADER)
                                {
                                    // Converte os 4 bytes do Packet Length para ler o resto dos dados
                                    byte[] pktLength = new byte[4]{
                                        bytesBuffer[PacketStructure.PACKET_LENGTH_OFFSET],
                                        bytesBuffer[PacketStructure.PACKET_LENGTH_OFFSET+1],
                                        bytesBuffer[PacketStructure.PACKET_LENGTH_OFFSET+2],
                                        bytesBuffer[PacketStructure.PACKET_LENGTH_OFFSET+3],
                                    };
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse( pktLength );
                                    }
                                    Int16 packetLength = BitConverter.ToInt16( pktLength , 0 );

                                    // Cria packet final
                                    Byte[] packet = new byte[PacketStructure.HEADER_LENGTH + packetLength];

                                    // Copia o buffer pro packet
                                    Buffer.BlockCopy( bytesBuffer , 0 , packet , 0 , bytesBuffer.Length );

                                    // Le o resto do packet
                                    byte[] packetBuffer = new byte[packetLength];
                                    bytesRead += stream.Read( packetBuffer , 0 , packetLength );

                                    // Copia pro packet
                                    Buffer.BlockCopy( packetBuffer , 0 , packet , PacketStructure.ECHO_CONTENT_OFFSET , packetBuffer.Length );

                                    // Processa o packet
                                    Packet p = new Packet( packet , client );

                                }
                            } while (stream.DataAvailable);

                        } else
                        {
                            Console.WriteLine( "cant read" );
                            Console.WriteLine( Constants.SERVER_STREAM_UNREADABLE );
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
            */
        }
    }
}
