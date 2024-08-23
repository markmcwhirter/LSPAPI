using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using Model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly IVendorRepository _Vendor;

    public VendorController(IVendorRepository Vendor)
    {
        _Vendor = Vendor;
    }


    [HttpGet, Route("{id:int}")]
    public async Task<Model.VendorDto> GetById(int id) => await _Vendor.GetById(id);

    [HttpGet]
    public async Task<IEnumerable<Model.VendorDto>> GetAll() => await _Vendor.GetAll();

    [HttpPost]
    public async Task Insert([FromBody] Model.VendorDto Vendor) => await _Vendor.Add(Vendor);
}
