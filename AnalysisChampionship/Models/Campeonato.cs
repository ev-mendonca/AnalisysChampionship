using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class Campeonatos
    {
        public Campeonatos(List<Campeonato> campeonatos)
        {
            CampeonatoList = campeonatos;
        }
        public List<Campeonato> CampeonatoList { get; set; }
    }
    public class Campeonato
    {
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string Pais { get; set; }
        public List<Time> Participantes { get; set; } = new List<Time>();
        public List<Partida> Partidas { get; set; } = new List<Partida>();
    }
}