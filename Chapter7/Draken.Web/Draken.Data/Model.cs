using System;

namespace Draken.Data
{
    public class Model
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "SYSTEM";
        public string ModifiedBy { get; set; } = "SYSTEM";
    }
}
