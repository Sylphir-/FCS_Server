using FCS_Server.refs;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FCS_Server
{
    class Server
    {
        public Thread[] threads;
        /**
         * Entry Point
         */
        public static void Main( string[] args )
        {
            Server.Start();

            Console.WriteLine( "\nServer is halted. Press any key to close the console." );
            Console.Read();
            Environment.Exit( 0 );
        }

        /**
         * Server startup
         */
        public static void Start()
        {
            // 255   00 00 00 O   '   00 00 00 0c   00 00 00 \n   2245591041   00 00 00 07   00 00 00 $   ---   00 00 00 00   ~   00 00 00 08   ~
            Console.WriteLine( "Testing Authentication packets..." );
            Byte[] testPacket = new byte[] { 
                PacketType.HEADER ,
                0x00 , 0x00 , 0x00 , 0x4f ,

                PacketType.ValidateAuthenticationKeyWithUserInfo ,
                0x00 , 0x00 , 0x00 , 0x22 ,

                0x00 , 0x00 , 0x00 , 0x0A ,
                0x32 , 0x32 , 0x34 , 0x35 , 0x35 , 0x39 , 0x31 , 0x30 , 0x34 , 0x31,
                
                0x00 , 0x00 , 0x00 , 0x07 ,

                0x00 , 0x00 , 0x00 , 0x24 ,
                0x36 , 0x41 , 0x37 , 0x45 , 0x37 , 0x36 , 0x41 , 0x37 , 0x2d , 0x38 , 0x31 , 0x35 , 0x46 , 0x2D , 0x34 ,  0x39 , 0x42 , 0x46 ,
                0x2d , 0x39 , 0x34 , 0x46 , 0x38 , 0x2d , 0x46 , 0x44 , 0x46 , 0x35 , 0x36 , 0x30 , 0x44 , 0x43 , 0x30 , 0x39 , 0x34 , 0x35 ,

                0x00 , 0x00 , 0x00 , 0x00 ,
                
                0x00 , 0x00 , 0x00 , 0x08 ,
                0x31 , 0x30 , 0x2E , 0x30 , 0x2E , 0x30 , 0x2E , 0x31
            };

            PacketProcess.ValidateAuthenticationKeyWithUserInfo( testPacket );

            Thread[] threads = new Thread[Constants.THREAD_COUNT];

            Console.Title = "MU LEGENND :: FCSA Server - Thread Count: " + threads.Length;
            for ( int i=0; i< threads.Length; i++ )
            {
                threads[i] = new Thread( ServerProcess );
            }

            /**
             * Prompt de Comando
             */
            bool quit = false;
            while( !quit)
            {
                String cmd = "";

                Console.WriteLine( "" );
                Console.Write( ">" );
                cmd = Console.ReadLine();

                int opt;
                switch (cmd)
                {
                    case "start":
                        Console.WriteLine( "Which Thread should be started?" );
                        for( int i=0; i<threads.Length; i++)
                        {
                            Console.WriteLine( i + " - Thread #" + i + "(" + threads[i].ThreadState + ")" );
                        }
                        opt = Console.Read();
                        opt = (int)Char.GetNumericValue( (char)opt );

                        Console.WriteLine( "Starting thread " + opt + "..." );
                        try
                        {
                            threads[opt].Start();
                        } catch (IndexOutOfRangeException e)
                        {
                            Console.WriteLine( "No such thread exists." );
                        }
                        break;
                    case "new":
                        Array.Resize<Thread>( ref threads , threads.Length + 1 );
                        threads[threads.Length - 1] = new Thread( ServerProcess );
                        Console.Title = "MU LEGENND :: FCSA Server - Thread Count: " + threads.Length;
                        break;
                    case "list":
                        for( int i=0; i<threads.Length; i++)
                        {
                            Console.WriteLine( "Thread #" + i + " Status: " + threads[i].ThreadState );
                        }
                        break;
                    case "quit":
                    case "exit":
                        quit = true;
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        public static void ServerProcess()
        {

            try
            {
                // Starts a new Listener for this thread
                TcpListener listener = null;
                bool foundPort = false;
                int _port = 0;
                do
                {
                    try
                    {
                        int port = Constants.port + _port;
                        listener = new TcpListener( IPAddress.Any , port );
                        listener.Start();
                        foundPort = true;
                    } catch (SocketException e)
                    {
                        foundPort = false;
                        _port++;
                    }
                } while (!foundPort);

                TcpClient client = new TcpClient();
                NetworkStream stream;

                do
                {
                    Console.WriteLine( String.Format( "[{0:HH:mm:ss}][SERVER] Waiting for connection..." , DateTime.Now ) );
                    client = listener.AcceptTcpClient();
                }
                while (!client.Connected);

                Console.WriteLine( String.Format( "[{0:HH:mm:ss}][SERVER] Connected to " + client.Client.RemoteEndPoint + "." , DateTime.Now ) );
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
        }
    }
}
