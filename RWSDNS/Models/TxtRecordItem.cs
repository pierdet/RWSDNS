using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWSDNS.Api.Models
{
    public class TxtRecordItem
    {
        public string Hostname { get; set; }
        public string DescriptiveText { get; set; }
        public string Zone { get; set; }
    }
}
