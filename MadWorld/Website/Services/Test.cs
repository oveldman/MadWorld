using System;
using Microsoft.JSInterop;

namespace Website.Services
{
    public class Test : ITest
    {
        private readonly IJSRuntime _JSRuntime;

        public Test(IJSRuntime jsRuntime)
        {
            _JSRuntime = jsRuntime;
        }

        public void Add(int x, int y)
        {
            _JSRuntime.InvokeVoidAsync("Test.Add", x, y);
        }
    }
}
