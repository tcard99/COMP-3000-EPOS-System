using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly EposDbContext _eposDbContext;
        public CategoryController(ILogger<CategoryController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }


        //Method to find all parent categoies
        [HttpGet("getParentCategoires")]
        public IEnumerable<categoryEntity> GetParentCategory(string sysAccountToken)
        {
            //Find system account by passed in id
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            //If null return empty list
            if (foundSysAccount == null)
            {
                return new List<categoryEntity>();
            }

            //Find all parent categoies which belong to logged in account
            var result = _eposDbContext.Category.Where(x => x.parentId == null && x.sysAccountId == foundSysAccount.Id);

            //Check to see if there is any
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<categoryEntity>();
            }
        }

        //Method to get all child categoies
        [HttpGet("getChildCategories")]
        public IEnumerable<categoryEntity> getChildCategory(string sysAccountToken, int parentId)
        {
            //find system account
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);


            //Check to see if exists 
            if (foundSysAccount == null)
            {
                return new List<categoryEntity>();
            }


            //Find all child cateogies 
            var result = _eposDbContext.Category.Where(x => x.parentId == parentId && x.sysAccountId == foundSysAccount.Id);

            //Check to see if there is any 
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<categoryEntity>();
            }
        }

        //Method to create a new category
        [HttpPost("addNewCategory")]
        public bool addNewCategory(string sysAccountToken, CreateCategoryModel newCatModel)
        {
            //Find system account
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            //Check to see if it exists
            if (foundSysAccount == null)
            {
                return false;
            }

            //Input info into new category
            var newCategory = new categoryEntity
            {
                Name = newCatModel.Name,
                parentId = newCatModel.parentId,
                sysAccountId = foundSysAccount.Id
            };


            //Add new categotry 
            _eposDbContext.Category.Add(newCategory);

            //Try and save the new changes
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
}
