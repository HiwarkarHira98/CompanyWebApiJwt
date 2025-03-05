namespace CompanyApiJwt.Models
{
    public class Company
    {
        public required string CompanyName { get; set; }
        public required string CompanyAddress { get; set; }
        public required string CompanyGSTN { get; set; }
        public required string CompanyCode { get; set; }
        public required string CompanyUserId { get; set; }
        public required string CompanyStatus { get; set; }
        public DateTime CompanyStartDate { get; set; }
        public required string CompanyNatureOfWork { get; set; }
    }
}
