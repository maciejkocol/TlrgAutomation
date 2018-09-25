using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlrgAutomation.Pages.Tlrg
{
    class HomePageRow
    {
        public string LoadId { get; set; }
        public string Carrier { get; set; }
        public string CarrierRank { get; set; }
        public string Cogs { get; set; }
        public string Status { get; set; }
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string PUDate { get; set; }
    }
}
