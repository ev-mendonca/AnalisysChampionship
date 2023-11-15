using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class AnaliseTimeNBA
    {
        public AnaliseTimeNBA(TimeNBA time)
        {
            ID = time.ID;
            Nome = time.Nome;
            Global = new AnaliseTimeNBADetalhe();
            ComMando = new AnaliseTimeNBADetalhe();
            Ultimos5ComMando = new AnaliseTimeNBADetalhe();
            Ultimos10 = new AnaliseTimeNBADetalhe();
        }
        public AnaliseTimeNBA()
        {
            ID = 0;
            Nome = string.Empty;
            Global = new AnaliseTimeNBADetalhe();
            ComMando = new AnaliseTimeNBADetalhe();
            Ultimos5ComMando = new AnaliseTimeNBADetalhe();
            Ultimos10 = new AnaliseTimeNBADetalhe();
        }
        public int ID { get; set; }
        public string Nome { get; set; }
        public AnaliseTimeNBADetalhe Global { get; set; }
        public AnaliseTimeNBADetalhe ComMando { get; set; }
        public AnaliseTimeNBADetalhe Ultimos5ComMando { get; set; }
        public AnaliseTimeNBADetalhe Ultimos10 { get; set; }
    }
}