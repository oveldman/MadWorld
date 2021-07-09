using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Services.Interfaces;
using Website.Settings;
using Website.Shared.Models;

namespace Website.Services
{
    public class BlogService : IBlogService
    {
        private HttpClient _client;

        public BlogService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
        }

        public async Task<BlogsModel> GetAll(int page, int totalPosts)
        {
            return await _client.GetFromJsonAsync<BlogsModel>($"blog/getall?page={page}&totalPosts={totalPosts}");
        }
    }
}
