using cenconApi.Data;
using cenconApi.Model.ViewForm;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace cenconApi.Services.Cencon
{
    public class OrderCenconService : IOrderCenconService
    {
        private readonly ApplicationDbContext _context;

        public OrderCenconService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ViewOrderCenconResult>> GetOrdersAsync(
            string? filter,
            DateTime? tglAwal,   // pakai nullable DateTime?
            DateTime? tglAkhir,  // pakai nullable DateTime?
            string? cabang,
            int startRow,
            int pageSize)
        {
            // jika null atau di luar rentang SQL Server, set default
            var validTglAwal = (tglAwal.HasValue && tglAwal.Value >= new DateTime(1753, 1, 1))
                ? tglAwal.Value
                : DateTime.Today; // contoh default: 7 hari ke belakang

            var validTglAkhir = (tglAkhir.HasValue && tglAkhir.Value >= new DateTime(1753, 1, 1))
                ? tglAkhir.Value
                : DateTime.Today;

            var parameters = new[]
            {
                new SqlParameter("@filter", (object?)filter ?? DBNull.Value),
                new SqlParameter("@tglawal", validTglAwal),
                new SqlParameter("@tglakhir", validTglAkhir),
                new SqlParameter("@cabang", (object?)cabang ?? DBNull.Value),
                new SqlParameter("@sdata", 1),
                new SqlParameter("@StartRowIndex", startRow),
                new SqlParameter("@PageSize", pageSize)
            };

            return await _context.ViewOrderCenconResults
                .FromSqlRaw("EXEC dbo.Web_Asp_ViewOrderCencon @filter, @tglawal, @tglakhir, @cabang, @sdata, @StartRowIndex, @PageSize", parameters)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<int> GetOrdersCountAsync(
            string? filter,
            DateTime? tglAwal,   // ubah jadi nullable
            DateTime? tglAkhir,  // ubah jadi nullable
            string? cabang)
        {
            // validasi tanggal agar tidak keluar dari batas datetime SQL
            var validTglAwal = (tglAwal.HasValue && tglAwal.Value >= new DateTime(1753, 1, 1))
                ? tglAwal.Value
                : DateTime.Today.AddDays(-7); // default: 7 hari ke belakang

            var validTglAkhir = (tglAkhir.HasValue && tglAkhir.Value >= new DateTime(1753, 1, 1))
                ? tglAkhir.Value
                : DateTime.Today;

            var parameters = new[]
            {
                new SqlParameter("@filter", (object?)filter ?? DBNull.Value),
                new SqlParameter("@tglawal", validTglAwal),
                new SqlParameter("@tglakhir", validTglAkhir),
                new SqlParameter("@cabang", (object?)cabang ?? DBNull.Value),
                new SqlParameter("@sdata", 2)
            };

            var result = await _context.Database
                .SqlQueryRaw<int>(
                    "EXEC dbo.Web_Asp_ViewOrderCencon @filter, @tglawal, @tglakhir, @cabang, @sdata",
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}
