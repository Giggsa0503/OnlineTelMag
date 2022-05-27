using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Order
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public User Users { get; set; }
        public int TelephoneId { get; set; }
        public Telephone Telephones { get; set; }
        public DateTime DateRegister { get; set; }
       // public ICollection<OrderList> OrderLists { get; set; }

    }
}
