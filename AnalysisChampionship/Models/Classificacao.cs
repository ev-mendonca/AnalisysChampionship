using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class Classificacao
    {
        public string Competicao { get; set; }
        public List<ClassificacaoTime> Times { get; set; }

    }

    public class ClassificacaoTime
    {
        public string Nome { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int Partidas => Vitorias + Empates + Derrotas;
        public int Pontuacao => (Vitorias * 3) + Empates;
    }

}