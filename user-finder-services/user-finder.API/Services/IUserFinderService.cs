using user_finder.API.Domain;

namespace user_finder.API.Services
{
	public interface IUserFinderService
	{
		Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles);
		Task<List<FoundUser>> GetByZipCode(string zip);
	}
}
