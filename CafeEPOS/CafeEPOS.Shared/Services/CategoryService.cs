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

        //method which calls api to get all categoires for system user
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

        //Method to make api call to get all parent categoies
        public async Task<List<CategoryModel>> GetAllParentCategories(string sysAccountToken)
        {
            //Set up Paramater
            var param = $"sysAccountToken={sysAccountToken}";

            //Set Up URL
            var url = $"{baseApiUrl}Category/getParentCategoires?{param}";

            //Send request
            var response = await _httpClient.GetAsync(url);

            //Check to see if successful
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

        //Method to get a single category
        public async Task<List<CategoryModel>> GetSingleCategory(string sysAccountToken, int catId)
        {
            //Set up param
            var param = $"sysAccountToken={sysAccountToken}&catId={catId}";

            //Set up URL
            var url = $"{baseApiUrl}Category/getSingleCategory?{param}";

            //Send request
            var response = await _httpClient.GetAsync(url);

            //Check if successful
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

        //Method to call api to add new category
        public async Task<bool> AddNewCategory(string sysAccountToken, int? parentId, string catName)
        {
            //Set up paramter
            var param = $"sysAccountToken={sysAccountToken}";

            //Set up URL
            var url = $"{baseApiUrl}Category/addNewCategory?{param}";

            //Add data for api to recive
            var data = new
            {
                name = catName,
                parentId = parentId == 0 ? null : parentId,
            };

            //Send request
            var response = await _httpClient.PostAsJsonAsync(url, data);

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        //Method to call api to call put method to update category

    }
}
