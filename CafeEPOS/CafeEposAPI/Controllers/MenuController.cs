using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly EposDbContext _eposDbContext;
        public MenuController(ILogger<MenuController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        //Method to get all menu items or only category menu items
        [HttpGet("GetMenu")]
        public IEnumerable<ReturnMenuItemModel> GetMenu(string sysAccountToken, int? catId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new List<ReturnMenuItemModel>();
            }

            //if cat id is null return all menu items
            if (catId == null)
            {
                var allMenu = _eposDbContext.Menu.Where(x => x.sysAccountId == foundUser.Id && x.archived == 0)
                    .Select(x => new ReturnMenuItemModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        categoryId = x.categortyId,
                        categoryName = x.category.Name,
                        price = x.price,
                        archived = x.archived
                    });
                return allMenu.ToList();
            }
            //otherwise return all items for the given category
            else
            {
                var catMenuItems = _eposDbContext.Menu.Where(x => x.sysAccountId == foundUser.Id && x.categortyId == catId && x.archived == 0)
                    .Select(x => new ReturnMenuItemModel
                    {
                        Id = x.Id,
                        Name= x.Name,
                        categoryId= x.categortyId,
                        categoryName= x.category.Name,
                        price = x.price,
                        archived = x.archived
                    });
                return catMenuItems.ToList();
            }
        }

        //ADd menu items
        [HttpPost("AddMenuItem")]
        public bool AddMenuItem(string sysAccountToken, CreateMenuItemModel newMenuModel)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            //Create new menu item 
            var newMenuItem = new menuEntity
            {
                Name = newMenuModel.Name,
                categortyId = newMenuModel.CatagoryId,
                price = newMenuModel.Price,
                sysAccountId = foundUser.Id
            };

            _eposDbContext.Add(newMenuItem);

            //Try and save new item to db
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

        //Update Menu Item
        [HttpPut("UpdateMenuItem")]
        public bool UpdateMenuItem(string sysAccountToken, int itemId, CreateMenuItemModel updateMenuItemInfo)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            var foundMenuItem = _eposDbContext.Menu.SingleOrDefault(x => x.Id == itemId);

            if (foundMenuItem == null)
            {
                return false;
            }

            //Update with new info
            foundMenuItem.Name = updateMenuItemInfo.Name;
            foundMenuItem.categortyId = updateMenuItemInfo.CatagoryId;
            foundMenuItem.price = updateMenuItemInfo.Price;

            //Try and save changes
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

        //Soft del on menu item
        [HttpPut("SoftDelMenuItem")]
        public bool SoftDelMenuItem(string sysAccountToken, int itemId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            var foundItem = _eposDbContext.Menu.SingleOrDefault( x => x.Id == itemId);

            if (foundItem == null)
            {
                return false;
            }

            foundItem.archived = 1;

            return false;
        }
    }
}
