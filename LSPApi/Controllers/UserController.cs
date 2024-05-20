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

	[HttpGet, Route("email/{email}")]
	public string GetUsername(string email) =>
		_user.GetUsername(email);

    [HttpGet, Route("username/{username}")]
    public string GetEmail(string username) =>
        _user.GetEmail(username);


}
