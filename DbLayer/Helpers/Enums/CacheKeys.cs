namespace DbLayer.Helpers.Enums
{
	/// <summary>
	/// Storing cache keys here
	/// </summary>
	public enum CacheKeys
	{
		None                = 0,
		ProductsCacheKey    = 1,
		ProductByIdCacheKey = 2
	}

	/// <summary>
	/// Choosing caching time in minutes
	/// </summary>
	public enum CachingTimeInMinutes
	{
		FiveMinutes    = 5,
		TenMinutes     = 10,
		fifteenMinutes = 15,
		ThirtyMinutes  = 30,
		SixtyMinutes   = 60
	}
}
