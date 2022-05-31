using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineTelMag.Data
{
    public class TelephoneImages
    {
        public TelephoneImages()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
       [Required]
        public int TelephoneId { get; set; }
        public Telephone Telephones { get; set; }

    }
}
