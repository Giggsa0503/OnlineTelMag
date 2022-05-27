using Microsoft.AspNetCore.Http;
using OnlineTelMag.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTelMag.Models
{
    public class TelephoneVM
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

        [Required(ErrorMessage = "Избери снимка от компютъра си...")]
        public List<IFormFile> ImagePath { get; set; }
    }
}
