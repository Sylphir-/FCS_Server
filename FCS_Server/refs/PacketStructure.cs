using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    public class PacketStructure
    {
        /************************************************************************************************************************/
        /*                                                PACKET          STRUCTURE                                             */
        /************************************************************************************************************************/
        // .--------------------------+------------------------------+-------------------------.
        // |          Header          |          Echo Content        |         Data            |
        // |--------------------------+-------------+----------------+-------------------------|
        // | Reserved | Packet Length | Packet Type | Transaction ID |      Packet Data        |
        // |----------+---------------+-------------+----------------+-------------------------|
        // |    FF    |  00 00 00 00  |     00      |   00 00 00 00  | 00 00 00 00 00 00 00 ~~ |
        // |     0    |   1  2  3  4  |      5      |    6  7  8  9  | 10 11 12 13 14 15 16 ~~ |
        // '----------'---------------'-------------'----------------'-------------------------'


        /*********************************************** HEADER */
        // Lenghts
        public const short HEADER_LENGTH = 5;
        public const short ECHO_CONTENT_LENGTH = 5;
        //  // Header
        public const short HEADER_RESERVED_LENGTH = 1;
        public const short PACKET_LENGTH_LENGTH = 4;
        //  // Echo Content
        public const short PACKET_TYPE_LENGTH = 1;
        public const short TRANSACTION_ID_LENGTH = 4;

        // Offsets
        public const short HEADER_OFFSET = 0;
        public const short ECHO_CONTENT_OFFSET = 5;
        public const short PACKET_DATA_OFFSET = 10;
        //  // Header
        public const short HEADER_RESERVED_OFFSET = 0;
        public const short PACKET_LENGTH_OFFSET = 1;
        //  // Echo Content
        public const short PACKET_TYPE_OFFSET = 5;
        public const short TRANSACTION_ID_OFFSET = 6;

        /*********************************************** DATA */

        // Initialize

        // Request
        // .---------------------.---------------.-------------.--------------------------.
        // | Service Code Length | Service Code  |  World No   | Client Keep-Alive Period |
        // |---------------------+---------------+-------------+--------------------------|
        // |     00 00 00 00     |  00 00 00 ~~  | 00 00 00 00 |        00 00 00 00       |
        // |     10 11 12 13     |  13 15 16 ~~  | ~~ ~~ ~~ ~~ |        ~~ ~~ ~~ ~~       |
        // |        Integer      |    String     |   Integer   |          Integer         |
        // '---------------------'---------------'-------------'--------------------------'

        // Response
        // .-------------.----------------.---------------.-------------.
        // | Result Code | Condition Type |  Service Code |  World No   |
        // |-------------+----------------+---------------+-------------|
        // | 00 00 00 00 |       00       | 00 00 00 ~~   | 00 00 00 00 |
        // | 10 11 12 13 |       14       | 15 16 17 ~~   | ~~ ~~ ~~ ~~ |
        // |   Integer   |      Byte      |    String     |   Integer   |
        // '-------------'----------------'---------------'-------------'

        // Length
        public const short INITIALIZE_SERVICE_CODE_LENGTH = 4;
        public const short INITIALIZE_WORLD_NUMBER_LENGTH = 4;
        public const short INITIALIZE_KEEP_ALIVE_LENGTH = 4;

        // Offset
        public const short INITIALIZE_SERVICE_CODE_LENGTH_OFFSET = 10;
        public const short INITIALIZE_SERVICE_CODE_OFFSET = 13;

        // Response
        public const short INITIALIZE_RESULT_CODE_LENGTH = 4;
        public const short INITIALIZE_RESULT_CODE_OFFSET = 10;
        public const short INITIALIZE_CONDITION_TYPE_LENGTH = 1;
        public const short INITIALIZE_CONDITION_TYPE_OFFSET = 14;
        public const short INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET = 15;

        // Keep Alive

        // Request
        // .-------------------------.
        // | Caching Product Version |
        // |-------------------------+
        // | 00 00 00 00 00 00 00 00 |
        // | 10 11 12 13 14 15 16 17 |
        // |           Long          |
        // '-------------------------'

        // Response
        // .-------------.----------------.
        // | Result Code | Condition Type |
        // |-------------+----------------+
        // | 00 00 00 00 |       00       |
        // | 10 11 12 13 |       14       |
        // |   Integer   |      Byte      |
        // '-------------'----------------'

        // Length
        public const short KEEPALIVE_CACHING_PRODUCT_VERSION_LENGTH = 8;

        // Offset
        public const short KEEPALIVE_CACHING_PRODUCT_VERSION_OFFSET = 10;
        public const short KEEPALIVE_RESPONSE_RESULT_CODE_OFFSET = 10;
        public const short KEEPALIVE_RESPONSE_CONDITION_TYPE_OFFSET = 14;
    }
}
