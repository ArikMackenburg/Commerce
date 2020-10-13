using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Products
{
    public class Product
    {
      
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string Details { get; set; }

        
    }

    
}
