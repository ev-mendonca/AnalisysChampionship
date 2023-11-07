using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class AnaliseTime
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Posicao { get; set; }
        public AnaliseTimeDetalhe Global { get; set; }
        public AnaliseTimeDetalhe ComMando { get; set; }
        public AnaliseTimeDetalhe Ultimos5 { get; set; }
        public AnaliseTimeDetalhe Ultimos5ComMando { get; set; }
        public AnaliseTimeDetalhe Ultimos10 { get; set; }
        public AnaliseTimeDetalhe Ultimos10ComMando { get; set; }
        public AnaliseTimeDetalhe Similares { get; set; }
        public AnaliseTimeDetalhe SimilaresComMando { get; set; }
    }
}