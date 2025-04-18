using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffManagmentController : ControllerBase
    {
        private readonly ILogger<StaffManagmentController> _logger;
        private readonly EposDbContext _eposDbContext;
        public StaffManagmentController(ILogger<StaffManagmentController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        //Get all staff accounts
        [HttpGet("GetAlStaff")]
        public IEnumerable<StaffAccountReturnModel> GetAllStaff(string sysAccountToken)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new List<StaffAccountReturnModel>();
            }

            var allStaff = _eposDbContext.StaffAccounts.Where(x => x.sysAccountId == foundUser.Id)
                .Select(x => new StaffAccountReturnModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    staffId = x.staffId,
                    passcode = x.passcode,
                    role = x.role,
                    archived = x.archived
                });

            return allStaff;
        }

        //Method to get single staff user
        [HttpGet("GetSpecificUser")]
        public StaffAccountReturnModel GetSingleStaffAccount(string sysAccountToken, int staffId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new StaffAccountReturnModel();
            }

            var foundStaff = _eposDbContext.StaffAccounts.SingleOrDefault(x => x.Id == staffId);

            if (foundStaff == null)
            {
                return new StaffAccountReturnModel();
            }

            var returnStaff = new StaffAccountReturnModel
            {
                Id = foundStaff.Id,
                Name = foundStaff.Name,
                staffId = foundStaff.staffId,
                passcode = foundStaff.passcode,
                role = foundStaff.role,
                archived = foundStaff.archived
            };

            return returnStaff;
        }

        //Method to make new staff account
        [HttpPost("MakeNewStaff")]
        public bool MakeNewStaffAccount(string sysAccountToken, MakeStaffAccountModel model)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            var newStaff = new staffLoginEntity
            {
                sysAccountId = foundUser.Id,
                Name = model.Name,
                staffId = model.staffId,
                passcode = model.passcode,
                role = model.role,
                archived = 0
            };

            _eposDbContext.Add(newStaff);
            try
            {
                _eposDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Method to update staff account info
        [HttpPut("UpdateStaffInfo")]
        public bool UpdateStaffInfo(string sysAccounToken, UpdateStaffInfoModel staffInfo)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccounToken);

            if (foundUser == null)
            {
                return false;
            }

            var foundStaff = _eposDbContext.StaffAccounts.SingleOrDefault(x => x.Id == staffInfo.Id);

            if (foundStaff == null)
            {
                return false;
            }

            //Dont allow default created account to be changed
            if (foundStaff.staffId == "0001")
            {
                Response.StatusCode = 304;
                return false;
            }
            else
            {
                foundStaff.Name = staffInfo.Name;
                foundStaff.staffId = staffInfo.StaffId;
                foundStaff.passcode = staffInfo.Passcode;
                foundStaff.role = staffInfo.Role;

                try
                {
                    _eposDbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        //Method to delete staff account
        [HttpDelete("DelStaffAccount")]
        public bool DelStaffAccount(string sysAccountToken, int id)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            var foundStaff = _eposDbContext.StaffAccounts.SingleOrDefault(x => x.Id == id);

            if (foundStaff == null)
            {
                return false;
            }

            if (foundStaff.staffId == "0001")
            {
                Response.StatusCode = 304;
                return false;
            }
            else
            {
                _eposDbContext.StaffAccounts.Remove(foundStaff);
                _eposDbContext.SaveChanges();
                return true;
            }
        }
    }
}
