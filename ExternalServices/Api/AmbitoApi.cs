using Domain.Model.Interfaces;
using Infra.ExternalServices.Api.Models;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Api
{
    public class AmbitoApi : IAmbitoApi
    {
        private readonly IDolarService _dolarService;
        private readonly HttpClient _httpClient;

        public AmbitoApi(IDolarService dolarService, HttpClient httpclient)
        {

            _dolarService = dolarService;
            _httpClient = httpclient;
        }

        public async Task<OperationResult<string>> ObtenerHistoricoCCL(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                string url = $"https://mercados.ambito.com//dolarrava/cl/historico-general/{fechaInicio.Day}-{fechaInicio.Month}-{fechaInicio.Year}/{fechaFinal.Day}-{fechaFinal.Month}-{fechaFinal.Year}";

                var response = await _httpClient.GetStringAsync(url);
                return OperationResult<string>.Ok(response);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail("Error en AmbitoHistorico " + ex.Message);
            }

        }

        public async Task<OperationResult<string>> obtenerCCLAmbito()
        {
            try
            {
                //if (Dolar.DolarCCL is null)
                //{
                string url = "https://mercados.ambito.com//dolarrava/cl/variacion";
                var response = await _httpClient.GetStringAsync(url);
                return OperationResult<string>.Ok(response);
                //    var dolarCCL = JsonConvert.DeserializeObject<DolarCCLAmbitoModel>(response);
                //    Dolar.DolarCCL = dolarCCL.venta.ToString();
                //    return Dolar.DolarCCL;
                //}
                //else
                //{
                //    return Dolar.DolarCCL;
                //}


            }
            catch (HttpRequestException ex)
            {
                return OperationResult<string>.Fail("Error en la petición Api-Dolar-Argentina " + ex.Message);
            }
        }

        public async Task ActualizarCotizacionAsync()
        {
            string url = "https://mercados.ambito.com//dolarrava/cl/variacion";
            var response = await _httpClient.GetStringAsync(url);
            var rta = JsonConvert.DeserializeObject<DolarCCLAmbitoModel>(response);
            var dolarCCL = rta.venta.ToString();
            if (dolarCCL != null)
            {
                _dolarService.DolarCCL = dolarCCL;
            }
        }
    }
}
