using static Contact.Models.Enums;

namespace Contact.Models
{
    public class ContactInfo
    {
        public Guid ID { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}
