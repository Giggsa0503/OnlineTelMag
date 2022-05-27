using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineTelMag.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTelMag.Models
{
    public class TelephoneDetailsVM
    {
        public int Id { get; set; }
        public string TelName { get; set; }
        public int BrandId { get; set; }
        public List<SelectListItem> Brands { get; set; }
        public decimal Prise { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int Broi { get; set; }
        public DateTime Date { get; set; }
        public List<string> ImagesPaths { get; set; }
    }
}
