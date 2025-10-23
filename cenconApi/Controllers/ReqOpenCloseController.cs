using cenconApi.Data;
using cenconApi.Hubs;
using cenconApi.Model;
using cenconApi.Model.DTO;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace cenconApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReqOpenCloseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ReqOpenCloseController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // ============================
        // OPEN ORDER
        // ============================
        [HttpPost("open")]
        public async Task<ActionResult<ApiResponse<object>>> OpenOrder([FromBody] FormOpenDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Error("Input tidak valid", "400"));

            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "system";
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // 🔒 Pastikan tidak ada NOWO yang sama (race-safe)
                    var existing = await _context.ReqOpenClose
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.NOWO == request.NOWO && x.WSID == request.WSID);

                    if (existing != null)
                    {
                        return Ok(ApiResponse<object>.Error("No Work Order sudah melakukan order open cencon", "409"));
                    }

                    var entity = new ReqOpenClose
                    {
                        NOWO = request.NOWO,
                        WSID = request.WSID,
                        Tanggal = DateTime.Now.Date,
                        Jam = DateTime.Now,
                        created = username,
                        createdat = DateTime.Now,
                        // nilai optional dibiarkan null (tidak perlu set string.Empty)
                    };

                    _context.ReqOpenClose.Add(entity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Open", request.WSID);

                    return Ok(ApiResponse<object>.SuccessNoData("Order Open berhasil dibuat"));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Ok(ApiResponse<object>.Error($"Gagal Open Order: {ex.Message}", "500"));
                }
            });
        }

        // ============================
        // CLOSE ORDER
        // ============================
        [HttpPost("close")]
        public async Task<ActionResult<ApiResponse<object>>> ValidateClose([FromBody] FormCloseDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Error("Input tidak valid", "400"));

            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "system";
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var entity = await _context.ReqOpenClose
                        .FirstOrDefaultAsync(x => x.NOWO == request.NOWO);

                    if (entity == null)
                    {
                        return Ok(ApiResponse<object>.Error("Nomor order belum melakukan order open", "404"));
                    }

                    // Validasi apakah belum selesai open
                    if (string.IsNullOrEmpty(entity.JReqOpen))
                    {
                        return Ok(ApiResponse<object>.Error("Masih proses order open", "400"));
                    }

                    // Validasi apakah belum selesai open
                    //if (entity.JReqClose != request.KDClose)
                    //{
                    //    return Ok(ApiResponse<object>.Error("Kode Close tidak salah", "400"));
                    //}

                    // Update data close
                    entity.JReqClose = request.JReqClose;
                    entity.KDClose = request.KDClose;
                    entity.updated = username;
                    entity.updatedat = DateTime.Now;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Close", request.WSID);

                    return Ok(ApiResponse<object>.SuccessNoData("Order Close berhasil dibuat"));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Ok(ApiResponse<object>.Error($"Gagal Close Order: {ex.Message}", "500"));
                }
            });
        }

        // ============================
        // HELPER
        // ============================
        private bool NowoExists(string id)
        {
            return _context.ReqOpenClose.Any(e => e.NOWO == id);
        }
    }
}
