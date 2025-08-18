using Microsoft.AspNetCore.Mvc;
using user_finder.API.Services;

namespace user_finder.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserFinderController : ControllerBase
	{
		private readonly IUserFinderService _userFinderService;

		public UserFinderController(IUserFinderService userFinderService)
		{
			_userFinderService = userFinderService;
		}

		[HttpGet("ByZipCodeAndProximity")]
		public async Task<IActionResult> FindUsersCloseToZipcode([FromQuery] string zipCode, [FromQuery] double proximityInMiles)
		{
			var results = await _userFinderService.FindUsersCloseToZipcode(zipCode, proximityInMiles);
			return Ok(results);
		}

		[HttpGet("ByZipcode")]
		public async Task<IActionResult> GetByZipcode([FromQuery] string zipCode)
		{
			var results = await _userFinderService.GetByZipCode(zipCode);
			return Ok(results);
		}
	}
}
