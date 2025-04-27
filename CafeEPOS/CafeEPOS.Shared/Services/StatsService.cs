using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class StatsService()
    {

        //private string baseApiUrl = "http://localhost:5123/";
        private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task<statsReturnModel> GetStats(string sysAccountToken)
        {
            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"{baseApiUrl}Stats/GetStats?{param}";

            //Add data for api 
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<statsReturnModel>();

            }
            else
            {
                throw new Exception();
            }
        }
    }
}
