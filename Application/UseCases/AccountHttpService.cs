using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        public async Task<AccountResponse> CreateAccount(string token, AccountCreateRequest request)
        {
            var content = JsonContent.Create(request);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7214/api/Account");

            // Agrega el header y el contenido al mensaje
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = content;

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AccountResponse>();
            }

            if (((int)response.StatusCode) == StatusCodes.Status401Unauthorized)
            {
                throw new Conflict("No autorizado.");
            }

            return null; // Manejar errores según sea necesario
        }
    }
}
