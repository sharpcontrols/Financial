using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Financial.Models.Contacts
{
    public class ContactModel
    {
        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? CorporateName { get; set; }
        public Dictionary<string, string> PhoneNumbers { get; set; } = [];
        public Dictionary<string, string> EmailAddresses { get; set; } = [];
        public string? AreaCode { get; set; }
        public string? Ddd { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? Complement { get; set; }
        public string? Area { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}
