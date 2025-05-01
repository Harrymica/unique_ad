using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace uniquead_App.Models
{
    [Table("userdb")]
    public class Users : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }
        [Column("phone")]

        public string Phone { get; set; }
        [Column("username")]

        public string username { get; set; }
        [Column("password")]

        public string Password { get; set; }

        [Column("userid")]

        public string userId { get; set; }


    }
}
