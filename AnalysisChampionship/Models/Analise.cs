using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class Analise
    {
        public Analise(AnaliseTime mandante, AnaliseTime visitante)
        {
            Mandante = mandante;
            Visitante = visitante;
        }
        public AnaliseTime Mandante { get; set; }
        public AnaliseTime Visitante { get; set; }
        
        public string GetTextoResultadoExato(enumTipoAnalise tipoAnalise)
        {
            bool isGreen = false;
            string paragrafo = "";
            
            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    paragrafo = $"Global: {GetTextoResultadoExato(Mandante.Global, Visitante.Global, out isGreen)}";
                    break;
                case enumTipoAnalise.ComMando:
                    paragrafo = $"Com mando: {GetTextoResultadoExato(Mandante.ComMando, Visitante.ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimo10:
                    paragrafo = $"Últimos 10 jogos: {GetTextoResultadoExato(Mandante.Ultimos10, Visitante.Ultimos10, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimos5ComMando:
                    paragrafo = $"Últimos 5 jogos com mando: {GetTextoResultadoExato(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Similares:
                    paragrafo = $"Similares: {GetTextoResultadoExato(Mandante.Similares, Visitante.Similares, out isGreen)}";
                    break;
                case enumTipoAnalise.SimilaresComMando:
                    paragrafo = $"Similares com mando: {GetTextoResultadoExato(Mandante.SimilaresComMando, Visitante.SimilaresComMando, out isGreen)}";
                    break;
                default:
                    paragrafo = "";
                    break;
            }

            return $"<p {(isGreen ? "class='text-green'" : "")}>{paragrafo}</p>";
        }

        public string GetTextoGols(enumTipoAnalise tipoAnalise)
        {
            bool isGreen = false;
            string paragrafo = "";

            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    paragrafo =  $"Global: {GetTextoGols(Mandante.Global, Visitante.Global, out isGreen)}";
                    break;
                case enumTipoAnalise.ComMando:
                    paragrafo = $"Com mando: {GetTextoGols(Mandante.ComMando, Visitante.ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimo10:
                    paragrafo = $"Últimos 10 jogos: {GetTextoGols(Mandante.Ultimos10, Visitante.Ultimos10, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimos5ComMando:
                    paragrafo = $"Últimos 5 jogos com mando: {GetTextoGols(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Similares:
                    paragrafo = $"Similares: {GetTextoGols(Mandante.Similares, Visitante.Similares, out isGreen)}";
                    break;
                case enumTipoAnalise.SimilaresComMando:
                    paragrafo = $"Similares com mando: {GetTextoGols(Mandante.SimilaresComMando, Visitante.SimilaresComMando, out isGreen)}";
                    break;
                default:
                    paragrafo = "";
                    break;
            }
            return $"<p {(isGreen ? "class='text-green'" : "")}>{paragrafo}</p>";
        }

        public string GetTextoAmbas(enumTipoAnalise tipoAnalise)
        {
            bool isGreen = false;
            string paragrafo = "";

            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    paragrafo = $"Global: {GetTextoAmbas(Mandante.Global, Visitante.Global, out isGreen)}";
                    break;
                case enumTipoAnalise.ComMando:
                    paragrafo = $"Com mando: {GetTextoAmbas(Mandante.ComMando, Visitante.ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimo10:
                    paragrafo = $"Últimos 10 jogos: {GetTextoAmbas(Mandante.Ultimos10, Visitante.Ultimos10, out isGreen)}";
                    break;
                case enumTipoAnalise.Ultimos5ComMando:
                    paragrafo = $"Últimos 5 jogos com mando: {GetTextoAmbas(Mandante.Ultimos5ComMando, Visitante.Ultimos5ComMando, out isGreen)}";
                    break;
                case enumTipoAnalise.Similares:
                    paragrafo = $"Similares: {GetTextoAmbas(Mandante.Similares, Visitante.Similares, out isGreen)}";
                    break;
                case enumTipoAnalise.SimilaresComMando:
                    paragrafo = $"Similares com mando: {GetTextoAmbas(Mandante.SimilaresComMando, Visitante.SimilaresComMando, out isGreen)}";
                    break;
                default:
                    paragrafo = "";
                    break;
            }
            return $"<p {(isGreen ? "class='text-green'" : "")}>{paragrafo}</p>";
        }

        private string GetTextoResultadoExato(AnaliseTimeDetalhe mandante, AnaliseTimeDetalhe visitante, out bool isGreen)
        {
            isGreen = false;
            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal vitoriaCasa = (mandante.PercentualVitoria + visitante.PercentualDerrota) / 2;
            decimal empate = (mandante.PercentualEmpate + visitante.PercentualEmpate) / 2;
            decimal vitoriaFora = (mandante.PercentualDerrota + visitante.PercentualVitoria) / 2;

            if ((vitoriaCasa >= 50 && mandante.PercentualVitoria >=50) || 
                (empate >= 50 && mandante.PercentualEmpate >= 50 && visitante.PercentualEmpate >= 50) || 
                (vitoriaFora >= 50 && visitante.PercentualVitoria >= 50))
                isGreen = true;

            return $"A média final de vitória do {Mandante.Nome} é de {vitoriaCasa}%. Para empate a média é de {empate}% enquanto a média de vitórias para {Visitante.Nome} é de {vitoriaFora}%";
        }

        private string GetTextoGols(AnaliseTimeDetalhe mandante, AnaliseTimeDetalhe visitante, out bool isGreen)
        {
            isGreen = false;

            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal mediaOver25 = (mandante.PercentualOver25 + visitante.PercentualOver25) / 2;
            decimal mediaUnder25 = (mandante.PercentualUnder25 + visitante.PercentualUnder25) / 2;
            
            if ((mediaOver25 >= 70 && mandante.PercentualOver25 >= 50 && visitante.PercentualOver25 >= 50) 
                || (mediaUnder25 >= 70 && mandante.PercentualUnder25 >= 50 && visitante.PercentualUnder25 >=50))
                isGreen = true;

            if (mediaOver25 > mediaUnder25)
                return $"Over {mediaOver25}%";

            if (mediaUnder25 > mediaOver25)
                return $"Under {mediaUnder25}%";

            return $"Neutro";
        }

        private string GetTextoAmbas(AnaliseTimeDetalhe mandante, AnaliseTimeDetalhe visitante, out bool isGreen)
        {
            isGreen = false;
            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal mediaAmbas = (mandante.PercentualAmbas + visitante.PercentualAmbas) / 2;
            if (mediaAmbas > 60 && mandante.PercentualAmbas >=50 && visitante.PercentualAmbas >= 50)
                isGreen = true;

            return $"Ambas marcam {mediaAmbas}%";


        }

    }
}