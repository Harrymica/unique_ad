using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace uniquead_App.Models
{
    [Table("feedbackDb")]
    public class feedBackModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("subject")]
        public string Subject { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("status")]
        public bool status { get; set; }
        [Column("Date")]
        public DateTime Date { get; set; }

    }
}
