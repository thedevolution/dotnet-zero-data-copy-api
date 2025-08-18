using user_finder.API.Domain;

namespace user_finder.API.Repositories
{
	public interface IFoundUserRepository
	{
		Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles);
		Task<List<FoundUser>> GetByZipCode(string zip);
	}
}
