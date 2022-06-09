using static Contact.Models.Enums;

namespace Contact.Models
{
    public class ContactInfo
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}
