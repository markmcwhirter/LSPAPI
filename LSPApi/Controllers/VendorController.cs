using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly ILogger<VendorController> _logger;
        private readonly IVendorRepository _Vendor;
        private readonly IConfiguration _configuration;

        public VendorController(ILogger<VendorController> logger, IConfiguration configuration, IVendorRepository Vendor)
        {
            _logger = logger;
            _Vendor = Vendor;
            _configuration = configuration;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<model.VendorDto> GetById(int id) => await _Vendor.GetById(id);

        [HttpGet]
        public async Task<IEnumerable<model.VendorDto>> GetAll() => await _Vendor.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] model.VendorDto Vendor) => await _Vendor.Add(Vendor);
    }
}
