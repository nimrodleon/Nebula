using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Document { get; set; }
        [MaxLength(250)] public string Name { get; set; }
        [MaxLength(250)] public string Address { get; set; }
        [MaxLength(250)] public string PhoneNumber1 { get; set; }
        [MaxLength(250)] public string PhoneNumber2 { get; set; }
        [MaxLength(250)] public string Email { get; set; }
        [DefaultValue(false)] public bool SoftDeleted { get; set; }
    }
}
