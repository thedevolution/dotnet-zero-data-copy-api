using user_finder.API.Domain;
using user_finder.API.Repositories;

namespace user_finder.API.Services
{
	public class UserFinderService : IUserFinderService
	{
		private readonly IFoundUserRepository _userLocationPopulationRepository;

		public UserFinderService(IFoundUserRepository userLocationPopulationRepository)
		{
			_userLocationPopulationRepository = userLocationPopulationRepository;
		}

		public async Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles)
		{
			return await _userLocationPopulationRepository.FindUsersCloseToZipcode(zipCode, proximityInMiles);
		}

		public async Task<List<FoundUser>> GetByZipCode(string zip)
		{
			return await _userLocationPopulationRepository.GetByZipCode(zip);
		}
	}
}
