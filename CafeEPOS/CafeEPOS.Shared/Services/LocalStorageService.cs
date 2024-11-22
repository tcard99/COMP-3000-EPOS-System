using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime JS;

        public LocalStorageService(IJSRuntime _JS)
        {
            JS = _JS;
        }

        public async Task StoreValue(string key, string value)
        {
            await JS.InvokeVoidAsync("storeValue", key, value);
        }

        public async Task GetValue(string key)
        {
            await JS.InvokeAsync<String>("getValue", key);
        }

        public async void RemoveValue(string key)
        {
            await JS.InvokeVoidAsync("removeValue", key);
        }
    }
}
