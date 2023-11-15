using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class AnaliseTimeDetalhe
    {
        #region Totais
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }
        public int Empates { get; set; }
        public int VitoriaEmpate => Vitorias + Empates;
        public int DerrotaEmpate => Derrotas + Empates;
        public int VitoriaOuDerrota => Vitorias + Derrotas;
        public int Partidas => Vitorias + Empates + Derrotas;
        public int Pontos => (Vitorias * 3) + Empates;
        public int TotalOver15 { get; set; }
        public int TotalUnder15 { get; set; }
        public int TotalOver25 { get; set; }
        public int TotalUnder25 { get; set; }
        public int TotalUnder35 { get; set; }
        public int TotalOver35 { get; set; }
        public int TotalAmbas { get; set; }
        public int TotalAmbasNao { get; set; }
        #endregion

        #region Percentuais
        public decimal PercentualVitoria => Partidas == 0 ? 0 : Convert.ToInt32((decimal)Vitorias / (decimal)Partidas * 100);
        public decimal PercentualEmpate => Partidas == 0 ? 0 : Convert.ToInt32((decimal)Empates / (decimal)Partidas * 100);
        public decimal PercentualDerrota => Partidas == 0 ? 0 : Convert.ToInt32((decimal)Derrotas / (decimal)Partidas * 100);
        public decimal PercentualOver15 => Partidas == 0 ? 0 : Convert.ToInt32((decimal)TotalOver15 / (decimal)Partidas * 100);
        public decimal PercentualOver25 => Partidas == 0 ? 0 : Convert.ToInt32((decimal)TotalOver25 / (decimal)Partidas * 100);
        public decimal PercentualUnder25 => Partidas == 0 ? 0 : Convert.ToInt32((decimal)TotalUnder25 / (decimal)Partidas * 100);
        public decimal PercentualUnder35 => Partidas == 0 ? 0 : Convert.ToInt32((decimal)TotalUnder35 / (decimal)Partidas * 100);

        public decimal PercentualAmbas => Partidas == 0 ? 0 : Convert.ToInt32((decimal)TotalAmbas / (decimal)Partidas * 100);
        #endregion
    }
}