using CafeEposAPI.Data;
using CafeEposAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;

namespace CafeEposAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly ILogger<StatsController> _logger;
        private readonly EposDbContext _eposDbContext;
        public StatsController(ILogger<StatsController> logger, EposDbContext eposDbContext)
        {
            _logger = logger;
            _eposDbContext = eposDbContext;
        }

        //Method to get stats info
        [HttpGet("GetStats")]
        public StatsReturnModel GetStats(string sysAccountToken)
        {
            var foundUser = _eposDbContext.SystemAccounts.SingleOrDefault(x => x.Token == sysAccountToken);
            var currentDate = DateTime.Now;

            if (foundUser == null)
            {
                return new StatsReturnModel() { Date = currentDate };
            }


            var SalesFigure = _eposDbContext.OrderInfo
                .Include(x => x.items)
                .Where(x => x.sysAccountId == foundUser.Id && x.date.Date == currentDate.Date)
                .GroupBy(x => x.date.Date)
                .Select(x => new
                {
                    date = x.Key,
                    total = x.Sum(y => y.total),
                    count = x.Count(),
                    preparingCount = x.Count(y => y.status == "Preparing")
                }).ToList();

            if (!SalesFigure.Any())
            {
                return new StatsReturnModel() { Date = currentDate };
            }

            //Raw SQL to calc avergae completion time cus linq being funny
            var avgPrepTime = _eposDbContext.Database
                .SqlQueryRaw<int>($"Select avg(datediff(MINUTE, placed, updated)) as Value  from orderitems where convert(date, placed)='{currentDate:yyyy-MM-dd}'")
                .SingleOrDefault();

    var data = new StatsReturnModel
    {
        Sales = SalesFigure[0].total,
        TotalOrders = SalesFigure[0].count,
        OrdersPreparing = SalesFigure[0].preparingCount,
        AvgPrepTime = avgPrepTime,
        Date = SalesFigure[0].date
    };

            return data;


        }

    }
}
