using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using CafeEposAPI.Models;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemAccountController : ControllerBase
    {
        private readonly ILogger<SystemAccountController> _logger;
        private readonly EposDbContext _eposDbContext;
        public SystemAccountController(ILogger<SystemAccountController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        //Get request to find system user account
        [HttpGet("GetSystemAccount")]
        public IEnumerable<SystemAccountEntity> GetSystemAccount(string token)
        {
            //check to see if a token has been entered
            if (token == null)
            {
                return new List<SystemAccountEntity>();
            }
            else
            {
                //find user via token if found return user info otherwise return empty list
                var result = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == token);
                return result == null ? new List<SystemAccountEntity>() : new List<SystemAccountEntity> { result };
            }
        }

        [HttpPost("CreateNewSystemUser")]
        public string PostNewSystemUser(CreateSystemUserModel newUser)
        {
            var newSystemAccount = new SystemAccountEntity
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Password = newUser.Password,
                Token = Guid.NewGuid().ToString()
            };

            //Create admin account
            newSystemAccount.Staff.Add(new staffLoginEntity
            {
                Name = "Admin",
                staffId = "0001",
                passcode = "1234",
                role = 1,
                archived = 0
            });

            _eposDbContext.Add(newSystemAccount);

            try
            {
                _eposDbContext.SaveChanges();

                return newSystemAccount.Token;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 401;
                return string.Empty;
            }

        }

    }
}