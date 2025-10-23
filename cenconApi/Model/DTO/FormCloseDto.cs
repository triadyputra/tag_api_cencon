using System.ComponentModel.DataAnnotations;

namespace cenconApi.Model.DTO
{
    public class FormCloseDto
    {
        [Required(ErrorMessage = "NOWO wajib diisi")]
        [StringLength(50, ErrorMessage = "NOWO maksimal 50 karakter")]
        public string NOWO { get; set; } = string.Empty;

        [Required(ErrorMessage = "WSID wajib diisi")]
        [StringLength(50, ErrorMessage = "WSID maksimal 50 karakter")]
        public string WSID { get; set; } = string.Empty;

        [Required(ErrorMessage = "JReqClose wajib diisi")]
        [StringLength(50, ErrorMessage = "WSID maksimal 50 karakter")]
        public string JReqClose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kode Close wajib diisi")]
        [StringLength(50, ErrorMessage = "Kode Close maksimal 50 karakter")]
        public string KDClose { get; set; } = string.Empty;
    }
}
