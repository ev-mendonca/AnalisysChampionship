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
            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    return $"Global: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.Global)}  e {Visitante.Nome} - {GetTextoResultadoExato(Visitante.Global)}";
                case enumTipoAnalise.ComMando:
                    return $"Com mando: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.ComMando)} e {Visitante.Nome} - {GetTextoResultadoExato(Visitante.ComMando)}";
                case enumTipoAnalise.Ultimo10:
                    return $"Últimos 10 jogos: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.Ultimos10)} e {Visitante.Nome} - {GetTextoResultadoExato(Visitante.Ultimos10)}";
                case enumTipoAnalise.Ultimos5ComMando:
                    return $"Últimos 5 jogos com mando: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.Ultimos5ComMando)} e {Visitante.Nome} - {GetTextoResultadoExato(Visitante.Ultimos5ComMando)}";
                case enumTipoAnalise.Similares:
                    return $"Similares: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.Similares)} e {Visitante.Nome} - {GetTextoResultadoExato(Visitante.Similares)}";
                case enumTipoAnalise.SimilaresComMando:
                    return $"Similares com mando: {Mandante.Nome} - {GetTextoResultadoExato(Mandante.SimilaresComMando)}  e  {Visitante.Nome}  -  {GetTextoResultadoExato(Visitante.SimilaresComMando)}";
                default:
                    return "";
            }
        }

        public string GetTextoGols(enumTipoAnalise tipoAnalise)
        {
            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    return $"Global: {Mandante.Nome} - {GetTextoGols(Mandante.Global)}  e {Visitante.Nome} - {GetTextoGols(Visitante.Global)}";
                case enumTipoAnalise.ComMando:
                    return $"Com mando: {Mandante.Nome} - {GetTextoGols(Mandante.ComMando)} e {Visitante.Nome} - {GetTextoGols(Visitante.ComMando)}";
                case enumTipoAnalise.Ultimo10:
                    return $"Últimos 10 jogos: {Mandante.Nome} - {GetTextoGols(Mandante.Ultimos10)} e {Visitante.Nome} - {GetTextoGols(Visitante.Ultimos10)}";
                case enumTipoAnalise.Ultimos5ComMando:
                    return $"Últimos 5 jogos com mando: {Mandante.Nome} - {GetTextoGols(Mandante.Ultimos5ComMando)} e {Visitante.Nome} - {GetTextoGols(Visitante.Ultimos5ComMando)}";
                case enumTipoAnalise.Similares:
                    return $"Similares: {Mandante.Nome} - {GetTextoGols(Mandante.Similares)} e {Visitante.Nome} - {GetTextoGols(Visitante.Similares)}";
                case enumTipoAnalise.SimilaresComMando:
                    return $"Similares com mando: {Mandante.Nome} - {GetTextoGols(Mandante.SimilaresComMando)}  e  {Visitante.Nome}  -  {GetTextoGols(Visitante.SimilaresComMando)}";
                default:
                    return "";
            }
        }

        public string GetTextoAmbas(enumTipoAnalise tipoAnalise)
        {
            switch (tipoAnalise)
            {
                case enumTipoAnalise.Global:
                    return $"Global: {Mandante.Nome} - {GetTextoAmbas(Mandante.Global)}  e {Visitante.Nome} - {GetTextoAmbas(Visitante.Global)}";
                case enumTipoAnalise.ComMando:
                    return $"Com mando: {Mandante.Nome} - {GetTextoAmbas(Mandante.ComMando)} e {Visitante.Nome} - {GetTextoAmbas(Visitante.ComMando)}";
                case enumTipoAnalise.Ultimo10:
                    return $"Últimos 10 jogos: {Mandante.Nome} - {GetTextoAmbas(Mandante.Ultimos10)} e {Visitante.Nome} - {GetTextoAmbas(Visitante.Ultimos10)}";
                case enumTipoAnalise.Ultimos5ComMando:
                    return $"Últimos 5 jogos com mando: {Mandante.Nome} - {GetTextoAmbas(Mandante.Ultimos5ComMando)} e {Visitante.Nome} - {GetTextoAmbas(Visitante.Ultimos5ComMando)}";
                case enumTipoAnalise.Similares:
                    return $"Similares: {Mandante.Nome} - {GetTextoAmbas(Mandante.Similares)} e {Visitante.Nome} - {GetTextoAmbas(Visitante.Similares)}";
                case enumTipoAnalise.SimilaresComMando:
                    return $"Similares com mando: {Mandante.Nome} - {GetTextoAmbas(Mandante.SimilaresComMando)}  e  {Visitante.Nome}  -  {GetTextoAmbas(Visitante.SimilaresComMando)}";
                default:
                    return "";
            }
        }

        private string GetTextoResultadoExato(AnaliseTimeDetalhe detalhe)
        {
            if (detalhe.Partidas == 0)
                return "N/A";

            if (detalhe.PercentualVitoria > detalhe.PercentualEmpate && detalhe.PercentualVitoria > detalhe.PercentualDerrota)
                return $"Vitória";

            if (detalhe.PercentualEmpate > detalhe.PercentualVitoria && detalhe.PercentualEmpate > detalhe.PercentualDerrota)
                return $"Empates";

            if (detalhe.PercentualDerrota > detalhe.PercentualVitoria && detalhe.PercentualDerrota > detalhe.PercentualEmpate)
                return $"Derrotas";
            
            return $"Neutro";
        }

        private string GetTextoGols(AnaliseTimeDetalhe detalhe)
        {
            if (detalhe.Partidas == 0)
                return "N/A";

            if (detalhe.PercentualOver25 > detalhe.PercentualUnder25)
                return $"Over";

            if (detalhe.PercentualUnder25 > detalhe.PercentualOver25)
                return $"Under";

            return $"Neutro";
        }

        private string GetTextoAmbas(AnaliseTimeDetalhe detalhe)
        {
            if (detalhe.Partidas == 0)
                return "N/A";

            if (detalhe.PercentualAmbas > 50)
                return $"Ambas marcam";

            if (detalhe.PercentualAmbas < 50)
                return $"Ambas não marcam";

            return $"Neutro";
        }

    }
}