using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Telephone
    {
        public int Id { get; set; }
        public string TelName { get; set; }
        public int BrandId { get; set; }
        public Brand Brands { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Prise { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int Broi { get; set; }
        public DateTime Date { get; set; }
        public ICollection<TelephoneImages> TelephoneImages { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
