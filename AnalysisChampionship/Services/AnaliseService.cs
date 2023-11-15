using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Services
{
    public class AnaliseService : IAnaliseService
    {
        private readonly AnaliseRepository _repository;
        private readonly PartidaRepository _partidaRepository;
        public AnaliseService()
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
    }
}