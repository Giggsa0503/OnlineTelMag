using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Accessoares
    {
        public int Id { get; set; }
        public int AccName { get; set; }
        public int TypeId { get; set; }
        public Type Types { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Broi { get; set; }
        public DateTime Date { get; set; }
       // public ICollection<OrderList> OrderLists { get; set; }
    }
}
