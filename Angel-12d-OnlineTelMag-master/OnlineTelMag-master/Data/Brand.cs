using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Data
{
    public class Brand
    {
        public int Id { get; set; }
        public string NameBrand { get; set; }
        public string NameModel { get; set; }
        public ICollection<Telephone> Telephones { get; set; }

    }
}
