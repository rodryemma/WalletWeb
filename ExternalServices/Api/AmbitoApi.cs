using Domain.Model.Interfaces;
using Infra.ExternalServices.Api.Models;
//using Newtonsoft.Json;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public async Task<OperationResult<decimal>> ObtenerHistoricoCCL(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                //string url = $"https://mercados.ambito.com//dolarrava/cl/historico-general/{fechaInicio.Day}-{fechaInicio.Month}-{fechaInicio.Year}/{fechaFinal.Day}-{fechaFinal.Month}-{fechaFinal.Year}";
                string url = $"https://mercados.ambito.com//dolarrava/cl/grafico/{fechaInicio.Year}-{fechaInicio.Month}-{fechaInicio.Day}/{fechaFinal.Year}-{fechaFinal.Month}-{fechaFinal.Day}";

                var response = await _httpClient.GetStringAsync(url);
                var rawData = JsonSerializer.Deserialize<List<List<JsonElement>>>(response);

                var datos = rawData.Skip(1) // saltar cabecera
                            .Select(row => new AmbitoCCLGraficoModel
                            {
                                Fecha = DateTime.ParseExact(row[0].GetString(), "dd/MM/yyyy", null),
                                Valor = row[1].GetDecimal()
                            })
                            .Where(x => x.Valor != 0)
                            .OrderByDescending(x => x.Fecha)
                            .ToList();
                var ultimoDatoCcl = datos.FirstOrDefault().Valor;

                return OperationResult<decimal>.Ok(ultimoDatoCcl);
            }
            catch (Exception ex)
            {
                return OperationResult<decimal>.Fail("Error en AmbitoHistorico " + ex.Message);
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
            var rta = JsonSerializer.Deserialize<DolarCCLAmbitoModel>(response);
            var dolarCCL = rta.venta.ToString();
            if (dolarCCL != null)
            {
                _dolarService.DolarCCL = dolarCCL;
            }
        }
    }
}
