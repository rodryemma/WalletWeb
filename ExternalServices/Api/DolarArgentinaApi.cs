using Domain.Model.Interfaces;
using Infra.ExternalServices.Api.Models;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Api
{
    public class DolarArgentinaApi : IDolarArgentinaApi
    {
        private readonly HttpClient _httpClient;

        public DolarArgentinaApi(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task<OperationResult<string>> obtenerCCL()
        {
            try
            {
                string url = "https://api-dolar-argentina.herokuapp.com/api/contadoliqui";
                var response = await _httpClient.GetStringAsync(url);
                var dolarCCL = JsonConvert.DeserializeObject<DolarCCLModel>(response);
                return OperationResult<string>.Ok(dolarCCL.venta.ToString());
            }
            catch (HttpRequestException ex)
            {
                return OperationResult<string>.Fail("Error en la petición Api-Dolar-Argentina " + ex.Message);
            }
        }        

    }

}
