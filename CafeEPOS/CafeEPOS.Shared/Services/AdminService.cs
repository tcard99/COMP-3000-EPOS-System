using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class AdminService
    {
        private string baseApiUrl = "http://localhost:5123/";
        //private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();

        //Method to retive all staff accounts from the api
        public async Task<List<StaffAccountReturnModel>> GetAllStaff(string sysAccountToken)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up request url
            var url = $"{baseApiUrl}StaffManagment/GetAlStaff?{param}";

            //Send request to API
            var response = await _httpClient.GetAsync(url);

            //Chcek to see if successful
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<StaffAccountReturnModel>>();
                return data;
            }
            else
            {
                return new List<StaffAccountReturnModel>();
            }
        }

    }
}
