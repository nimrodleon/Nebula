using System;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class PeopleDocType
    {
        [Key] public Guid Id { get; set; }
        [MaxLength(250)] public string Description { get; set; }
        [MaxLength(250)] public string SunatCode { get; set; }
    }
}
