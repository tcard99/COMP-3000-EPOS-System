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
        private string baseApiUrl = "http://localhost:5123/";
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> SystemAccountLoginCall(string email, string password)
        {
            var data = new
            {
                eamil = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync(baseApiUrl + "Auth/SystemAccountLogin", data);

            return await response.Content.ReadAsStringAsync();
        }

    }
}
