using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Data
{
    class LogzErrorResponse
    {
        public int? malformedLines;
        public int? successfulLines;
        public int? oversizedLines;
        public int? emptyLogLines;

    }
}
