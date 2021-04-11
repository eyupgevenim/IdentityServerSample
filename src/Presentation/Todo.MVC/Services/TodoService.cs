namespace Todo.MVC.Services
{
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Todo.MVC.Models;

    public class TodoService : ITodoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;


        public TodoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var todoUrl = configuration.GetValue<string>("TodoUrl");
            _remoteServiceBaseUrl = $"{todoUrl}/api/v1/todo";
        }

        async public Task<List<TodoViewModel>> GetTodos()
        {
            var uri = GetAllTodos(_remoteServiceBaseUrl);
            var responseString = await _httpClient.GetStringAsync(uri);
            var response = JsonConvert.DeserializeObject<List<TodoViewModel>>(responseString);

            return response;
        }

        async public Task<TodoViewModel> GetTodo(string todoId)
        {
            var uri = GetTodo(_remoteServiceBaseUrl, todoId);
            var responseString = await _httpClient.GetStringAsync(uri);
            var response = JsonConvert.DeserializeObject<TodoViewModel>(responseString);

            return response;
        }

        #region Helpers

        static string GetAllTodos(string baseUri) => baseUri;
        static string GetTodo(string baseUri, string todoId) => $"{baseUri}/{todoId}";

        #endregion
    }
}
