namespace Contact.Models
{
    public  class Person
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }

        public List<ContactInfo> ContactInfos { get; set; }

    }
}