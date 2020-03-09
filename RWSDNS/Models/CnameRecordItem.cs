using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWSDNS.Api.Models
{
    public class CnameRecordItem
    {
        public string Zone { get; set; }
        public string Hostname { get; set; }
        public string PrimaryName { get; set; }
    }
}
