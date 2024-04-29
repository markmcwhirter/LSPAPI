using LSPApi.DataLayer;

using Microsoft.AspNetCore.Mvc;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthorRepository _user;

    public UserController(IAuthorRepository user)
    {
        _user = user;
    }


    [HttpGet, Route("{username}")]
    public bool CheckUsername(string username) =>
            _user.CheckUsername(username);

  
}
