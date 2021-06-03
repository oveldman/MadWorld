using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Shared.Models.BackendInfo;

namespace Website.Services.BackofficeInfo
{
    public class LoggingService : AuthenticatedBaseService, ILoggingService
    {
        public LoggingService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }

        public async Task<LoggingResponse> GetLogging(DateTime? startDate, DateTime? endDate)
        {
            LoggingRequest loggingRequest = new LoggingRequest
            {
                StartTime = startDate,
                EndTime = endDate
            };

            return await SendPostRequest<LoggingResponse, LoggingRequest>("logging/GetAll", loggingRequest);
        }
    }
}
