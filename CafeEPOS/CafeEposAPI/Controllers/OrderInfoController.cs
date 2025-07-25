﻿using CafeEposAPI.Data;
using CafeEposAPI.Data.Entity;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                var allOrders = _eposDbContext.OrderInfo.Where(x => x.sysAccountId == foundUser.Id).Include(x => x.items);

                return allOrders.ToList();
            }
            else
            {
                var foundOrder = _eposDbContext.OrderInfo.Where(x => x.sysAccountId == foundUser.Id && x.Id == orderId).Include(x => x.items);

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

        //Method to get orders that are preparing
        [HttpGet("GetOrderForKDS")]
        public IEnumerable<OrderInfoEntity> GetOrdersForKDS(string sysAccountToken)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new List<OrderInfoEntity>();
            }

            var foundOrders = _eposDbContext.OrderInfo.Where(x => x.sysAccountId == foundUser.Id && x.status == "Preparing").Include(x => x.items).OrderBy(x => x.date);

            if (foundOrders == null)
            {
                return new List<OrderInfoEntity>();
            }

            return foundOrders;
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
                updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                status = "Ordering",
                ammountPaid = 0.00m,
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
                    status = "Ordering",
                    placed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                    updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
                };

                data.total += menuItem.price;

                data.items.Add(orderIt);
            }

            //save to db 
            _eposDbContext.OrderInfo.Add(data);
            _eposDbContext.SaveChanges();

            var order = new MakeOrderReturnModel
            {
                Id = data.Id
            };

            return order;
        }

        //Method to add new items
        [HttpPost("AddNewItems")]
        public MakeOrderReturnModel AddMoreItems(string SysAccountToken, int orderId, List<OrderItemsMakeOrderModel> newItems)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == SysAccountToken);

            if (foundUser == null)
            {
                return new MakeOrderReturnModel();
            }

            //FInd passed in order
            var foundOrderInfo = _eposDbContext.OrderInfo.SingleOrDefault(x => x.Id == orderId);

            if (foundOrderInfo == null)
            {
                return new MakeOrderReturnModel();
            }

            //Chaneg status 
            foundOrderInfo.status = "Preparing";

            //Add all new items
            foreach (var item in newItems)
            {
                var menuItem = _eposDbContext.Menu.Single(x => x.Id == item.ItemId);
                var orderIt = new OrderItemsEntity
                {
                    sysAccountId = foundUser.Id,
                    Name = menuItem.Name,
                    price = menuItem.price,
                    status = "Preparing",
                    placed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                    updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
                };

                //Calculate new total 
                foundOrderInfo.total += menuItem.price;

                //Add new items to order
                foundOrderInfo.items.Add(orderIt);
            }

            var order = new MakeOrderReturnModel
            {
                Id = foundOrderInfo.Id
            };

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
                item.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            foundOrderInfo.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

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
                item.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            foundOrderInfo.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

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

        //Method to change status to Closed
        [HttpPut("ChanegStatClosed")]
        public bool ChangeStatusClosed(string sysAccountToken, int orderId)
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
            foundOrderInfo.status = "Closed";

            //For each item change status to paid
            foreach (var item in foundOrderInfo.items.Where(x => x.status == "Prepared"))
            {
                item.status = "Closed";
                item.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            foundOrderInfo.updated = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

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

        //Method to update paid ammount
        [HttpPut("UpdatePaidAmmount")]
        public UpdatePayAmountReturnModel UpdatePaidAmmount(string sysAccountToken, UpdateAmmountPaidInfoModel model)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);

            if (foundUser == null)
            {
                return new();
            }

            var foundOrderInfo = _eposDbContext.OrderInfo.SingleOrDefault(x => x.Id == model.OrderId);

            if (foundOrderInfo == null)
            {
                return new();
            }

            //Update the ammount paid if less than total
            foundOrderInfo.ammountPaid += model.AmmountPaid;

            if (foundOrderInfo.ammountPaid >= foundOrderInfo.total)
            {
                //If ammount is paid change the state of the order
                ChangeStatusToPreparing(sysAccountToken, model.OrderId);
            }

            try
            {
                _eposDbContext.SaveChanges();

                //Return ammount due and total
                var data = new UpdatePayAmountReturnModel
                {
                    Id = foundOrderInfo.Id,
                    AmountDue = foundOrderInfo.total - foundOrderInfo.ammountPaid,
                    Total = foundOrderInfo.total
                };

                return data;
            }
            catch (Exception ex)
            {
                return new();
            }

        }
    }
}
