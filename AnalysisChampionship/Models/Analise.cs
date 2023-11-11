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

        public int PercentualVitoriaCasa => GetResultado(enumTipoCalculo.VitoriaCasa);
        public int PercentualEmpate => GetResultado(enumTipoCalculo.Empate);
        public int PercentualVitoriaFora => GetResultado(enumTipoCalculo.VitoriaFora);
        public int PercentualVitoriaEmpateCasa => GetResultado(enumTipoCalculo.VitoriaEmpateCasa);
        public int PercentualVitoriaEmpateFora => GetResultado(enumTipoCalculo.VitoriaEmpateFora);
        public int PercentualVitoriaCasaFora => GetResultado(enumTipoCalculo.VitoriaCasaFora);
        public int PercentualOver15 => GetResultado(enumTipoCalculo.Over15);
        public int PercentualOver25 => GetResultado(enumTipoCalculo.Over25);
        public int PercentualUnder25 => GetResultado(enumTipoCalculo.Under25);
        public int PercentualUnder35 => GetResultado(enumTipoCalculo.Under35);
        public int PercentualAmbas => GetResultado(enumTipoCalculo.Ambas);

        private int GetResultado(enumTipoCalculo tipoCalculo)
        {
            enumTipoResultado typeMandante, typeVisitante;

            switch (tipoCalculo)
            {
                case enumTipoCalculo.VitoriaCasa:
                    typeMandante = enumTipoResultado.Vitoria;
                    typeVisitante = enumTipoResultado.Derrota;
                    break;

                case enumTipoCalculo.VitoriaFora:
                    typeMandante = enumTipoResultado.Derrota;
                    typeVisitante = enumTipoResultado.Vitoria;
                    break;

                case enumTipoCalculo.Empate:
                    typeMandante = typeVisitante = enumTipoResultado.Empate;
                    break;

                case enumTipoCalculo.VitoriaEmpateCasa:
                    typeMandante = enumTipoResultado.VitoriaEmpate;
                    typeVisitante = enumTipoResultado.DerrotaEmpate;
                    break;

                case enumTipoCalculo.VitoriaEmpateFora:
                    typeMandante = enumTipoResultado.DerrotaEmpate;
                    typeVisitante = enumTipoResultado.VitoriaEmpate;
                    break;

                case enumTipoCalculo.VitoriaCasaFora:
                    typeMandante = enumTipoResultado.VitoriaDerrota;
                    typeVisitante = enumTipoResultado.VitoriaDerrota;
                    break;

                case enumTipoCalculo.Over15:
                    typeMandante = enumTipoResultado.Over15;
                    typeVisitante = enumTipoResultado.Over15;
                    break;

                case enumTipoCalculo.Over25:
                    typeMandante = enumTipoResultado.Over25;
                    typeVisitante = enumTipoResultado.Over25;
                    break;

                case enumTipoCalculo.Under25:
                    typeMandante = enumTipoResultado.Under25;
                    typeVisitante = enumTipoResultado.Under25;
                    break;

                case enumTipoCalculo.Under35:
                    typeMandante = enumTipoResultado.Under35;
                    typeVisitante = enumTipoResultado.Under35;
                    break;

                default:
                    typeMandante = enumTipoResultado.Ambas;
                    typeVisitante = enumTipoResultado.Ambas;
                    break;
            }

            decimal resultado = 0, contador = 0;
            GetResultado(Mandante, typeMandante, ref resultado, ref contador);
            GetResultado(Visitante, typeVisitante, ref resultado, ref contador);
            return Convert.ToInt32((resultado / contador) * 100);
        }

        private void GetResultado(AnaliseTime time, enumTipoResultado type, ref decimal resultado, ref decimal contador)
        {
            GetResultado(time.Global, type, ref resultado, ref contador);
            GetResultado(time.ComMando, type, ref resultado, ref contador);
            GetResultado(time.Ultimos10, type, ref resultado, ref contador);
            GetResultado(time.Ultimos10ComMando, type, ref resultado, ref contador);
            GetResultado(time.Ultimos5, type, ref resultado, ref contador);
            GetResultado(time.Ultimos5ComMando, type, ref resultado, ref contador);
            GetResultado(time.Similares, type, ref resultado, ref contador);
            GetResultado(time.SimilaresComMando, type, ref resultado, ref contador);
        }

        private void GetResultado(AnaliseTimeDetalhe time, enumTipoResultado type, ref decimal resultado, ref decimal contador)
        {
            if (time.Partidas > 0)
            {
                contador++;
                switch (type)
                {
                    case enumTipoResultado.Vitoria:
                        resultado += GetResultado(time.PercentualVitoria, 50, 30);
                        break;
                    case enumTipoResultado.Empate:
                        resultado += GetResultado(time.PercentualEmpate, 50, 30);
                        break;
                    case enumTipoResultado.Derrota:
                        resultado += GetResultado(time.PercentualDerrota, 50, 30);
                        break;
                    case enumTipoResultado.VitoriaEmpate:
                        resultado += GetResultado(time.PercentualVitoriaEmpate, 70, 50);
                        break;
                    case enumTipoResultado.DerrotaEmpate:
                        resultado += GetResultado(time.PercentualDerrotaEmpate, 70, 50);
                        break;
                    case enumTipoResultado.VitoriaDerrota:
                        resultado += GetResultado(time.PercentualVitoriaOuDerrota, 70, 50);
                        break;
                    case enumTipoResultado.Over15:
                        resultado += GetResultado(time.PercentualOver15, 70, 50);
                        break;
                    case enumTipoResultado.Over25:
                        resultado += GetResultado(time.PercentualOver25, 70, 50);
                        break;
                    case enumTipoResultado.Under25:
                        resultado += GetResultado(time.PercentualUnder25, 70, 50);
                        break;
                    case enumTipoResultado.Under35:
                        resultado += GetResultado(time.PercentualUnder35, 70, 50);
                        break;
                    case enumTipoResultado.Ambas:
                        resultado += GetResultado(time.PercentualAmbas, 60, 40);
                        break;
                }
            }
        }

        private decimal GetResultado(decimal value, int percentualPositivo, int percentualNegativo)
        {
            if (value >= percentualPositivo)
                return 1;
            else if (value <= percentualNegativo)
                return -1;
            else
                return 0;
        }

        public decimal VitoriaCasa { get; set; }
        public decimal VitoriaEmpateCasa { get; set; }
        public decimal Empate { get; set; }
        public decimal VitoriaFora { get; set; }
        public decimal VitoriaEmpateFora { get; set; }

        public decimal VitoriaCasaOuFora { get; set; }

        public decimal Over15 { get; set; }
        public decimal Over25 { get; set; }
        public decimal Under25 { get; set; }
        public decimal Under35 { get; set; }
        public decimal Ambas { get; set; }

        public string Insight { get; set; }

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

            if (vitoriaCasa >= 50 || empate >= 50 || vitoriaFora >= 50)
                isGreen = true;

            return $"A média final de vitória do {Mandante.Nome} é de {vitoriaCasa}%. Para empate a média é de {empate}% enquanto a média de vitórias para {Visitante.Nome} é de {vitoriaFora}%";
        }

        private string GetTextoGols(AnaliseTimeDetalhe mandante, AnaliseTimeDetalhe visitante, out bool isGreen)
        {
            isGreen = false;

            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal mediaOver = (mandante.PercentualOver25 + visitante.PercentualOver25) / 2;
            decimal mediaUnder = (mandante.PercentualUnder25 + visitante.PercentualUnder25) / 2;

            if (mediaOver >= 70 || mediaUnder >= 70)
                isGreen = true;

            if (mediaOver > mediaUnder)
                return $"Over {mediaOver}%";

            if (mediaUnder > mediaOver)
                return $"Under {mediaUnder}%";

            return $"Neutro";
        }

        private string GetTextoAmbas(AnaliseTimeDetalhe mandante, AnaliseTimeDetalhe visitante, out bool isGreen)
        {
            isGreen = false;
            if (mandante.Partidas == 0 || visitante.Partidas == 0)
                return "N/A";

            decimal mediaAmbas = (mandante.PercentualAmbas + visitante.PercentualAmbas) / 2;
            if (mediaAmbas > 60)
                isGreen = true;

            return $"Ambas marcam {mediaAmbas}%";


        }

    }
}