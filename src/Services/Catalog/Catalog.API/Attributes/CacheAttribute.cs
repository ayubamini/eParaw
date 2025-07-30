using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Attributes
{
    public class CacheAttribute: ResponseCacheAttribute
    {
        public CacheAttribute(int durationInSeconds = 300) // Default 5 minutes
        {
            Duration = durationInSeconds;
            Location = ResponseCacheLocation.Any;
            VaryByQueryKeys = new[] { "*" }; // Vary by all query parameters
        }
    }
}
