using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Server.refs
{
    internal class ResultCode
    {
        public const Int32 kRCSuccess = 1;
        public const Int32 kRCFailure = 2;
        public const Int32 kRCException = 3;
        public const Int32 kRCNoResult = 4;
        public const Int32 kRCFailureTransactionalOperation = 5;
        public const Int32 kRCFCSAdapterError = 9;
    }
}
