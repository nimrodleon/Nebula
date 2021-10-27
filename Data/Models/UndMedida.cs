using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class UndMedida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(250)] public string Name { get; set; }
        [MaxLength(250)] public string SunatCode { get; set; }

        public List<Product> Products { get; set; }
    }
}
