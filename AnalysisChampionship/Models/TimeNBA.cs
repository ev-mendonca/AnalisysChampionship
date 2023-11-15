using System.Collections.Generic;

namespace AnalysisChampionship.Models
{
    public class TimesNBA
    {
        public TimesNBA(List<TimeNBA> times)
        {
            TimeList = times;
        }
        public List<TimeNBA> TimeList { get; set; } = new List<TimeNBA>();
    }
    public class TimeNBA
    {
        public TimeNBA()
        {}
        public TimeNBA(string nome)
        {
            Nome = nome;
        }
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}