using Newtonsoft.Json;
using System;

namespace WebClientOrder.Domain.Dto
{
    public class ViaCepResponse
    {
        [JsonProperty("cep")]
        public string ZipCode { get; set; }

        [JsonProperty("logradouro")]
        public string Street { get; set; }
        
        [JsonProperty("bairro")]
        public string District { get; set; }

        [JsonProperty("uf")]
        public string State { get; set; }

        [JsonProperty("localidade")]
        public string City { get; set; }
    }
}
