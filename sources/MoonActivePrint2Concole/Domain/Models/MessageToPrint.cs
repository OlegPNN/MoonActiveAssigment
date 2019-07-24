using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoonActivePrint2Concole.Domain.Models
{
    public class MessageToPrint
    {
        public string Message { get; set; }
        public string PrintTimeStr { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? Sequence { get; set; }


    }
}
