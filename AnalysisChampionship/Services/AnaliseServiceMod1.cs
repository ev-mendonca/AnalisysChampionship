using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Services
{
    public class AnaliseServiceMod1 : IAnaliseService
    {
        private readonly AnaliseRepository _repository;
        private readonly PartidaRepository _partidaRepository;
        public AnaliseServiceMod1()
        {
            _repository = new AnaliseRepository();
            _partidaRepository = new PartidaRepository();
        }
        public Analise GetAnalise(int campeonatoID, int timeCasaID, int timeForaID)
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

            #region Resultado Exato 1
            analise.VitoriaCasa = (analise.Mandante.Global.PercentualVitoria
                + analise.Mandante.ComMando.PercentualVitoria
                + analise.Mandante.Similares.PercentualVitoria
                + analise.Mandante.SimilaresComMando.PercentualVitoria
                + analise.Mandante.Ultimos10.PercentualVitoria
                + analise.Mandante.Ultimos10ComMando.PercentualVitoria
                + analise.Mandante.Ultimos5.PercentualVitoria
                + analise.Mandante.Ultimos5ComMando.PercentualVitoria
                + analise.Visitante.Global.PercentualDerrota
                + analise.Visitante.ComMando.PercentualDerrota
                + analise.Visitante.Similares.PercentualDerrota
                + analise.Visitante.SimilaresComMando.PercentualDerrota
                + analise.Visitante.Ultimos10.PercentualDerrota
                + analise.Visitante.Ultimos10ComMando.PercentualDerrota
                + analise.Visitante.Ultimos5.PercentualDerrota
                + analise.Visitante.Ultimos5ComMando.PercentualDerrota) / 16;
            #endregion

            #region Resultado Exato X
            analise.Empate = (analise.Visitante.Global.PercentualEmpate
                + analise.Visitante.ComMando.PercentualEmpate
                + analise.Visitante.Similares.PercentualEmpate
                + analise.Visitante.SimilaresComMando.PercentualEmpate
                + analise.Visitante.Ultimos10.PercentualEmpate
                + analise.Visitante.Ultimos10ComMando.PercentualEmpate
                + analise.Visitante.Ultimos5.PercentualEmpate
                + analise.Visitante.Ultimos5ComMando.PercentualEmpate
                + analise.Mandante.Global.PercentualEmpate
                + analise.Mandante.ComMando.PercentualEmpate
                + analise.Mandante.Similares.PercentualEmpate
                + analise.Mandante.SimilaresComMando.PercentualEmpate
                + analise.Mandante.Ultimos10.PercentualEmpate
                + analise.Mandante.Ultimos10ComMando.PercentualEmpate
                + analise.Mandante.Ultimos5.PercentualEmpate
                + analise.Mandante.Ultimos5ComMando.PercentualEmpate) / 16;
            #endregion

            #region Resultado Exato 2
            analise.VitoriaFora = (analise.Visitante.Global.PercentualVitoria
                + analise.Visitante.ComMando.PercentualVitoria
                + analise.Visitante.Similares.PercentualVitoria
                + analise.Visitante.SimilaresComMando.PercentualVitoria
                + analise.Visitante.Ultimos10.PercentualVitoria
                + analise.Visitante.Ultimos10ComMando.PercentualVitoria
                + analise.Visitante.Ultimos5.PercentualVitoria
                + analise.Visitante.Ultimos5ComMando.PercentualVitoria
                + analise.Mandante.Global.PercentualDerrota
                + analise.Mandante.ComMando.PercentualDerrota
                + analise.Mandante.Similares.PercentualDerrota
                + analise.Mandante.SimilaresComMando.PercentualDerrota
                + analise.Mandante.Ultimos10.PercentualDerrota
                + analise.Mandante.Ultimos10ComMando.PercentualDerrota
                + analise.Mandante.Ultimos5.PercentualDerrota
                + analise.Mandante.Ultimos5ComMando.PercentualDerrota) / 16;
            #endregion

            #region Dupla Chance 1x
            analise.VitoriaEmpateCasa = (analise.Mandante.Global.PercentualVitoriaEmpate
                + analise.Mandante.ComMando.PercentualVitoriaEmpate
                + analise.Mandante.Similares.PercentualVitoriaEmpate
                + analise.Mandante.SimilaresComMando.PercentualVitoriaEmpate
                + analise.Mandante.Ultimos10.PercentualVitoriaEmpate
                + analise.Mandante.Ultimos10ComMando.PercentualVitoriaEmpate
                + analise.Mandante.Ultimos5.PercentualVitoriaEmpate
                + analise.Mandante.Ultimos5ComMando.PercentualVitoriaEmpate
                + analise.Visitante.Global.PercentualDerrotaEmpate
                + analise.Visitante.ComMando.PercentualDerrotaEmpate
                + analise.Visitante.Similares.PercentualDerrotaEmpate
                + analise.Visitante.SimilaresComMando.PercentualDerrotaEmpate
                + analise.Visitante.Ultimos10.PercentualDerrotaEmpate
                + analise.Visitante.Ultimos10ComMando.PercentualDerrotaEmpate
                + analise.Visitante.Ultimos5.PercentualDerrotaEmpate
                + analise.Visitante.Ultimos5ComMando.PercentualDerrotaEmpate) / 16;
            #endregion

            #region Dupla Chance x2
            analise.VitoriaEmpateFora = (analise.Visitante.Global.PercentualVitoriaEmpate
                + analise.Visitante.ComMando.PercentualVitoriaEmpate
                + analise.Visitante.Similares.PercentualVitoriaEmpate
                + analise.Visitante.SimilaresComMando.PercentualVitoriaEmpate
                + analise.Visitante.Ultimos10.PercentualVitoriaEmpate
                + analise.Visitante.Ultimos10ComMando.PercentualVitoriaEmpate
                + analise.Visitante.Ultimos5.PercentualVitoriaEmpate
                + analise.Visitante.Ultimos5ComMando.PercentualVitoriaEmpate
                + analise.Mandante.Global.PercentualDerrotaEmpate
                + analise.Mandante.ComMando.PercentualDerrotaEmpate
                + analise.Mandante.Similares.PercentualDerrotaEmpate
                + analise.Mandante.SimilaresComMando.PercentualDerrotaEmpate
                + analise.Mandante.Ultimos10.PercentualDerrotaEmpate
                + analise.Mandante.Ultimos10ComMando.PercentualDerrotaEmpate
                + analise.Mandante.Ultimos5.PercentualDerrotaEmpate
                + analise.Mandante.Ultimos5ComMando.PercentualDerrotaEmpate) / 16;
            #endregion

            #region Dupla Chance 12
            analise.VitoriaCasaOuFora = (analise.Visitante.Global.PercentualVitoriaOuDerrota
                + analise.Visitante.ComMando.PercentualVitoriaOuDerrota
                + analise.Visitante.Similares.PercentualVitoriaOuDerrota
                + analise.Visitante.SimilaresComMando.PercentualVitoriaOuDerrota
                + analise.Visitante.Ultimos10.PercentualVitoriaOuDerrota
                + analise.Visitante.Ultimos10ComMando.PercentualVitoriaOuDerrota
                + analise.Visitante.Ultimos5.PercentualVitoriaOuDerrota
                + analise.Visitante.Ultimos5ComMando.PercentualVitoriaOuDerrota
                + analise.Mandante.Global.PercentualVitoriaOuDerrota
                + analise.Mandante.ComMando.PercentualVitoriaOuDerrota
                + analise.Mandante.Similares.PercentualVitoriaOuDerrota
                + analise.Mandante.SimilaresComMando.PercentualVitoriaOuDerrota
                + analise.Mandante.Ultimos10.PercentualVitoriaOuDerrota
                + analise.Mandante.Ultimos10ComMando.PercentualVitoriaOuDerrota
                + analise.Mandante.Ultimos5.PercentualVitoriaOuDerrota
                + analise.Mandante.Ultimos5ComMando.PercentualVitoriaOuDerrota) / 16;
            #endregion

            #region Over 1.5
            analise.Over15 = (analise.Visitante.Global.PercentualOver15
                + analise.Visitante.ComMando.PercentualOver15
                + analise.Visitante.Similares.PercentualOver15
                + analise.Visitante.SimilaresComMando.PercentualOver15
                + analise.Visitante.Ultimos10.PercentualOver15
                + analise.Visitante.Ultimos10ComMando.PercentualOver15
                + analise.Visitante.Ultimos5.PercentualOver15
                + analise.Visitante.Ultimos5ComMando.PercentualOver15
                + analise.Mandante.Global.PercentualOver15
                + analise.Mandante.ComMando.PercentualOver15
                + analise.Mandante.Similares.PercentualOver15
                + analise.Mandante.SimilaresComMando.PercentualOver15
                + analise.Mandante.Ultimos10.PercentualOver15
                + analise.Mandante.Ultimos10ComMando.PercentualOver15
                + analise.Mandante.Ultimos5.PercentualOver15
                + analise.Mandante.Ultimos5ComMando.PercentualOver15) / 16;
            #endregion

            #region Over 2.5
            analise.Over25 = (analise.Visitante.Global.PercentualOver25
                + analise.Visitante.ComMando.PercentualOver25
                + analise.Visitante.Similares.PercentualOver25
                + analise.Visitante.SimilaresComMando.PercentualOver25
                + analise.Visitante.Ultimos10.PercentualOver25
                + analise.Visitante.Ultimos10ComMando.PercentualOver25
                + analise.Visitante.Ultimos5.PercentualOver25
                + analise.Visitante.Ultimos5ComMando.PercentualOver25
                + analise.Mandante.Global.PercentualOver25
                + analise.Mandante.ComMando.PercentualOver25
                + analise.Mandante.Similares.PercentualOver25
                + analise.Mandante.SimilaresComMando.PercentualOver25
                + analise.Mandante.Ultimos10.PercentualOver25
                + analise.Mandante.Ultimos10ComMando.PercentualOver25
                + analise.Mandante.Ultimos5.PercentualOver25
                + analise.Mandante.Ultimos5ComMando.PercentualOver25) / 16;
            #endregion

            #region Under 2.5
            analise.Under25 = (analise.Visitante.Global.PercentualUnder25
                + analise.Visitante.ComMando.PercentualUnder25
                + analise.Visitante.Similares.PercentualUnder25
                + analise.Visitante.SimilaresComMando.PercentualUnder25
                + analise.Visitante.Ultimos10.PercentualUnder25
                + analise.Visitante.Ultimos10ComMando.PercentualUnder25
                + analise.Visitante.Ultimos5.PercentualUnder25
                + analise.Visitante.Ultimos5ComMando.PercentualUnder25
                + analise.Mandante.Global.PercentualUnder25
                + analise.Mandante.ComMando.PercentualUnder25
                + analise.Mandante.Similares.PercentualUnder25
                + analise.Mandante.SimilaresComMando.PercentualUnder25
                + analise.Mandante.Ultimos10.PercentualUnder25
                + analise.Mandante.Ultimos10ComMando.PercentualUnder25
                + analise.Mandante.Ultimos5.PercentualUnder25
                + analise.Mandante.Ultimos5ComMando.PercentualUnder25) / 16;
            #endregion

            #region Under 3.5
            analise.Under35 = (analise.Visitante.Global.PercentualUnder35
                + analise.Visitante.ComMando.PercentualUnder35
                + analise.Visitante.Similares.PercentualUnder35
                + analise.Visitante.SimilaresComMando.PercentualUnder35
                + analise.Visitante.Ultimos10.PercentualUnder35
                + analise.Visitante.Ultimos10ComMando.PercentualUnder35
                + analise.Visitante.Ultimos5.PercentualUnder35
                + analise.Visitante.Ultimos5ComMando.PercentualUnder35
                + analise.Mandante.Global.PercentualUnder35
                + analise.Mandante.ComMando.PercentualUnder35
                + analise.Mandante.Similares.PercentualUnder35
                + analise.Mandante.SimilaresComMando.PercentualUnder35
                + analise.Mandante.Ultimos10.PercentualUnder35
                + analise.Mandante.Ultimos10ComMando.PercentualUnder35
                + analise.Mandante.Ultimos5.PercentualUnder35
                + analise.Mandante.Ultimos5ComMando.PercentualUnder35) / 16;
            #endregion

            #region Ambas
            analise.Ambas = (analise.Visitante.Global.PercentualAmbas
                + analise.Visitante.ComMando.PercentualAmbas
                + analise.Visitante.Similares.PercentualAmbas
                + analise.Visitante.SimilaresComMando.PercentualAmbas
                + analise.Visitante.Ultimos10.PercentualAmbas
                + analise.Visitante.Ultimos10ComMando.PercentualAmbas
                + analise.Visitante.Ultimos5.PercentualAmbas
                + analise.Visitante.Ultimos5ComMando.PercentualAmbas
                + analise.Mandante.Global.PercentualAmbas
                + analise.Mandante.ComMando.PercentualAmbas
                + analise.Mandante.Similares.PercentualAmbas
                + analise.Mandante.SimilaresComMando.PercentualAmbas
                + analise.Mandante.Ultimos10.PercentualAmbas
                + analise.Mandante.Ultimos10ComMando.PercentualAmbas
                + analise.Mandante.Ultimos5.PercentualAmbas
                + analise.Mandante.Ultimos5ComMando.PercentualAmbas) / 16;
            #endregion

            GetInsightPlacarExato(analise);
            GetInsightDuplaChance(analise);
            GetInsightGols(analise);
            GetInsightAmbas(analise);

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

                CalculaTotalizadoresGolsMandante(timeDetalhe, item);
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
                CalculaTotalizadoresGolsMandante(timeDetalhe, item);
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
                CalculaTotalizadoresGolsMandante(timeDetalhe, item);
            }
        }

        private void CalculaTotalizadoresGolsMandante(AnaliseTimeDetalhe detalhe, Partida partida)
        {
            switch (partida.GolsCasa + partida.GolsFora)
            {
                case 0:
                case 1:
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
                    break;
            }
            if (partida.GolsCasa > 0 && partida.GolsFora > 0)
                detalhe.TotalAmbas += 1;
        }

        private void GetInsightPlacarExato(Analise analise)
        {
            int sumTrue;
            if (analise.VitoriaCasa >= 60)
            {

                sumTrue = ValidaPlacarExato(analise.Mandante.ComMando.PercentualVitoria, analise.Visitante.ComMando.PercentualDerrota)
                              ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Global.PercentualVitoria, analise.Visitante.Global.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Similares.PercentualVitoria, analise.Visitante.Similares.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.SimilaresComMando.PercentualVitoria, analise.Visitante.SimilaresComMando.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Ultimos10.PercentualVitoria, analise.Visitante.Ultimos10.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Ultimos10ComMando.PercentualVitoria, analise.Visitante.Ultimos10ComMando.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Ultimos5.PercentualVitoria, analise.Visitante.Ultimos5.PercentualDerrota)
                                ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Mandante.Ultimos5ComMando.PercentualVitoria, analise.Visitante.Ultimos5ComMando.PercentualDerrota)
                                ? 1 : 0;
                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA DO MANDANTE!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += $"<li class='text-yellow'>APOSTAR NA VITÓRIA DO MANDANTE!!!</li>";
            }
            if (analise.VitoriaFora >= 60)
            {
                sumTrue = ValidaPlacarExato(analise.Visitante.ComMando.PercentualVitoria, analise.Mandante.ComMando.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Global.PercentualVitoria, analise.Mandante.Global.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Similares.PercentualVitoria, analise.Mandante.Similares.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.SimilaresComMando.PercentualVitoria, analise.Mandante.SimilaresComMando.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos10.PercentualVitoria, analise.Mandante.Ultimos10.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos10ComMando.PercentualVitoria, analise.Mandante.Ultimos10ComMando.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos5.PercentualVitoria, analise.Mandante.Ultimos5.PercentualDerrota)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos5ComMando.PercentualVitoria, analise.Mandante.Ultimos5ComMando.PercentualDerrota)
                        ? 1 : 0;
                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA DO VISITANTE!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NA VITÓRIA DO VISITANTE!!!</li>";
            }
            if (analise.Empate >= 60)
            {
                sumTrue = ValidaPlacarExato(analise.Visitante.ComMando.PercentualEmpate, analise.Mandante.ComMando.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Global.PercentualEmpate, analise.Mandante.Global.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Similares.PercentualEmpate, analise.Mandante.Similares.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.SimilaresComMando.PercentualEmpate, analise.Mandante.SimilaresComMando.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos10.PercentualEmpate, analise.Mandante.Ultimos10.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos10ComMando.PercentualEmpate, analise.Mandante.Ultimos10ComMando.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos5.PercentualEmpate, analise.Mandante.Ultimos5.PercentualEmpate)
                        ? 1 : 0;
                sumTrue += ValidaPlacarExato(analise.Visitante.Ultimos5ComMando.PercentualEmpate, analise.Mandante.Ultimos5ComMando.PercentualEmpate)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO EMPATE!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO EMPATE!!!</li>";
            }
        }
        private void GetInsightDuplaChance(Analise analise)
        {
            int sumTrue;
            if (analise.VitoriaEmpateCasa >= 80)
            {
                sumTrue = ValidaDuplaChance(analise.Mandante.ComMando.PercentualVitoriaEmpate, analise.Visitante.ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Global.PercentualVitoriaEmpate, analise.Visitante.Global.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Similares.PercentualVitoriaEmpate, analise.Visitante.Similares.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.SimilaresComMando.PercentualVitoriaEmpate, analise.Visitante.SimilaresComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Ultimos10.PercentualVitoriaEmpate, analise.Visitante.Ultimos10.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Ultimos10ComMando.PercentualVitoriaEmpate, analise.Visitante.Ultimos10ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Ultimos5.PercentualVitoriaEmpate, analise.Visitante.Ultimos5.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Mandante.Ultimos5ComMando.PercentualVitoriaEmpate, analise.Visitante.Ultimos5ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA/EMPATE DO MANDANTE!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NA VITÓRIA/EMPATE DO MANDANTE!!!</li>";
            }


            if (analise.VitoriaFora >= 80)
            {
                sumTrue = ValidaDuplaChance(analise.Visitante.ComMando.PercentualVitoriaEmpate, analise.Mandante.ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Global.PercentualVitoriaEmpate, analise.Mandante.Global.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Similares.PercentualVitoriaEmpate, analise.Mandante.Similares.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.SimilaresComMando.PercentualVitoriaEmpate, analise.Mandante.SimilaresComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos10.PercentualVitoriaEmpate, analise.Mandante.Ultimos10.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos10ComMando.PercentualVitoriaEmpate, analise.Mandante.Ultimos10ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos5.PercentualVitoriaEmpate, analise.Mandante.Ultimos5.PercentualDerrotaEmpate)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos5ComMando.PercentualVitoriaEmpate, analise.Mandante.Ultimos5ComMando.PercentualDerrotaEmpate)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NA VITÓRIA/EMPATE DO VISITANTE!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NA VITÓRIA/EMPATE DO VISITANTE!!!</li>";
            }

            if (analise.Empate >= 80)
            {
                sumTrue = ValidaDuplaChance(analise.Visitante.ComMando.PercentualVitoriaOuDerrota, analise.Mandante.ComMando.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Global.PercentualVitoriaOuDerrota, analise.Mandante.Global.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Similares.PercentualVitoriaOuDerrota, analise.Mandante.Similares.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.SimilaresComMando.PercentualVitoriaOuDerrota, analise.Mandante.SimilaresComMando.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos10.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos10.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos10ComMando.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos10ComMando.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos5.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos5.PercentualVitoriaOuDerrota)
                        ? 1 : 0;
                sumTrue += ValidaDuplaChance(analise.Visitante.Ultimos5ComMando.PercentualVitoriaOuDerrota, analise.Mandante.Ultimos5ComMando.PercentualVitoriaOuDerrota)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NA VITORIA DA CASA OU FORA!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NA VITORIA DA CASA OU FORA!!!</li>";
            }
        }
        private void GetInsightGols(Analise analise)
        {
            int sumTrue;
            if (analise.Over15 >= 75)
            {
                sumTrue = ValidaGols(analise.Mandante.ComMando.PercentualOver15, analise.Visitante.ComMando.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Global.PercentualOver15, analise.Visitante.Global.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Similares.PercentualOver15, analise.Visitante.Similares.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.SimilaresComMando.PercentualOver15, analise.Visitante.SimilaresComMando.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Ultimos10.PercentualOver15, analise.Visitante.Ultimos10.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Ultimos10ComMando.PercentualOver15, analise.Visitante.Ultimos10ComMando.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Ultimos5.PercentualOver15, analise.Visitante.Ultimos5.PercentualOver15)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Mandante.Ultimos5ComMando.PercentualOver15, analise.Visitante.Ultimos5ComMando.PercentualOver15)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO +1.5 GOLS!!!</li>";
                if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO +1.5 GOLS!!!</li>";
            }

            if (analise.Over25 >= 75)
            {
                sumTrue = ValidaGols(analise.Visitante.ComMando.PercentualOver25, analise.Mandante.ComMando.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Global.PercentualOver25, analise.Mandante.Global.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Similares.PercentualOver25, analise.Mandante.Similares.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.SimilaresComMando.PercentualOver25, analise.Mandante.SimilaresComMando.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10.PercentualOver25, analise.Mandante.Ultimos10.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualOver25, analise.Mandante.Ultimos10ComMando.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5.PercentualOver25, analise.Mandante.Ultimos5.PercentualOver25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualOver25, analise.Mandante.Ultimos5ComMando.PercentualOver25)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO +2.5 GOLS!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO +2.5 GOLS!!!</li>";
            }

            if (analise.Under25 >= 75)
            {
                sumTrue = ValidaGols(analise.Visitante.ComMando.PercentualUnder25, analise.Mandante.ComMando.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Global.PercentualUnder25, analise.Mandante.Global.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Similares.PercentualUnder25, analise.Mandante.Similares.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.SimilaresComMando.PercentualUnder25, analise.Mandante.SimilaresComMando.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10.PercentualUnder25, analise.Mandante.Ultimos10.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualUnder25, analise.Mandante.Ultimos10ComMando.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5.PercentualUnder25, analise.Mandante.Ultimos5.PercentualUnder25)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualUnder25, analise.Mandante.Ultimos5ComMando.PercentualUnder25)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO -2.5 GOLS!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO -2.5 GOLS!!!</li>";
            }

            if (analise.Under35 >= 75)
            {
                sumTrue = ValidaGols(analise.Visitante.ComMando.PercentualUnder35, analise.Mandante.ComMando.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Global.PercentualUnder35, analise.Mandante.Global.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Similares.PercentualUnder35, analise.Mandante.Similares.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.SimilaresComMando.PercentualUnder35, analise.Mandante.SimilaresComMando.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10.PercentualUnder35, analise.Mandante.Ultimos10.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos10ComMando.PercentualUnder35, analise.Mandante.Ultimos10ComMando.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5.PercentualUnder35, analise.Mandante.Ultimos5.PercentualUnder35)
                        ? 1 : 0;
                sumTrue += ValidaGols(analise.Visitante.Ultimos5ComMando.PercentualUnder35, analise.Mandante.Ultimos5ComMando.PercentualUnder35)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO -3.5 GOLS!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO -3.5 GOLS!!!</li>";
            }
        }
        private void GetInsightAmbas(Analise analise)
        {
            int sumTrue;
            if (analise.Ambas >= 65)
            {
                sumTrue = ValidaAmbas(analise.Mandante.ComMando.PercentualAmbas, analise.Visitante.ComMando.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Global.PercentualAmbas, analise.Visitante.Global.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Similares.PercentualAmbas, analise.Visitante.Similares.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.SimilaresComMando.PercentualAmbas, analise.Visitante.SimilaresComMando.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos10.PercentualAmbas, analise.Visitante.Ultimos10.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos10ComMando.PercentualAmbas, analise.Visitante.Ultimos10ComMando.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos5.PercentualAmbas, analise.Visitante.Ultimos5.PercentualAmbas)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos5ComMando.PercentualAmbas, analise.Visitante.Ultimos5ComMando.PercentualAmbas)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO AMBAS MARCAM!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO AMBAS MARCAM!!!</li>";
            }

            if (analise.Ambas <= 35)
            {
                sumTrue = ValidaAmbas(analise.Mandante.ComMando.PercentualAmbas, analise.Visitante.ComMando.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Global.PercentualAmbas, analise.Visitante.Global.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Similares.PercentualAmbas, analise.Visitante.Similares.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.SimilaresComMando.PercentualAmbas, analise.Visitante.SimilaresComMando.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos10.PercentualAmbas, analise.Visitante.Ultimos10.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos10ComMando.PercentualAmbas, analise.Visitante.Ultimos10ComMando.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos5.PercentualAmbas, analise.Visitante.Ultimos5.PercentualAmbas, false)
                        ? 1 : 0;
                sumTrue += ValidaAmbas(analise.Mandante.Ultimos5ComMando.PercentualAmbas, analise.Visitante.Ultimos5ComMando.PercentualAmbas, false)
                        ? 1 : 0;

                if (sumTrue == 8)
                    analise.Insight += "<li class='text-green'>APOSTAR NO AMBAS NÃO MARCAM!!!</li>";
                else if (sumTrue == 7)
                    analise.Insight += "<li class='text-yellow'>APOSTAR NO AMBAS NÃO MARCAM!!!</li>";
            }
        }
        private bool ValidaPlacarExato(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 50 && percentual2 >= 50;
        }
        private bool ValidaDuplaChance(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 70 && percentual2 >= 70;
        }
        private bool ValidaGols(decimal percentual1, decimal percentual2)
        {
            return percentual1 >= 65 && percentual2 >= 65;
        }
        private bool ValidaAmbas(decimal percentual1, decimal percentual2, bool ambasSim = true)
        {
            return ambasSim ? percentual1 >= 55 && percentual2 >= 55
                : percentual1 <= 45 && percentual2 <= 45;
        }
    }
}