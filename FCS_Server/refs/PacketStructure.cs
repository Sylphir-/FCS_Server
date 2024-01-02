using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /*********************************************** COMMON */
        public const short CONDITION_TYPE_LENGTH = 1;
        public const short RESULT_CODE_LENGTH = 4;
        public const short STRUCTURE_TYPE_LENGTH = 4;
        public const short SERVICE_CODE_LENGTH = 4;

        public const short INT_LENGTH = 4;
        public const short BYTE_LENGTH = 1;

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

        //********************************************************************************************* Initialize

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
        public const short INITIALIZE_WORLD_NUMBER_LENGTH = 4;
        public const short INITIALIZE_KEEP_ALIVE_LENGTH = 4;

        // Offset
        public const short INITIALIZE_SERVICE_CODE_LENGTH_OFFSET = 10;
        public const short INITIALIZE_SERVICE_CODE_OFFSET = 13;

        // Response
        public const short INITIALIZE_RESULT_CODE_OFFSET = 10;
        public const short INITIALIZE_CONDITION_TYPE_OFFSET = 14;
        public const short INITIALIZE_RESPONSE_SERVICE_CODE_OFFSET = 15;

        //********************************************************************************************* Keep Alive

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

        //************************************************************************ Validate Authentication Key With User Info

        // Request
        // .--------------------.----------------.--------------------.-----------.-------------------.
        // | Callback Attribute | Account Number | Authentication Key | Extension |     Client IP     |
        // |--------------------+----------------+--------------------+-----------+-------------------|
        // | 10                 | ~              | ~                  | ~         | ~                 |
        // | Int (Len) + String | Int            | Int(Len) + String  | Int+Str   | Int(Len) + String |
        // '--------------------'----------------'--------------------'-----------'-------------------'

        // Response
        // .-------------.----------------.--------------------.---------------.----------.---------.------------.------------.-----------.
        // | Result Code | Condition Type | Callback Attribute | Provider Code |  User No | User ID | Account No | Account ID | Extension |
        // |-------------+----------------+--------------------+---------------+----------+---------+------------+------------+-----------|
        // | 10 11 12 13 |       14       | 15                 | ~             | ~        | ~       | ~          | ~          | ~         |
        // |   Integer   |      Byte      | Int+Str            | Int+Str       | Int      | Str     | Int        | Str        | JSON      |
        // '-------------'----------------'--------------------'---------------'----------'---------'------------'------------'-----------'

        public const short AUTH_KEY_WITH_USER_INFO_CONDITION_TYPE_OFFSET = 14;
        public const short AUTH_KEY_WITH_USER_INFO_CALLBACK_ATTRIBUTE_OFFSEST = 15;

    }
}
