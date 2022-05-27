using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class OOrderList
    {
        public int Id { get; set; }
        public int TelephoneId { get; set; }
        public Telephone Telephones { get; set; }
        public string OrderId { get; set; }
        public Order Orders { get; set; }
        public int Quantity { get; set; }
    }
}
