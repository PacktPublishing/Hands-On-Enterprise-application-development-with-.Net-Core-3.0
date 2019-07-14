using System.ComponentModel.DataAnnotations;

namespace Draken.Data
{
    public class Contact : Model
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Designation { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string BusinessPhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }
        public int Status { get; set; }
        [DataType(DataType.Url)]
        public string LinkedIn { get; set; }
        [Required]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
    }
}
