using CafeEPOS.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class AuthenticationService
    {
        //private string baseApiUrl = "http://localhost:5123/";
        private string baseApiUrl = "https://web.socem.plymouth.ac.uk/comp3000/tcard-api/";

        private static readonly HttpClient _httpClient = new HttpClient();

        //Method to call systemAccountLogin method on API
        public async Task<string> SystemAccountLoginCall(string email, string password)
        {
            var data = new
            {
                eamil = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync(baseApiUrl + "Auth/SystemAccountLogin", data);

            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "";
            }
        }

        //Method to call System Registraion method on API
        public async Task<string> SystemAccountRegistrationCall(string name, string email, string password)
        {
            var data = new
            {
                name = name,
                email = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync(baseApiUrl + "SystemAccount/CreateNewSystemUser", data);

            return await response.Content.ReadAsStringAsync();
        }

        //Method to call the staff login method on API
        public async Task<List<StaffAuthLoginReturnModel>> StaffLogin(string token, string userId, string passcode)
        {
            var data = new
            {
                token = token,
                staffId = userId,
                passcode = passcode
            };

            var response = await _httpClient.PostAsJsonAsync(baseApiUrl + "Auth/StaffLogin", data);

            return await response.Content.ReadFromJsonAsync<List<StaffAuthLoginReturnModel>>();
        }

    }
}
