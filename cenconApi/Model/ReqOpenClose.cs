using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cenconApi.Model
{
    [Table("ReqOpenClose")]
    public class ReqOpenClose  : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string NOWO { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string WSID { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? JReqOpen { get; set; } //= string.Empty;

        [MaxLength(10)]
        public string? JReqClose { get; set; } //= string.Empty;

        [MaxLength(10)]
        public string? KDClose { get; set; } //= string.Empty;

        public DateTime Tanggal { get; set; }

        public DateTime Jam { get; set; } 
    }
}
