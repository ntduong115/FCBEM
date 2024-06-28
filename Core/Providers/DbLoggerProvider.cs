using Core.Models.Systems;
using Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Core.Providers
{
    [ProviderAlias("Database")]
    public class DbLoggerProvider(IOptions<DbLoggerOptions> _options) : ILoggerProvider
    {

        public readonly DbLoggerOptions Options = _options.Value;

        /// <summary>  
        /// Creates a new instance of the db logger.  
        /// </summary>  
        /// <param name="categoryName"></param>  
        /// <returns></returns>  
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
