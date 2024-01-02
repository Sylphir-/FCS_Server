using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public class Constants
    {
        public const Int32 port = 27015;
        public static IPAddress host = IPAddress.Parse( "127.0.0.1" );
        public const int THREAD_COUNT = 1;

        /*************************************************
         *      STRINGS                                  *
         *************************************************/
        public const string SERVER_CONN_WAITING = "Aguardando Conexao";
        public const string SERVER_STREAM_UNREADABLE = "Dados recebidos ilegiveis";
        public const string SERVER_STREAM_PACKET_RECEIVED = "Pacote Recebido: ";
        public const string SEND_RESPONSE_EXCEPTION_CANT_WRITE = "Conexão não permite escrita, resposta abortada.";
    }
}
