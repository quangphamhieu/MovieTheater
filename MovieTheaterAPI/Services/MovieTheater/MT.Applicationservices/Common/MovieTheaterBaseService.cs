using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MT.Infrastructure;

namespace MT.Applicationservices.Common
{
    public abstract class MovieTheaterBaseService
    {
        protected readonly ILogger _logger;
        protected readonly MovieTheaterDbContext _dbContext;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor cho các service chỉ cần Logger và DbContext
        protected MovieTheaterBaseService(ILogger logger, MovieTheaterDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // Constructor cho các service cần thêm HttpContextAccessor
        protected MovieTheaterBaseService(ILogger logger, MovieTheaterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : this(logger, dbContext) // Gọi constructor đầu tiên
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
