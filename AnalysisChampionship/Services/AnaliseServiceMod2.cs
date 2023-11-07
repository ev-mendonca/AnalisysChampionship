using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Services
{
    public class AnaliseServiceMod2 : AnaliseService, IAnaliseService
    {
        public AnaliseServiceMod2() : base() { }
        public override Analise GetAnalise(int campeonatoID, int timeCasaID, int timeForaID)
        {
            Analise analise = base.GetAnalise(campeonatoID, timeCasaID, timeForaID);

            decimal vitoriasCasa = GetVitorias(analise.Mandante) + GetDerrotas(analise.Visitante);
            decimal vitoriasFora = GetVitorias(analise.Visitante) + GetDerrotas(analise.Mandante);
            decimal empates = GetEmpates(analise.Mandante) + GetEmpates(analise.Visitante);

            decimal total = vitoriasCasa + vitoriasFora + empates;

            analise.VitoriaCasa = Decimal.Round((vitoriasCasa / total), 2) * 100;
            analise.Empate = Decimal.Round((empates / total), 2) * 100;
            analise.VitoriaFora = Decimal.Round((vitoriasFora / total), 2) * 100;

            decimal vitoriasEmpateCasa = GetVitoriasEmpate(analise.Mandante) + GetDerrotasEmpate(analise.Visitante);
            decimal vitoriasEmpateFora = GetVitoriasEmpate(analise.Visitante) + GetDerrotasEmpate(analise.Mandante);
            decimal vitoriasCasaOuFora = GetVitoriasOuDerrotas(analise.Mandante) + GetVitoriasOuDerrotas(analise.Visitante);

            total = vitoriasEmpateCasa + vitoriasEmpateFora + vitoriasCasaOuFora;

            analise.VitoriaEmpateCasa = Decimal.Round((vitoriasEmpateCasa / total), 2) * 100;
            analise.VitoriaEmpateFora = Decimal.Round((vitoriasEmpateFora / total), 2) * 100;
            analise.VitoriaCasaOuFora = Decimal.Round((vitoriasCasaOuFora / total), 2) * 100;


            decimal overGols = GetOver15(analise.Mandante) + GetOver15(analise.Visitante);
            decimal underGols = GetUnder15(analise.Mandante) + GetUnder15(analise.Visitante);
            
            analise.Over15 = Decimal.Round((overGols / (overGols + underGols)), 2) * 100;

            overGols = GetOver25(analise.Mandante) + GetOver25(analise.Visitante);
            underGols = GetUnder25(analise.Mandante) + GetUnder25(analise.Visitante);

            analise.Over25 = Decimal.Round((overGols / (overGols + underGols)),2) * 100;
            analise.Under25 = Decimal.Round((underGols / (overGols + underGols)),2) * 100;

            overGols = GetOver35(analise.Mandante) + GetOver35(analise.Visitante);
            underGols = GetUnder35(analise.Mandante) + GetUnder35(analise.Visitante);

            analise.Under35 = Decimal.Round((underGols / (overGols + underGols)),2) * 100;

            decimal ambas = GetAmbas(analise.Mandante) + GetAmbas(analise.Visitante);
            decimal ambasNao = GetAmbasNao(analise.Mandante) + GetAmbasNao(analise.Visitante);

            analise.Ambas = Decimal.Round((ambas / (ambas + ambasNao)),2) * 100;

            GetInsightPlacarExato(analise);
            GetInsightDuplaChance(analise);
            GetInsightGols(analise);
            GetInsightAmbas(analise);

            return analise;
        }

        
        private decimal GetVitorias(AnaliseTime time)
        {
            return
                  time.Global.Vitorias + time.ComMando.Vitorias + time.Similares.Vitorias
                + time.SimilaresComMando.Vitorias + time.Ultimos10.Vitorias
                + time.Ultimos10ComMando.Vitorias + time.Ultimos5.Vitorias
                + time.Ultimos5ComMando.Vitorias;
        }
        private decimal GetDerrotas(AnaliseTime time)
        {
            return
                  time.Global.Derrotas + time.ComMando.Derrotas + time.Similares.Derrotas
                + time.SimilaresComMando.Derrotas + time.Ultimos10.Derrotas
                + time.Ultimos10ComMando.Derrotas + time.Ultimos5.Derrotas
                + time.Ultimos5ComMando.Derrotas;
        }
        private decimal GetEmpates(AnaliseTime time)
        {
            return
                  time.Global.Empates + time.ComMando.Empates + time.Similares.Empates
                + time.SimilaresComMando.Empates + time.Ultimos10.Empates
                + time.Ultimos10ComMando.Empates + time.Ultimos5.Empates
                + time.Ultimos5ComMando.Empates;
        }
        private decimal GetVitoriasEmpate(AnaliseTime time)
        {
            return
                  time.Global.VitoriaEmpate + time.ComMando.VitoriaEmpate
                + time.Similares.VitoriaEmpate + time.SimilaresComMando.VitoriaEmpate
                + time.Ultimos10.VitoriaEmpate + time.Ultimos10ComMando.VitoriaEmpate
                + time.Ultimos5.VitoriaEmpate + time.Ultimos5ComMando.VitoriaEmpate;
        }
        private decimal GetDerrotasEmpate(AnaliseTime time)
        {
            return
                  time.Global.DerrotaEmpate + time.ComMando.DerrotaEmpate
                + time.Similares.DerrotaEmpate + time.SimilaresComMando.DerrotaEmpate
                + time.Ultimos10.DerrotaEmpate + time.Ultimos10ComMando.DerrotaEmpate
                + time.Ultimos5.DerrotaEmpate + time.Ultimos5ComMando.DerrotaEmpate;
        }
        private decimal GetVitoriasOuDerrotas(AnaliseTime time)
        {
            return
                  time.Global.VitoriaOuDerrota + time.ComMando.VitoriaOuDerrota
                + time.Similares.VitoriaOuDerrota + time.SimilaresComMando.VitoriaOuDerrota
                + time.Ultimos10.VitoriaOuDerrota + time.Ultimos10ComMando.VitoriaOuDerrota
                + time.Ultimos5.VitoriaOuDerrota + time.Ultimos5ComMando.VitoriaOuDerrota;
        }
        private decimal GetOver15(AnaliseTime time)
        {
            return
                  time.Global.TotalOver15 + time.ComMando.TotalOver15
                + time.Similares.TotalOver15 + time.SimilaresComMando.TotalOver15
                + time.Ultimos10.TotalOver15 + time.Ultimos10ComMando.TotalOver15
                + time.Ultimos5.TotalOver15 + time.Ultimos5ComMando.TotalOver15;
        }
        private decimal GetUnder15(AnaliseTime time)
        {
            return
                  time.Global.TotalUnder15 + time.ComMando.TotalUnder15
                + time.Similares.TotalUnder15 + time.SimilaresComMando.TotalUnder15
                + time.Ultimos10.TotalUnder15 + time.Ultimos10ComMando.TotalUnder15
                + time.Ultimos5.TotalUnder15 + time.Ultimos5ComMando.TotalUnder15;
        }
        private decimal GetOver25(AnaliseTime time)
        {
            return
                  time.Global.TotalOver25 + time.ComMando.TotalOver25
                + time.Similares.TotalOver25 + time.SimilaresComMando.TotalOver25
                + time.Ultimos10.TotalOver25 + time.Ultimos10ComMando.TotalOver25
                + time.Ultimos5.TotalOver25 + time.Ultimos5ComMando.TotalOver25;
        }
        private decimal GetUnder25(AnaliseTime time)
        {
            return
                  time.Global.TotalUnder25 + time.ComMando.TotalUnder25
                + time.Similares.TotalUnder25 + time.SimilaresComMando.TotalUnder25
                + time.Ultimos10.TotalUnder25 + time.Ultimos10ComMando.TotalUnder25
                + time.Ultimos5.TotalUnder25 + time.Ultimos5ComMando.TotalUnder25;
        }
        private decimal GetUnder35(AnaliseTime time)
        {
            return
                  time.Global.TotalUnder35 + time.ComMando.TotalUnder35
                + time.Similares.TotalUnder35 + time.SimilaresComMando.TotalUnder35
                + time.Ultimos10.TotalUnder35 + time.Ultimos10ComMando.TotalUnder35
                + time.Ultimos5.TotalUnder35 + time.Ultimos5ComMando.TotalUnder35;
        }
        private decimal GetOver35(AnaliseTime time)
        {
            return
                  time.Global.TotalOver35 + time.ComMando.TotalOver35
                + time.Similares.TotalOver35 + time.SimilaresComMando.TotalOver35
                + time.Ultimos10.TotalOver35 + time.Ultimos10ComMando.TotalOver35
                + time.Ultimos5.TotalOver35 + time.Ultimos5ComMando.TotalOver35;
        }
        private decimal GetAmbas(AnaliseTime time)
        {
            return
                  time.Global.TotalAmbas + time.ComMando.TotalAmbas
                + time.Similares.TotalAmbas + time.SimilaresComMando.TotalAmbas
                + time.Ultimos10.TotalAmbas + time.Ultimos10ComMando.TotalAmbas
                + time.Ultimos5.TotalAmbas + time.Ultimos5ComMando.TotalAmbas;
        }
        private decimal GetAmbasNao(AnaliseTime time)
        {
            return
                  time.Global.TotalAmbasNao + time.ComMando.TotalAmbasNao
                + time.Similares.TotalAmbasNao + time.SimilaresComMando.TotalAmbasNao
                + time.Ultimos10.TotalAmbasNao + time.Ultimos10ComMando.TotalAmbasNao
                + time.Ultimos5.TotalAmbasNao + time.Ultimos5ComMando.TotalAmbasNao;
        }
    }
}