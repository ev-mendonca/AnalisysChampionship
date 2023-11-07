using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Services
{
    public abstract class AnaliseService : IAnaliseService
    {
        private readonly AnaliseRepository _repository;
        private readonly PartidaRepository _partidaRepository;
        public AnaliseService()
        {
            _repository = new AnaliseRepository();
            _partidaRepository = new PartidaRepository();
        }
        public virtual Analise GetAnalise(int campeonatoID, int timeCasaID, int timeForaID)
        {
            Analise analise;
            var partidasMandante = _partidaRepository.GetByCampeonato_Time(campeonatoID, timeCasaID);
            var partidasVisitante = _partidaRepository.GetByCampeonato_Time(campeonatoID, timeForaID);
            var analiseGeral = _repository.Get(campeonatoID);

            //Global
            analise = new Analise(analiseGeral.FirstOrDefault(x => x.ID == timeCasaID),
                                  analiseGeral.FirstOrDefault(x => x.ID == timeForaID));
            //Com Mando
            analise.Mandante.ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresMandante(analise.Mandante.ComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID));
            analise.Visitante.ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresVisitante(analise.Visitante.ComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID));
            //Ultimos 5
            analise.Mandante.Ultimos5 = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Mandante.Ultimos5, partidasMandante.Take(5), timeCasaID);
            analise.Visitante.Ultimos5 = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Visitante.Ultimos5, partidasVisitante.Take(5), timeForaID);
            //Ultimos 5 Com Mando
            analise.Mandante.Ultimos5ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresMandante(analise.Mandante.Ultimos5ComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID).Take(5));
            analise.Visitante.Ultimos5ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresVisitante(analise.Visitante.Ultimos5ComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID).Take(5));
            //Ultimos 10
            analise.Mandante.Ultimos10 = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Mandante.Ultimos10, partidasMandante.Take(10), timeCasaID);
            analise.Visitante.Ultimos10 = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Visitante.Ultimos10, partidasVisitante.Take(10), timeForaID);
            //Ultimos 10 Com Mando
            analise.Mandante.Ultimos10ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresMandante(analise.Mandante.Ultimos10ComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID).Take(10));
            analise.Visitante.Ultimos10ComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresVisitante(analise.Visitante.Ultimos10ComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID).Take(10));
            //Similares
            List<int> TimesSimilaresMandanteID = analiseGeral.Where(x => x.Global.Pontos >= analise.Mandante.Global.Pontos - 3 &&
                                                                         x.Global.Pontos <= analise.Mandante.Global.Pontos + 3).Where(x => x.ID != timeForaID).Select(x => x.ID).ToList();
            List<int> TimesSimilaresVisitanteID = analiseGeral.Where(x => x.Global.Pontos >= analise.Visitante.Global.Pontos - 3 &&
                                                                         x.Global.Pontos <= analise.Visitante.Global.Pontos + 3).Where(x => x.ID != timeCasaID).Select(x => x.ID).ToList();
            analise.Mandante.Similares = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Mandante.Similares, partidasMandante
                                                                       .Where(x => TimesSimilaresVisitanteID.Any(y => y == x.TimeCasaID || y == x.TimeForaID)), timeCasaID);
            analise.Visitante.Similares = new AnaliseTimeDetalhe();
            CalculaTotalizadoresSemMando(analise.Visitante.Similares, partidasVisitante
                                                       .Where(x => TimesSimilaresMandanteID.Any(y => y == x.TimeCasaID || y == x.TimeForaID)), timeForaID);
            //Similares Com Mando
            analise.Mandante.SimilaresComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresMandante(analise.Mandante.SimilaresComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID)
                                                                       .Where(x => TimesSimilaresVisitanteID.Any(y => y == x.TimeCasaID || y == x.TimeForaID)));
            analise.Visitante.SimilaresComMando = new AnaliseTimeDetalhe();
            CalculaTotalizadoresVisitante(analise.Visitante.SimilaresComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID)
                                                       .Where(x => TimesSimilaresMandanteID.Any(y => y == x.TimeCasaID || y == x.TimeForaID)));
            return analise;
        }
        
        private void CalculaTotalizadoresSemMando(AnaliseTimeDetalhe timeDetalhe, IEnumerable<Partida> partidas, int timeID)
        {
            foreach (var item in partidas)
            {
                if (item.GolsCasa == item.GolsFora)
                    timeDetalhe.Empates += 1;
                else
                {
                    if (item.TimeCasaID == timeID)
                    {
                        if (item.GolsCasa > item.GolsFora)
                            timeDetalhe.Vitorias += 1;
                        else
                            timeDetalhe.Derrotas += 1;
                    }
                    else
                    {
                        if (item.GolsFora > item.GolsCasa)
                            timeDetalhe.Vitorias += 1;
                        else
                            timeDetalhe.Derrotas += 1;
                    }
                }

                CalculaTotalizadoresGols(timeDetalhe, item);
            }
        }

        private void CalculaTotalizadoresMandante(AnaliseTimeDetalhe timeDetalhe, IEnumerable<Partida> partidas)
        {
            foreach (var item in partidas)
            {
                if (item.GolsCasa > item.GolsFora)
                    timeDetalhe.Vitorias += 1;
                else if (item.GolsCasa == item.GolsFora)
                    timeDetalhe.Empates += 1;
                else
                    timeDetalhe.Derrotas += 1;
                CalculaTotalizadoresGols(timeDetalhe, item);
            }
        }

        private void CalculaTotalizadoresVisitante(AnaliseTimeDetalhe timeDetalhe, IEnumerable<Partida> partidas)
        {
            foreach (var item in partidas)
            {
                if (item.GolsFora > item.GolsCasa)
                    timeDetalhe.Vitorias += 1;
                else if (item.GolsFora == item.GolsCasa)
                    timeDetalhe.Empates += 1;
                else
                    timeDetalhe.Derrotas += 1;
                CalculaTotalizadoresGols(timeDetalhe, item);
            }
        }

        private void CalculaTotalizadoresGols(AnaliseTimeDetalhe detalhe, Partida partida)
        {
            switch (partida.GolsCasa + partida.GolsFora)
            {
                case 0:
                case 1:
                    detalhe.TotalUnder15 += 1;
                    detalhe.TotalUnder25 += 1;
                    detalhe.TotalUnder35 += 1;
                    break;
                case 2:
                    detalhe.TotalOver15 += 1;
                    detalhe.TotalUnder25 += 1;
                    detalhe.TotalUnder35 += 1;
                    break;
                case 3:
                    detalhe.TotalOver15 += 1;
                    detalhe.TotalOver25 += 1;
                    detalhe.TotalUnder35 += 1;
                    break;
                default:
                    detalhe.TotalOver15 += 1;
                    detalhe.TotalOver25 += 1;
                    detalhe.TotalOver35 += 1;
                    break;
            }
            if (partida.GolsCasa > 0 && partida.GolsFora > 0)
                detalhe.TotalAmbas += 1;
            else
                detalhe.TotalAmbasNao += 1;
        }

        protected void GetInsightPlacarExato(Analise analise)
        {
            if (analise.VitoriaCasa >= 60
                && ValidaPlacarExato(analise.Mandante.ComMando.PercentualVitoria, analise.Visitante.ComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Global.PercentualVitoria, analise.Visitante.Global.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Similares.PercentualVitoria, analise.Visitante.Similares.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.SimilaresComMando.PercentualVitoria, analise.Visitante.SimilaresComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Ultimos10.PercentualVitoria, analise.Visitante.Ultimos10.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Ultimos10ComMando.PercentualVitoria, analise.Visitante.Ultimos10ComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Ultimos5.PercentualVitoria, analise.Visitante.Ultimos5.PercentualDerrota)
                && ValidaPlacarExato(analise.Mandante.Ultimos5ComMando.PercentualVitoria, analise.Visitante.Ultimos5ComMando.PercentualDerrota))
                analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA DO MANDANTE!!!</li>";

            if (analise.VitoriaFora >= 60
                && ValidaPlacarExato(analise.Visitante.ComMando.PercentualVitoria, analise.Mandante.ComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Global.PercentualVitoria, analise.Mandante.Global.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Similares.PercentualVitoria, analise.Mandante.Similares.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.SimilaresComMando.PercentualVitoria, analise.Mandante.SimilaresComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Ultimos10.PercentualVitoria, analise.Mandante.Ultimos10.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Ultimos10ComMando.PercentualVitoria, analise.Mandante.Ultimos10ComMando.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Ultimos5.PercentualVitoria, analise.Mandante.Ultimos5.PercentualDerrota)
                && ValidaPlacarExato(analise.Visitante.Ultimos5ComMando.PercentualVitoria, analise.Mandante.Ultimos5ComMando.PercentualDerrota))
                analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA DO VISITANTE!!!</li>";

            if (analise.Empate >= 60
                && ValidaPlacarExato(analise.Visitante.ComMando.PercentualEmpate, analise.Mandante.ComMando.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Global.PercentualEmpate, analise.Mandante.Global.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Similares.PercentualEmpate, analise.Mandante.Similares.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.SimilaresComMando.PercentualEmpate, analise.Mandante.SimilaresComMando.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Ultimos10.PercentualEmpate, analise.Mandante.Ultimos10.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Ultimos10ComMando.PercentualEmpate, analise.Mandante.Ultimos10ComMando.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Ultimos5.PercentualEmpate, analise.Mandante.Ultimos5.PercentualEmpate)
                && ValidaPlacarExato(analise.Visitante.Ultimos5ComMando.PercentualEmpate, analise.Mandante.Ultimos5ComMando.PercentualEmpate))
                analise.Insight += "<li class='text-green'>APOSTAR NO EMPATE!!!</li>";
        }
        protected void GetInsightDuplaChance(Analise analise)
        {
            if (analise.VitoriaEmpateCasa >= 60
                && ValidaDuplaChance(analise.Mandante.ComMando.PercentualVitoriaEmpate, analise.Visitante.ComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Global.PercentualVitoriaEmpate, analise.Visitante.Global.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Similares.PercentualVitoriaEmpate, analise.Visitante.Similares.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.SimilaresComMando.PercentualVitoriaEmpate, analise.Visitante.SimilaresComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Ultimos10.PercentualVitoriaEmpate, analise.Visitante.Ultimos10.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Ultimos10ComMando.PercentualVitoriaEmpate, analise.Visitante.Ultimos10ComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Ultimos5.PercentualVitoriaEmpate, analise.Visitante.Ultimos5.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Mandante.Ultimos5ComMando.PercentualVitoriaEmpate, analise.Visitante.Ultimos5ComMando.PercentualDerrotaEmpate))
                analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA/EMPATE DO MANDANTE!!!</li>";

            if (analise.VitoriaEmpateFora >= 60
                && ValidaDuplaChance(analise.Visitante.ComMando.PercentualVitoriaEmpate, analise.Mandante.ComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Global.PercentualVitoriaEmpate, analise.Mandante.Global.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Similares.PercentualVitoriaEmpate, analise.Mandante.Similares.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.SimilaresComMando.PercentualVitoriaEmpate, analise.Mandante.SimilaresComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Ultimos10.PercentualVitoriaEmpate, analise.Mandante.Ultimos10.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Ultimos10ComMando.PercentualVitoriaEmpate, analise.Mandante.Ultimos10ComMando.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Ultimos5.PercentualVitoriaEmpate, analise.Mandante.Ultimos5.PercentualDerrotaEmpate)
                && ValidaDuplaChance(analise.Visitante.Ultimos5ComMando.PercentualVitoriaEmpate, analise.Mandante.Ultimos5ComMando.PercentualDerrotaEmpate))
                analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA/EMPATE DO VISITANTE!!!</li>";

            if (analise.VitoriaCasaOuFora >= 60
                && ValidaDuplaChance(analise.Visitante.ComMando.PercentualVitoriaOuDerrota, analise.Mandante.ComMando.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Global.PercentualVitoriaOuDerrota, analise.Mandante.Global.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Similares.PercentualVitoriaOuDerrota, analise.Mandante.Similares.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.SimilaresComMando.PercentualVitoriaOuDerrota, analise.Mandante.SimilaresComMando.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Ultimos10.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos10.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Ultimos10ComMando.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos10ComMando.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Ultimos5.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos5.PercentualVitoriaOuDerrota)
                && ValidaDuplaChance(analise.Visitante.Ultimos5ComMando.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos5ComMando.PercentualVitoriaOuDerrota))
                analise.Insight += "<li class='text-green'>APOSTAR NA VITORIA DA CASA OU FORA!!!</li>";

        }
        protected void GetInsightGols(Analise analise)
        {
            if (analise.Over15 >= 60
                && ValidaGols(analise.Mandante.ComMando.PercentualOver15, analise.Visitante.ComMando.PercentualOver15)
                && ValidaGols(analise.Mandante.Global.PercentualOver15, analise.Visitante.Global.PercentualOver15)
                && ValidaGols(analise.Mandante.Similares.PercentualOver15, analise.Visitante.Similares.PercentualOver15)
                && ValidaGols(analise.Mandante.SimilaresComMando.PercentualOver15, analise.Visitante.SimilaresComMando.PercentualOver15)
                && ValidaGols(analise.Mandante.Ultimos10.PercentualOver15, analise.Visitante.Ultimos10.PercentualOver15)
                && ValidaGols(analise.Mandante.Ultimos10ComMando.PercentualOver15, analise.Visitante.Ultimos10ComMando.PercentualOver15)
                && ValidaGols(analise.Mandante.Ultimos5.PercentualOver15, analise.Visitante.Ultimos5.PercentualOver15)
                && ValidaGols(analise.Mandante.Ultimos5ComMando.PercentualOver15, analise.Visitante.Ultimos5ComMando.PercentualOver15))
                analise.Insight += "<li class='text-green'>APOSTAR NO +1.5 GOLS!!!</li>";

            if (analise.Over25 >= 60
                && ValidaGols(analise.Visitante.ComMando.PercentualOver25, analise.Mandante.ComMando.PercentualOver25)
                && ValidaGols(analise.Visitante.Global.PercentualOver25, analise.Mandante.Global.PercentualOver25)
                && ValidaGols(analise.Visitante.Similares.PercentualOver25, analise.Mandante.Similares.PercentualOver25)
                && ValidaGols(analise.Visitante.SimilaresComMando.PercentualOver25, analise.Mandante.SimilaresComMando.PercentualOver25)
                && ValidaGols(analise.Visitante.Ultimos10.PercentualOver25, analise.Mandante.Ultimos10.PercentualOver25)
                && ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualOver25, analise.Mandante.Ultimos10ComMando.PercentualOver25)
                && ValidaGols(analise.Visitante.Ultimos5.PercentualOver25, analise.Mandante.Ultimos5.PercentualOver25)
                && ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualOver25, analise.Mandante.Ultimos5ComMando.PercentualOver25))
                analise.Insight += "<li class='text-green'>APOSTAR NO +2.5 GOLS!!!</li>";

            if (analise.Under25 >= 60
                && ValidaGols(analise.Visitante.ComMando.PercentualUnder25, analise.Mandante.ComMando.PercentualUnder25)
                && ValidaGols(analise.Visitante.Global.PercentualUnder25, analise.Mandante.Global.PercentualUnder25)
                && ValidaGols(analise.Visitante.Similares.PercentualUnder25, analise.Mandante.Similares.PercentualUnder25)
                && ValidaGols(analise.Visitante.SimilaresComMando.PercentualUnder25, analise.Mandante.SimilaresComMando.PercentualUnder25)
                && ValidaGols(analise.Visitante.Ultimos10.PercentualUnder25, analise.Mandante.Ultimos10.PercentualUnder25)
                && ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualUnder25, analise.Mandante.Ultimos10ComMando.PercentualUnder25)
                && ValidaGols(analise.Visitante.Ultimos5.PercentualUnder25, analise.Mandante.Ultimos5.PercentualUnder25)
                && ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualUnder25, analise.Mandante.Ultimos5ComMando.PercentualUnder25))
                analise.Insight += "<li class='text-green'>APOSTAR NO -2.5 GOLS!!!</li>";
            if (analise.Under35 >= 60
            && ValidaGols(analise.Visitante.ComMando.PercentualUnder35, analise.Mandante.ComMando.PercentualUnder35)
            && ValidaGols(analise.Visitante.Global.PercentualUnder35, analise.Mandante.Global.PercentualUnder35)
            && ValidaGols(analise.Visitante.Similares.PercentualUnder35, analise.Mandante.Similares.PercentualUnder35)
            && ValidaGols(analise.Visitante.SimilaresComMando.PercentualUnder35, analise.Mandante.SimilaresComMando.PercentualUnder35)
            && ValidaGols(analise.Visitante.Ultimos10.PercentualUnder35, analise.Mandante.Ultimos10.PercentualUnder35)
            && ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualUnder35, analise.Mandante.Ultimos10ComMando.PercentualUnder35)
            && ValidaGols(analise.Visitante.Ultimos5.PercentualUnder35, analise.Mandante.Ultimos5.PercentualUnder35)
            && ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualUnder35, analise.Mandante.Ultimos5ComMando.PercentualUnder35))
                analise.Insight += "<li class='text-green'>APOSTAR NO -3.5 GOLS!!!</li>";

        }
        protected void GetInsightAmbas(Analise analise)
        {
            if (analise.Ambas >= 60
                && ValidaAmbas(analise.Mandante.ComMando.PercentualAmbas, analise.Visitante.ComMando.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Global.PercentualAmbas, analise.Visitante.Global.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Similares.PercentualAmbas, analise.Visitante.Similares.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.SimilaresComMando.PercentualAmbas, analise.Visitante.SimilaresComMando.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Ultimos10.PercentualAmbas, analise.Visitante.Ultimos10.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Ultimos10ComMando.PercentualAmbas, analise.Visitante.Ultimos10ComMando.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Ultimos5.PercentualAmbas, analise.Visitante.Ultimos5.PercentualAmbas)
                && ValidaAmbas(analise.Mandante.Ultimos5ComMando.PercentualAmbas, analise.Visitante.Ultimos5ComMando.PercentualAmbas))
                analise.Insight += "<li class='text-green'>APOSTAR NO AMBAS MARCAM!!!</li>";

            if (analise.Ambas <= 40
            && ValidaAmbas(analise.Mandante.ComMando.PercentualAmbas, analise.Visitante.ComMando.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Global.PercentualAmbas, analise.Visitante.Global.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Similares.PercentualAmbas, analise.Visitante.Similares.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.SimilaresComMando.PercentualAmbas, analise.Visitante.SimilaresComMando.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Ultimos10.PercentualAmbas, analise.Visitante.Ultimos10.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Ultimos10ComMando.PercentualAmbas, analise.Visitante.Ultimos10ComMando.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Ultimos5.PercentualAmbas, analise.Visitante.Ultimos5.PercentualAmbas, false)
            && ValidaAmbas(analise.Mandante.Ultimos5ComMando.PercentualAmbas, analise.Visitante.Ultimos5ComMando.PercentualAmbas, false))
                analise.Insight += "<li class='text-green'>APOSTAR NO AMBAS NÃO MARCAM!!!</li>";

        }

        protected bool ValidaPlacarExato(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 60 && percentual2 >= 60;
        }
        protected bool ValidaDuplaChance(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 60 && percentual2 >= 60;
        }
        protected bool ValidaGols(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 60 && percentual2 >= 60;
        }
        protected bool ValidaAmbas(decimal percentual1, decimal percentual2, bool ambasSim = true)
        {
            return ambasSim ? percentual1 >= 60 && percentual2 >= 60
                : percentual1 <= 40 && percentual2 <= 40;
        }
    }
}