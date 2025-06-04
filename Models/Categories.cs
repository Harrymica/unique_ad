using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace uniquead_App.Models
{
    [Table("categories")]
    public class Categories : BaseModel
    {
       
            [PrimaryKey("id", false)]
            public Guid Id { get; set; }
            [Column("name")]
            public string Name { get; set; }
            
            [Column("description")]
            public string Description { get; set; }
            [Column("image_url")]

            public string? ImageUrl { get; set; }
            [Column("is_active")]

            public bool IsActive { get; set; } = true;
            [Column("sort_order")]

            public int SortOrder { get; set; } = 0;
            [Column("status")]

            public string Status { get; set; }

            [Column("created_at")]

            public DateTime Date { get; set; }

            [Column("price_range")]

            public string Price { get; set; }

    }
}
