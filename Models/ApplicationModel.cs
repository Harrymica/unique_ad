using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace uniquead_App.Models
{
    [Table("ApplicationDb")]
    public class ApplicationModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("contact")]
        public string Contact { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("apply_as")]
        public string Apply_As { get; set; }
        [Column("cv_url")]
        public string Cv { get; set; }
        [Column("status")]
        public string status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
