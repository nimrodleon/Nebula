using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Document { get; set; }
        public Guid? PeopleDocTypeId { get; set; }

        [JsonIgnore]
        [ForeignKey("PeopleDocTypeId")]
        public PeopleDocType PeopleDocType { get; set; }

        [MaxLength(250)] public string Name { get; set; }
        [MaxLength(250)] public string Address { get; set; }
        [MaxLength(250)] public string PhoneNumber1 { get; set; }
        [MaxLength(250)] public string PhoneNumber2 { get; set; }
        [MaxLength(250)] public string Email { get; set; }
    }
}
