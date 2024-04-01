namespace CosmosDB.Models
{
    public class Issue
    {
        public string Id { get; set; } // Unik identifikator for hver issue
        public string category { get; set; } // Anvendes af CosmosDB til partitionkey

        // Kontaktinformation
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Henvendelsesinformation
        public DateTime InquiryDate { get; set; } = DateTime.Now;
        public string Message { get; set; }
        // CategoryType er fjernet
    }
}
