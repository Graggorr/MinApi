using Microsoft.Extensions.Logging;
using WebStore.Database;

namespace WebStore.Domain.Core
{
    public class WebStoreService(WebStoreContext context, ILogger<WebStoreService> logger)
    {
        public WebStoreContext Context { get; } = context;
        public ILogger<WebStoreService> Logger { get; } = logger;
    }
}
