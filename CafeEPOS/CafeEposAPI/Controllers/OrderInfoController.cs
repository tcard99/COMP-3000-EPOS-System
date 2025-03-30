using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualBasic;
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
        public MakeOrderReturnModel MakeOrder(string sysAccountToken, MakeOrderModel orderModel)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new MakeOrderReturnModel();
            }

            //Fill in info for the orderInfo table 
            var data = new OrderInfoEntity
            {
                sysAccountId = foundUser.Id,
                waiterName = orderModel.WaiterName,
                table = orderModel.Table,
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                status = "Ordering",
                total = 0.00m
            };

            //Go through all the items passed in 
            foreach (var item in orderModel.Items)
            {
                //Find item fill in info 
                var menuItem = _eposDbContext.Menu.Single(x => x.Id == item.ItemId);
                var orderIt = new OrderItemsEntity
                {
                    sysAccountId = foundUser.Id,
                    Name = menuItem.Name,
                    price = menuItem.price,
                    status = "Ordering"
                };

                data.total += menuItem.price;

                data.items.Add(orderIt);
            }


            var order = new MakeOrderReturnModel
            {
                Id = data.Id
            };

            //save to db 
            _eposDbContext.OrderInfo.Add(data);
            _eposDbContext.SaveChanges();

            return order;
        }

        //Mehtod to change status to Preparing
        [HttpPut("ChangeStatPreparing")]
        public bool ChangeStatusToPreparing(string sysAccountToken, int orderId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            //Find passed in order and its items 
            var foundOrderInfo = _eposDbContext.OrderInfo.Include(x => x.items).SingleOrDefault(x => x.Id == orderId);



            if (foundOrderInfo == null)
            {
                return false;
            }

            //Change the status to Preparing
            foundOrderInfo.status = "Preparing";

            //Go and change status of all items that have the ordering status
            foreach (var item in foundOrderInfo.items.Where(x => x.status == "Ordering"))
            {
                item.status = "Preparing";
            }

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

        //Method to change status to Prepared
        [HttpPut("ChangeStatPrepared")]
        public bool ChangeStatusPrepared(string sysAccountToken, int orderId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            //Find order with passed in id and its items
            var foundOrderInfo = _eposDbContext.OrderInfo.Include(x => x.items).SingleOrDefault(x => x.Id == orderId);

            if (foundOrderInfo == null)
            {
                return false;
            }

            //Change status to Prepared 
            foundOrderInfo.status = "Prepared";

            foreach (var item in foundOrderInfo.items.Where(x => x.status == "Preparing"))
            {
                //Chaneg item status to Prepared
                item.status = "Prepared";
            }

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

        //Method to change status to paid
        [HttpPut("ChanegStatPaid")]
        public bool ChangeStatusPaid(string sysAccountToken, int orderId)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return false;
            }

            //Fidn passed in order and its items
            var foundOrderInfo = _eposDbContext.OrderInfo.Include(x => x.items).SingleOrDefault(x => x.Id == orderId);

            if (foundOrderInfo == null)
            {
                return false;
            }

            //Chaneg the status to paid
            foundOrderInfo.status = "Paid";

            //For each item change status to paid
            foreach (var item in foundOrderInfo.items.Where(x => x.status == "Prepared"))
            {
                item.status = "Paid";
            }

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
