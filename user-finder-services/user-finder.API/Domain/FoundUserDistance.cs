namespace user_finder.API.Domain
{
	public class FoundUserDistance : User
	{
		public string Zip { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string StateAbbr { get; set; } = string.Empty;
		public double DistanceInMiles { get; set; }
	}
}
