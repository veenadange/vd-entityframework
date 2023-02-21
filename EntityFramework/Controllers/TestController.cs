using EntityFrameworkRls.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkRls.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        readonly PtDbContext _dbContext;
        public TestController(PtDbContext dbContext) {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get clients for current session
        /// </summary>
        /// <param name="tenantId">Set session for rls</param>
        /// <returns></returns>
        [HttpGet("/clients")]
        public IEnumerable<Client> GetClients([FromQuery]Guid tenantId) 
        {
            return _dbContext.Clients;
        }

        /// <summary>
        /// Post client for current session
        /// </summary>
        /// <param name="tenantId">Set session for rls</param>
        /// <returns></returns>
        [HttpPost("/clients")]
        public void GetClients([FromBody]Client client, [FromQuery] Guid tenantId)
        {
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();
        }
    }
}