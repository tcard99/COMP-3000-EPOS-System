using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Reflection.Metadata.Ecma335;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderInfoController : ControllerBase
    {
        private readonly ILogger<OrderInfoController> _logger;
        private readonly EposDbContext _eposDbContext;
        public OrderInfoController(ILogger<OrderInfoController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        //Method ot get order info
        [HttpGet("GetOrders")]
        public IEnumerable<OrderInfoEntity> GetOrders(string sysAccountToken, int? orderId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new List<OrderInfoEntity>();
            }

            if (orderId == null)
            {
                var allOrders = _eposDbContext.OrderInfo.Where(x => x.sysAccountId == foundUser.Id);

                return allOrders.ToList();
            }
            else
            {
                var foundOrder = _eposDbContext.OrderInfo.Where(x => x.sysAccountId == foundUser.Id && x.Id == orderId);

                if (foundOrder == null)
                {
                    return new List<OrderInfoEntity>();
                }
                else
                {
                    return foundOrder.ToList();
                }
            }
        }

        //Method to create new order with or without items
        [HttpPost("MakeOrder")]
        public bool MakeOrder(string sysAccountToken, MakeOrderModel orderModel)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if ( foundUser == null)
            {
                return false;
            }

            //Fill in info for the orderInfo table 
            var data = new OrderInfoEntity
            {
                sysAccountId = foundUser.Id,
                waiterName = orderModel.WaiterName,
                table = orderModel.Table,
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                status = "Not Sent",
                total = "0.00"
            };

            return false;
        }
    }
}
