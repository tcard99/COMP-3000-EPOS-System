﻿using CafeEposAPI.Data;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly EposDbContext _eposDbContext;
        public AuthController(ILogger<AuthController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        [HttpPost("SystemAccountLogin")]
        public string systemUserLogin(SystemUserLoginModel systemLogin)
        {
            var user = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Email == systemLogin.Eamil && x.Password == systemLogin.Password);

            if (user != null)
            {
                return user.Token;
            }

            else
            {
                return string.Empty;
            }
        }

        [HttpPost("StaffLogin")]
        public IEnumerable<ReturnStaffAuthModel> StaffLogin(StaffLoginModel staffLogin)
        {
            var userId = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == staffLogin.token);

            if (userId == null)
            {
                return new List<ReturnStaffAuthModel>();
            }

            var user = _eposDbContext.StaffAccounts.SingleOrDefault(x => x.staffId == staffLogin.staffId && x.passcode == staffLogin.passcode && x.sysAccountId == userId.Id);


            if (user != null)
            {
                var returnData = new ReturnStaffAuthModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    role = user.role
                };
                return new List<ReturnStaffAuthModel> { returnData };
            }
            else
            {
                return new List<ReturnStaffAuthModel>();
            }

        }
    }
}