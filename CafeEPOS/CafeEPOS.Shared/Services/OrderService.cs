using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class OrderService(IHttpClientFactory clientFactory)
    {
        private string baseApiUrl = "http://localhost:5123/";
        //private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();

        //Method to send request to server to add a new order
        public async Task<MakeOrderReturnModel?> MakeNewOrder(string sysAccountToken, MakeOrderModel model)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"OrderInfo/MakeOrder?{param}";

            //Add data for api 
            var response = await httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {

            return await response.Content.ReadFromJsonAsync<MakeOrderReturnModel>();

            }
            else
            {
                throw new Exception();
            }

        }

        public async Task<UpdateAmountPaidReturnModel> UpdateAmmountPaid(string sysAccountToken, int orderId, decimal ammount)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"OrderInfo/UpdatePaidAmmount?{param}";

            //set up the data
            var data = new UpdateAmmountPaidInfoModel
            {
                AmmountPaid = ammount,
                OrderId = orderId
            };

            var response = await httpClient.PutAsJsonAsync(url,data);

            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<UpdateAmountPaidReturnModel>();

            }
            else
            {
                throw new Exception();
            }
        }

        //Get specific order info 
        public async Task<OrderInfoReturnModel> GetSpecificOrderInfo(string sysAccountToken, int orderId)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}&orderId={orderId}";

            //Set up URL
            var url = $"OrderInfo/GetOrders?{param}";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var newResponse = await response.Content.ReadFromJsonAsync<List<OrderInfoReturnModel>>();
                return newResponse.SingleOrDefault();
            }
            else
            {
                throw new Exception();
            }
        }

        //Method to get all preparing orders
        public async Task<List<OrderInfoReturnModel>> GetPreparingOrders(string sysAccountToken)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"OrderInfo/GetOrderForKDS?{param}";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var newResponse = await response.Content.ReadFromJsonAsync<List<OrderInfoReturnModel>>();
                return newResponse;
            }
            else
            {
                throw new Exception();
            }
        }

        //Method to get all orders
        public async Task<List<OrderInfoReturnModel>> GetAllPastOrders(string sysAccountToken)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"OrderInfo/GetOrders?{param}";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var newResponse = await response.Content.ReadFromJsonAsync<List<OrderInfoReturnModel>>();
                return newResponse.Where(x => x.status == "Prepared").ToList();
            }
            else
            {
                throw new Exception();
            }
        }

        //MEthod to change state to prepared
        public async Task<bool> ChangeStateToPrepared(string sysAccountToken, int orderId)
        {
            using var httpClient = clientFactory.CreateClient("api");

            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}&orderId={orderId}";

            //Set up URL
            var url = $"OrderInfo/ChangeStatPrepared?{param}";

            var response = await httpClient.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var newResponse = await response.Content.ReadFromJsonAsync<bool>();
                return newResponse;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
