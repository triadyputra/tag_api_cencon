using cenconApi.Data;
using cenconApi.Model.DTO;
using cenconApi.Model.ViewForm;
using cenconApi.Services.Cencon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cenconApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCenconController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly DapperContext _daperContext;
        private readonly IOrderCenconService _service;

        public OrderCenconController(ApplicationDbContext context, DapperContext daperContext, IOrderCenconService service)
        {
            _context = context;
            _daperContext = daperContext;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ViewOrderCenconResult>>> GetOrders(
        string? filter,
        DateTime tglAwal,
        DateTime tglAkhir,
        string? cabang,
        int start = 1,
        int pageSize = 20)
        {
            var data = await _service.GetOrdersAsync(filter, tglAwal, tglAkhir, cabang, start, pageSize);
            var count = await _service.GetOrdersCountAsync(filter, tglAwal, tglAkhir, cabang);

            return Ok(new PaginatedResponse<ViewOrderCenconResult>
            {
                Data = data,
                TotalCount = count,
                Page = start,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            });
        }
    }
}
