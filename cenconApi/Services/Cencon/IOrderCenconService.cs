using cenconApi.Model.ViewForm;

namespace cenconApi.Services.Cencon
{
    public interface IOrderCenconService
    {
        Task<List<ViewOrderCenconResult>> GetOrdersAsync(string? filter, DateTime? tglAwal, DateTime? tglAkhir, string? cabang, int startRow, int pageSize);
        Task<int> GetOrdersCountAsync(string? filter, DateTime? tglAwal, DateTime? tglAkhir, string? cabang);
    }
}
