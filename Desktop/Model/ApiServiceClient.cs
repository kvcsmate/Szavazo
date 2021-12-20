using Persistence;
using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Model
{
    public class ApiServiceClient
    {
        private readonly HttpClient _client;

        public ApiServiceClient(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }
        public async Task<bool> LoginAsync(string Email, string password)
        {
            LoginDto user = new LoginDto
            {
                Email = Email,
                Password = password
            };
            var response = await _client.PostAsJsonAsync("api/Account/Login", user);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Account/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<PollDto>> LoadPollsAsync()
        {
            var response = await _client.GetAsync("api/Polls/");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<PollDto>>();
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<AnswerDto>> LoadAnswersAsync(int id)
        {
            var response = await _client.GetAsync($"GetAnswers/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<AnswerDto>>();
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        public async Task<IEnumerable<PollBindingDto>> LoadPollBindingsAsync(int id)
        {
            var response = await _client.GetAsync($"api/Polls/GetPollBindingsByPollId/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<PollBindingDto>>();
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        public async Task CreatePollAsync(PollCreateRequest request)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync("api/Polls/CreatePoll/",request);
            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        public async Task<User> GetCurrentUser()
        {
            var response = await _client.GetAsync("/CurrentUser");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<User>();
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        public async Task<IEnumerable<User>> LoadUsersAsync()
        {
            var response = await _client.GetAsync($"api/Polls/GetUsers");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<User>>();
            }
            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}
