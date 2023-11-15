namespace AnalysisChampionship.Models
{
    public class AnaliseTimeNBADetalhe
    {
        #region Totais
        public int Partidas { get; set; }
        public int PontosFeitos { get; set; }
        public int PontosSofridos { get; set; }
        public int RebotesFavor { get; set; }
        public int RebotesContra { get; set; }
        public int AssistenciasFavor { get; set; }
        public int AssistenciasContra { get; set; }
        #endregion

        #region Percentuais
        public decimal MediaPontos => (decimal)PontosFeitos / (decimal)Partidas;
        public decimal MediaPontosSofridos => (decimal)PontosSofridos / (decimal)Partidas;
        public decimal MediaRebotes => (decimal)RebotesFavor / (decimal)Partidas;
        public decimal MediaRebotesContra => (decimal)RebotesContra / (decimal)Partidas;
        public decimal MediaAssistencias => (decimal)AssistenciasFavor / (decimal)Partidas;
        public decimal MediaAssistenciasContra => (decimal)AssistenciasContra / (decimal)Partidas;
        #endregion
    }
}