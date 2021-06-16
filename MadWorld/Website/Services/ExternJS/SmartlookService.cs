using System;
using Microsoft.JSInterop;
using Website.Services.Interfaces;

namespace Website.Services.ExternJS
{
    public class SmartlookService : ISmartlookService
    {
        private readonly IJSRuntime _JSRuntime;

        public SmartlookService(IJSRuntime jsRuntime)
        {
            _JSRuntime = jsRuntime;
        }

        public void Init()
        {
            _JSRuntime.InvokeVoidAsync("SmartLookAnalyser.Init");
        }
    }
}
