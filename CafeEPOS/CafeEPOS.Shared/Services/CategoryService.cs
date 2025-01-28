using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class CategoryService
    {
        private string baseApiUrl = "http://localhost:5123/";
        //private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<CategoryModel>> GetAllCategoiesCall(string sysAccountToken)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up request url
            var url = $"{baseApiUrl}Category/getAllCategoies?{param}";

            //Send request to API
            var response = await _httpClient.GetAsync(url);

            //Chcek to see if successful
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
                return data;
            }
            else
            {
                return new List<CategoryModel>();
            }
        }
    }
}
