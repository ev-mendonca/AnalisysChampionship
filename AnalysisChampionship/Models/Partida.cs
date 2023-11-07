using System.Collections.Generic;

namespace AnalysisChampionship.Models
{
    public class Partida
    {
        public int ID { get; set; }
        public int TimeCasaID { get; set; }
        public int TimeForaID { get; set; }
        public int CampeonatoID { get; set; }
        public int GolsCasa { get; set; }
        public int GolsFora { get; set; }
        public Time TimeFora { get; set; }
        public Time TimeCasa { get; set; } 

    }
}
