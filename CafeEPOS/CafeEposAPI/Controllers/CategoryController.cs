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


        //Method to get all categoies
        [HttpGet("getAllCategoies")]
        public IEnumerable<ReturnCategoryModel> GetAllCategoies(string sysAccountToken)
        {
            //Find system account by passed in id
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            //If null return empty list
            if (foundSysAccount == null)
            {
                return new List<ReturnCategoryModel>();
            }

            //Find all categoies which belong to logged in account
            var result = _eposDbContext.Category.Where(x => x.sysAccountId == foundSysAccount.Id && x.archived == 0)
                .Select(x => new ReturnCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    parentId = x.parentId,
                    parentName = x.parentCategory.Name == null ? "N/A" : x.parentCategory.Name,
                    archived = x.archived
                });

            //Check to see if there is any
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<ReturnCategoryModel>();
            }
        }


        //Method to find all parent categoies
        [HttpGet("getParentCategoires")]
        public IEnumerable<ReturnCategoryModel> GetParentCategory(string sysAccountToken)
        {
            //Find system account by passed in id
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            //If null return empty list
            if (foundSysAccount == null)
            {
                return new List<ReturnCategoryModel>();
            }

            //Find all parent categoies which belong to logged in account
            var result = _eposDbContext.Category.Where(x => x.parentId == null && x.sysAccountId == foundSysAccount.Id && x.archived == 0)
                .Select(x => new ReturnCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    parentId = x.parentId,
                    parentName = x.parentCategory.Name,
                    archived = x.archived
                });

            //Check to see if there is any
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<ReturnCategoryModel>();
            }
        }

        //Method to get all child categoies
        [HttpGet("getChildCategories")]
        public IEnumerable<ReturnCategoryModel> getChildCategory(string sysAccountToken, int parentId)
        {
            //find system account
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);


            //Check to see if exists 
            if (foundSysAccount == null)
            {
                return new List<ReturnCategoryModel>();
            }


            //Find all child cateogies 
            var result = _eposDbContext.Category.Where(x => x.parentId == parentId && x.sysAccountId == foundSysAccount.Id && x.archived == 0)
                .Select(x => new ReturnCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    parentId = x.parentId,
                    parentName = x.parentCategory.Name,
                    archived = x.archived
                });

            //Check to see if there is any 
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<ReturnCategoryModel>();
            }
        }

        //Method to get specified category
        [HttpGet("getSingleCategory")]
        public IEnumerable<ReturnCategoryModel> GetSingleCategory(string sysAccountToken, int catId)
        {
            //find system account
            var foundSysAccount = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);


            //Check to see if exists 
            if (foundSysAccount == null)
            {
                return new List<ReturnCategoryModel>();
            }


            //Find all child cateogies 
            var result = _eposDbContext.Category.Where(x => x.Id == catId && x.sysAccountId == foundSysAccount.Id && x.archived == 0)
                .Select(x => new ReturnCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    parentId = x.parentId,
                    parentName = x.parentCategory.Name,
                    archived = x.archived
                });

            //Check to see if there is any 
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<ReturnCategoryModel>();
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
                sysAccountId = foundSysAccount.Id,
                archived = 0
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

        //Update method
        [HttpPut("UpdateCategory")]
        public bool UpdateCategory(string sysAccountToken, int catId, CreateCategoryModel updateCatInfo)
        {
            //Find user
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            //Check to see if exists 
            if (foundUser == null)
            {
                return false;
            }

            //Find Category
            var foundCat = _eposDbContext.Category.SingleOrDefault(x => x.Id == catId);

            //Check to see if exists
            if (foundCat == null)
            {
                return false;
            }

            foundCat.Name = updateCatInfo.Name;
            foundCat.parentId = updateCatInfo.parentId;

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

        //SOft delete category
        [HttpPut("SoftDelCategory")]
        public bool SoftDelCategory(string sysAccountToken, int catId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            var foundCat = _eposDbContext.Category.SingleOrDefault(x => x.Id == catId);

            if (foundCat == null)
            {
                return false;
            }

            foundCat.archived = 1;

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
