using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace uniquead_App.Models
{
    [Table("orderdb")]
    public class OrderItem : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("userid")]
        public Guid userId { get; set; }
        [Column("items")]
        public string Items { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("total")]
        public decimal Total { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("image_src")]
        public string ImageUrl { get; set; }
        [Column("tag")]
        public string Tag { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
    }
}
