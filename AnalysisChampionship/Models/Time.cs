using System.Collections.Generic;

namespace AnalysisChampionship.Models
{
    public class Times
    {
        public Times(List<Time> times)
        {
            TimeList = times;
        }
        public List<Time> TimeList { get; set; } = new List<Time>();
    }
    public class Time
    {
        public Time()
        {}
        public Time(string nome, string pais)
        {
            Nome = nome;
            Pais = pais;
        }
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}