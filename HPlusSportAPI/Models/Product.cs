using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace HPlusSportAPI.Models
{
    public class Product
    {
        public Product()
        { }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }



    }
}

