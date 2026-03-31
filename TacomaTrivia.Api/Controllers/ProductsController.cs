// Test controller for our auth policies

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TacomaTrivia.Api.Auth;
[ApiController]
[Route("api/products")]
public class ProductsController: ControllerBase
{
    // Both users can access this get route
    [HttpGet]
    [Authorize(Policy = AuthConfig.ReadOnlyPolicy)]
    public IActionResult Get() => Ok("Viewing all products.");

    //Only Admin can access these
    [HttpPost]
    [Authorize(Policy = AuthConfig.AdminPolicy)]
    public IActionResult Create() => Ok("Product Created.");

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthConfig.AdminPolicy)]
    public IActionResult Delete(int id) => Ok($"Product {id} deleted.");
}