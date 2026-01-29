using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ChartSerieDto
    {
        public string Label { get; set; } = string.Empty;
        public List<double> Data { get; set; } = new();
    }

    public class ChartResultDto
    {
        public List<string> Labels { get; set; } = new(); 
        public List<ChartSerieDto> Series { get; set; } = new();
    }

    public class ReporteCategoriaDto
    {
        public ChartResultDto Chart { get; set; }
        public Dictionary<string, Dictionary<string, bool>> DiccionarioCategoria { get; set; }
    }

    public class ReporteFiltroCategoriaDto
    {
        public string TipoMovimiento { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public Dictionary<string, Dictionary<string, bool>> CategoriaFiltro { get; set; }
    }

}
