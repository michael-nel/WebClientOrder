using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebClientOrder.Domain.Services;
using WebClientOrder.Domain.ValueOfObjects;
using WebClientOrder.Domain.Dto;
using Newtonsoft.Json;

namespace WebClientOrder.Infra.Services
{
    public class CepService : ICepService
    {
        private readonly IConfiguration _configuration;
        public CepService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Address> Execute(string zipCode)
        {

            string url = $"{_configuration["Cep:Api"]}/{zipCode}/json";

            using(var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.GetAsync(url);

                var response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var responseJson = JsonConvert.DeserializeObject<ViaCepResponse>(response);
                        return ProcessAddress(responseJson);
                    default:
                        throw new Exception("ZipCode not found or invalid! ");
                }
            }
        }
        private Address ProcessAddress(ViaCepResponse response)
        {
            return new Address
            {
                Street = response.Street,
                District = response.District,
                ZipCode = response.ZipCode,
                City = response.City,
                State = response.State,
            };
        }
    }
}
