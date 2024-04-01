namespace CosmosDB.Models
{
    public class Issue
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Unik identifikator for hver issue

        public CustomerInformation customerInformation { get; set; } // Kundeinformation

        public InquiryInformation inquiryInformation { get; set; } // Henvendelsesinformation
        
    }

    public class CustomerInformation
    {
        public string CustomerId { get; set; } = Guid.NewGuid().ToString(); // Unik identifikator for hver kunde
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class InquiryInformation
    {
        public string InquiryId { get; set; } = Guid.NewGuid().ToString(); // Unik identifikator for hver henvendelse
        public string Message { get; set; }
        public DateTime InquiryDate { get; set; } = DateTime.Now;
        public string Category { get; set; }
    }
}
