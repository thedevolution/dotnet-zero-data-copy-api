namespace user_finder.API.Repositories.EF.DataPlatforms
{
	/// <summary>
	/// Enumeration of all the supported vendors in this codebase
	/// </summary>
	internal enum DataPlatformVendor
	{
		Databricks,
		Snowflake,
		NoneConfigured
	}

	internal static class DataPlatformConfigurationExtensions
	{
		/// <summary>
		/// Extension method used to simplify the Program.cs to choose which vendor we are using at runtime.  Currently
		/// supports Snowflake, Databricks
		/// </summary>
		/// <param name="builder"></param>
		public static void ConfigureDataPlatform(this WebApplicationBuilder builder)
		{
			var configuration = builder.Configuration;
			var selectedDataPlatform = DataPlatformVendor.NoneConfigured;
			try
			{
				selectedDataPlatform = configuration.GetValue<DataPlatformVendor>("DataPlatform:Vendor");
			}
			catch (Exception)
			{
				// Do nothing, selectedDataPlatform is set to NoneConfigured
			}
			switch (selectedDataPlatform)
			{
				case DataPlatformVendor.Snowflake:
					builder.ConfigureSnowflake();
					break;
				case DataPlatformVendor.Databricks:
					builder.ConfigureDatabricks();
					break;
				default:
					throw new ArgumentException("Configure DataPlatform:Vendor with a valid DataPlatformVendor enum value");
			}
		}
	}
}
