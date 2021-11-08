using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class PeopleDocType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(250)] public string Description { get; set; }
        [MaxLength(250)] public string SunatCode { get; set; }

        [JsonIgnore] public List<Contact> Contacts { get; set; }
    }
}
