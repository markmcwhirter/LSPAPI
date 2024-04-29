using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthorRepository _user;

    public UserController(IAuthorRepository author)
    {
        _user = author;
    }


    [HttpGet, Route("{username}")]
    public bool CheckUsername(string checkUsername) =>
            _user.CheckUsername(checkUsername);

  
}
