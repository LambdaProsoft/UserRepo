using Application.Interfaces;
using Application.Request;
using Application.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class AccountHttpService:IAcountHttpService
    {
        private readonly HttpClient _httpClient;
        public AccountHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AccountResponse> CreateAccount(AccountCreateRequest request)
        {
            var content = JsonContent.Create(request);

            var response = await _httpClient.PostAsync($"https://localhost:7214/api/Account",content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AccountResponse>();
            }
            return null;
        }
    }
}
