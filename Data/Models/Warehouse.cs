using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(250)] public string Name { get; set; }
        [MaxLength(250)] public string Remark { get; set; }
    }
}
