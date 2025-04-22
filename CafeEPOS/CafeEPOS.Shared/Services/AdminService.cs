using CafeEPOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CafeEPOS.Shared.Services
{
    public class AdminService
    {
        //private string baseApiUrl = "http://localhost:5123/";
        private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

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

        //Method to get single staff info
        public async Task<StaffAccountReturnModel> GetSingleStaffInfo(string sysAccountToken, int Id)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}&staffId={Id}";

            //Set up request url
            var url = $"{baseApiUrl}StaffManagment/GetSpecificUser?{param}";

            //Send request to API
            var response = await _httpClient.GetAsync(url);

            //Chcek to see if successful
            if (response.IsSuccessStatusCode)
            {

                var a = await response.Content.ReadAsStringAsync();
                var data = await response.Content.ReadFromJsonAsync<StaffAccountReturnModel>();
                return data;


            }
            else
            {
                return new StaffAccountReturnModel();
            }
        }

        //Method to add new staff member
        public async Task<bool> AddNewStaffAccount(string sysAccountToken, string Name, string staffId, string passCode, int Role)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up request url
            var url = $"{baseApiUrl}StaffManagment/MakeNewStaff?{param}";

            var data = new MakeNewStaffAccountModel
            {
                Name = Name,
                StaffId = staffId,
                Passcode = passCode,
                Role = Role
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                return false;
            }

        }

        //Method to update staff account info
        public async Task<int> UpdateStaffAccountInfo(string sysAccountToken, int Id, string Name, string staffId, string passCode, int Role)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up request url
            var url = $"{baseApiUrl}StaffManagment/UpdateStaffInfo?{param}";

            //set up data to give to api
            var data = new UpdateStaffAccountModel
            {
                Id = Id,
                Name = Name,
                StaffId = staffId,
                Passcode = passCode,
                Role = Role
            };

            //Send request to API
            var response = await _httpClient.PutAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            else
            {
                return 0;
            }
        }

        //Method to remove user#
        public async Task<bool> RemoveStaffAccount(string sysAccountToken, int Id)
        {
            //Set up paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up request url
            var url = $"{baseApiUrl}StaffManagment/DelStaffAccount?{param}&id={Id}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                return false;
            }

        }
    }
}
