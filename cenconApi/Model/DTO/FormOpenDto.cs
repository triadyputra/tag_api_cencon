using System.ComponentModel.DataAnnotations;

namespace cenconApi.Model.DTO
{
    public class FormOpenDto
    {
        [Required(ErrorMessage = "NOWO wajib diisi")]
        [StringLength(50, ErrorMessage = "NOWO maksimal 50 karakter")]
        public string NOWO { get; set; } = string.Empty;

        [Required(ErrorMessage = "WSID wajib diisi")]
        [StringLength(50, ErrorMessage = "WSID maksimal 50 karakter")]
        public string WSID { get; set; } = string.Empty;
    }
}
