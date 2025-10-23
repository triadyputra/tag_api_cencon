namespace cenconApi.Model.ViewForm
{
    public class ViewOrderCenconResult
    {
        public int Id { get; set; }
        public string NOWO { get; set; } = string.Empty;
        public string WSID { get; set; } = string.Empty;
        public DateTime? Tanggal { get; set; }
        public DateTime? Jam { get; set; }
        public string? JReqOpen { get; set; }
        public string? JReqClose { get; set; }
        public string? KDClose { get; set; }
        public string? Lokasi { get; set; }
        public string? Mesin { get; set; }
        public string? NMCABANG { get; set; }
    }
}
