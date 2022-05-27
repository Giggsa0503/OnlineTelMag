using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Accessoares> Accessoaress { get; set; }
    }
}
