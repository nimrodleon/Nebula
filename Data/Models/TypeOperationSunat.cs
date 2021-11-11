using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class TypeOperationSunat
    {
        [Key] [MaxLength(250)] public string Id { get; set; }
        [MaxLength(250)] public string Description { get; set; }
    }
}
