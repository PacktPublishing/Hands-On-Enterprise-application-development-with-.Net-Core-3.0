using System.ComponentModel.DataAnnotations;

namespace Draken.Domain.Entities
{
    public class Contact : Entity
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Designation { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string BusinessPhone { get; set; }

        public string MobilePhone { get; set; }

        public int Status { get; set; }

        public string LinkedIn { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string ZipCode { get; set; }
    }
}
