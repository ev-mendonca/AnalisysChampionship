using System.Collections.Generic;

namespace AnalysisChampionship.Models
{
    public class PartidaNBA
    {
        public int ID { get; set; }
        public int TimeCasaID { get; set; }
        public int TimeForaID { get; set; }
        public int PontosCasa { get; set; }
        public int PontosFora { get; set; }
        public int RebotesCasa { get; set; }
        public int RebotesFora { get; set; }
        public int AssistenciasCasa { get; set; }
        public int AssistenciasFora { get; set; }
        public TimeNBA TimeFora { get; set; }
        public TimeNBA TimeCasa { get; set; } 
    }
}
