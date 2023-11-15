using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class JogadorStats
    {
        public int ID { get; set; }
        public int JogadorID { get; set; }
        public int Pontos { get; set; }
        public int Rebotes { get; set; }
        public int Assistencia { get; set; }
        public int Roubo { get; set; }
        public int Toco { get; set; }
        public int Tres { get; set; }

        public Jogador Jogador { get; set; }
    }
}