using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWSDNS.Api.Models
{
    public class ARecordItem
    {
        public string Hostname { get; set; }
        public string IPAddress { get; set; }
        public string Zone { get; set; }
    }
}
