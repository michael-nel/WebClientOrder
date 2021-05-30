using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebClientOrder.Domain.Services;
using WebClientOrder.Infra.Services;
using Xunit;

namespace WebClientOrder.Test.Integration.Service
{
    public class CepServiceTest
    {
        private readonly ICepService _cepService;
        public CepServiceTest()
        {
            var appSettings = new Dictionary<string, string> {
                {"Cep:Api", "https://viacep.com.br/ws"},
            };

            var appConfiguration = new ConfigurationBuilder().AddInMemoryCollection(appSettings).Build();

            _cepService = new CepService(appConfiguration);
        }


        [Fact]

        public async Task ShouldBe_Get_Address_With_Cep()
        {
            var address = await _cepService.Execute("23575460");

            Assert.Equal("23575-460", address.ZipCode);
            Assert.Equal("Rua Cristo Redentor", address.Street);
            Assert.Equal("Paciência", address.District);
            Assert.Equal("Rio de Janeiro", address.City);
            Assert.Equal("RJ", address.State);
        }

    }
}
