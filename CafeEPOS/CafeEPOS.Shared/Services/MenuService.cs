using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class MenuService
    {
        private string baseApiUrl = "http://localhost:5123/";
        //private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();

        //Method to get all menu items
        public async Task<List<MenuModel>> GetAllMenuItemsCall(string sysAccountToken)
        {
            //set up token param
            var param = $"sysAccountToken={sysAccountToken}";

            //combine url and params
            var url = $"{baseApiUrl}Menu/GetMenu?{param}";

            //Send request to api
            var response = await _httpClient.GetAsync(url);

            //See if response is successful
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<MenuModel>>();
                return data;
            }
            else
            {
                return new List<MenuModel>();
            }
        }

        //Method to add a new menu item
        public async Task<bool> AddNewMenuItem(string sysAccountToken, string itemName, int catId, string price)
        {
            //Set up param
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"{baseApiUrl}Menu/AddMenuItem?{param}";

            var data = new
            {
                name = itemName,
                catagoryId = catId,
                price = price
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
