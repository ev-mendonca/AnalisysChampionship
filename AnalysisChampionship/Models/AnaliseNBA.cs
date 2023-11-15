using System.Collections.Generic;

namespace AnalysisChampionship.Models
{
    public class AnaliseNBA
    {
        public AnaliseNBA(AnaliseTimeNBA mandante, AnaliseTimeNBA visitante)
        {
            Mandante = mandante;
            Visitante = visitante;
        }
        public AnaliseTimeNBA Mandante { get; set; }
        public AnaliseTimeNBA Visitante { get; set; }
        public List<Jogador> Jogadores { get; set; }

        public decimal MediasPontos(AnaliseTimeNBADetalhe time1, AnaliseTimeNBADetalhe time2) 
            => (time1.MediaPontos + time2.MediaPontosSofridos) / 2;

        public decimal MediaFinalPontosMandante()
            => (MediasPontos(Mandante.Global, Visitante.Global)
              + MediasPontos(Mandante.ComMando, Visitante.ComMando)
              + MediasPontos(Mandante.Ultimos10, Visitante.Ultimos10)
              + MediasPontos(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando)) / 4;
        public decimal MediaFinalPontosVisitante()
            => (MediasPontos(Visitante.Global, Mandante.Global)
              + MediasPontos(Visitante.ComMando, Mandante.ComMando)
              + MediasPontos(Visitante.Ultimos10, Mandante.Ultimos10)
              + MediasPontos(Visitante.Ultimos5ComMando, Mandante.Ultimos5ComMando)) / 4;
        
        public decimal MediasRebotes(AnaliseTimeNBADetalhe time1, AnaliseTimeNBADetalhe time2)
            => (time1.MediaRebotes + time2.MediaRebotesContra) / 2;

        public decimal MediaFinalRebotesMandante()
            => (MediasRebotes(Mandante.Global, Visitante.Global)
              + MediasRebotes(Mandante.ComMando, Visitante.ComMando)
              + MediasRebotes(Mandante.Ultimos10, Visitante.Ultimos10)
              + MediasRebotes(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando)) / 4;
        public decimal MediaFinalRebotesVisitante()
            => (MediasRebotes(Visitante.Global, Mandante.Global)
              + MediasRebotes(Visitante.ComMando, Mandante.ComMando)
              + MediasRebotes(Visitante.Ultimos10, Mandante.Ultimos10)
              + MediasRebotes(Visitante.Ultimos5ComMando, Mandante.Ultimos5ComMando)) / 4;


        public decimal MediasAssistencia(AnaliseTimeNBADetalhe time1, AnaliseTimeNBADetalhe time2)
            => (time1.MediaAssistencias + time2.MediaAssistenciasContra) / 2;

        public decimal MediaFinalAssitenciasMandante()
            => (MediasAssistencia(Mandante.Global, Visitante.Global)
              + MediasAssistencia(Mandante.ComMando, Visitante.ComMando)
              + MediasAssistencia(Mandante.Ultimos10, Visitante.Ultimos10)
              + MediasAssistencia(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando)) / 4;
        public decimal MediaFinalAssistenciasVisitante()
            => (MediasAssistencia(Visitante.Global, Mandante.Global)
              + MediasAssistencia(Visitante.ComMando, Mandante.ComMando)
              + MediasAssistencia(Visitante.Ultimos10, Mandante.Ultimos10)
              + MediasAssistencia(Visitante.Ultimos5ComMando, Mandante.Ultimos5ComMando)) / 4;
        public string GetTexto(enumTipoAnalise tipoAnalise)
        {
            string paragrafo = "";

            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    paragrafo = $"Global: {GetTexto(Mandante.Global, Visitante.Global)}";
                    break;
                case enumTipoAnalise.ComMando:
                    paragrafo = $"Com mando: {GetTexto(Mandante.ComMando, Visitante.ComMando)}";
                    break;
                case enumTipoAnalise.Ultimo10:
                    paragrafo = $"Últimos 10 jogos: {GetTexto(Mandante.Ultimos10, Visitante.Ultimos10)}";
                    break;
                case enumTipoAnalise.Ultimos5ComMando:
                    paragrafo = $"Últimos 5 jogos com mando: {GetTexto(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando)}";
                    break;
                default:
                    paragrafo = "";
                    break;
            }

            return $"<p>{paragrafo}</p>";
        }
        private string GetTexto(AnaliseTimeNBADetalhe mandante, AnaliseTimeNBADetalhe visitante)
        {
            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal mediaPontosCasa = MediasPontos(mandante, visitante);
            decimal mediaPontosFora = MediasPontos(visitante, mandante);
            decimal mediaRebotesCasa = MediasRebotes(mandante, visitante);
            decimal mediaRebotesFora = MediasRebotes(visitante, mandante);
            decimal mediaAssistenciasCasa = MediasAssistencia(mandante, visitante);
            decimal mediaAssistenciasFora = MediasAssistencia(visitante, mandante);

            return $@"A estimativa é de {mediaPontosCasa.ToString("0.0")} | {mediaRebotesCasa.ToString("0.0")} | {mediaAssistenciasCasa.ToString("0.0")} contra {mediaPontosFora.ToString("0.0")} | {mediaRebotesFora.ToString("0.0")} | {mediaAssistenciasFora.ToString("0.0")}";
        }

        
    }
}