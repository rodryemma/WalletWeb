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
        public List<string> Labels { get; set; } = new(); // Meses
        public List<ChartSerieDto> Series { get; set; } = new();
    }

}
