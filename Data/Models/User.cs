using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Name { get; set; }
        [MaxLength(250)] public string UserName { get; set; }
        [MaxLength(250)] public string Password { get; set; }
        [MaxLength(250)] public string Role { get; set; }
        [MaxLength(250)] public string Email { get; set; }
        [DefaultValue(false)] public bool Suspended { get; set; }
        [DefaultValue(false)] public bool SoftDeleted { get; set; }
    }
}
