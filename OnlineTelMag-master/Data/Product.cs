using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

       
    }
}
