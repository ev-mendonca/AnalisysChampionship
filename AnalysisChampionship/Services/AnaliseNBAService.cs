using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Services
{
    public class AnaliseNBAService
    {
        private readonly TimeNBARepository _repository;
        private readonly PartidaNBARepository _partidaRepository;
        private readonly JogadorRepository _joogadorRepository;
        public AnaliseNBAService()
        {
            _repository = new TimeNBARepository();
            _partidaRepository = new PartidaNBARepository();
            _joogadorRepository = new JogadorRepository();
        }
        public AnaliseNBA GetAnalise(int timeCasaID, int timeForaID)
        {
            var analiseMandante = new AnaliseTimeNBA(_repository.Get(timeCasaID));
            var analiseVisitante = new AnaliseTimeNBA(_repository.Get(timeForaID));

            var partidasMandante = _partidaRepository.GetByTime(timeCasaID);

            var partidasVisitante = _partidaRepository.GetByTime(timeForaID);

            AnaliseNBA analise = new AnaliseNBA(analiseMandante, analiseVisitante);

            analise.Jogadores = _joogadorRepository.GetByTimes(timeCasaID, timeForaID);

            //Global
            CalculaTotalizadoresSemMando(analise.Mandante.Global, partidasMandante, timeCasaID);
            CalculaTotalizadoresSemMando(analise.Visitante.Global, partidasVisitante, timeForaID);
            //Com Mando
            CalculaTotalizadoresMandante(analise.Mandante.ComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID));
            CalculaTotalizadoresVisitante(analise.Visitante.ComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID));
            //Ultimos 5 Com Mando
            CalculaTotalizadoresMandante(analise.Mandante.Ultimos5ComMando, partidasMandante.Where(x => x.TimeCasaID == timeCasaID).Take(5));
            CalculaTotalizadoresVisitante(analise.Visitante.Ultimos5ComMando, partidasVisitante.Where(x => x.TimeForaID == timeForaID).Take(5));
            //Ultimos 10
            CalculaTotalizadoresSemMando(analise.Mandante.Ultimos10, partidasMandante.Take(10), timeCasaID);
            CalculaTotalizadoresSemMando(analise.Visitante.Ultimos10, partidasVisitante.Take(10), timeForaID);
            return analise;
        }

        private void CalculaTotalizadoresSemMando(AnaliseTimeNBADetalhe timeDetalhe, IEnumerable<PartidaNBA> partidas, int timeID)
        {
            foreach (var item in partidas)
            {
                if (item.TimeCasaID == timeID)
                {
                    CalculaTotalizadorMandante(timeDetalhe, item);
                }
                else
                {
                    CalculaTotalizadorVisitante(timeDetalhe, item);
                }
            }
        }

        private void CalculaTotalizadoresMandante(AnaliseTimeNBADetalhe timeDetalhe, IEnumerable<PartidaNBA> partidas)
        {
            foreach (var item in partidas)
            {
                CalculaTotalizadorMandante(timeDetalhe, item);
            }
        }

        private void CalculaTotalizadoresVisitante(AnaliseTimeNBADetalhe timeDetalhe, IEnumerable<PartidaNBA> partidas)
        {
            foreach (var item in partidas)
            {
                CalculaTotalizadorVisitante(timeDetalhe, item);    
            }
        }

        private void CalculaTotalizadorMandante(AnaliseTimeNBADetalhe timeDetalhe, PartidaNBA partida)
        {

            timeDetalhe.Partidas++;
            timeDetalhe.PontosFeitos += partida.PontosCasa;
            timeDetalhe.PontosSofridos += partida.PontosFora;
            timeDetalhe.RebotesFavor += partida.RebotesCasa;
            timeDetalhe.RebotesContra += partida.RebotesFora;
            timeDetalhe.AssistenciasFavor += partida.AssistenciasCasa;
            timeDetalhe.AssistenciasContra += partida.AssistenciasFora;

        }
        private void CalculaTotalizadorVisitante(AnaliseTimeNBADetalhe timeDetalhe, PartidaNBA partida)
        {

            timeDetalhe.Partidas++;
            timeDetalhe.PontosFeitos += partida.PontosFora;
            timeDetalhe.PontosSofridos += partida.PontosCasa;
            timeDetalhe.RebotesFavor += partida.RebotesFora;
            timeDetalhe.RebotesContra += partida.RebotesCasa;
            timeDetalhe.AssistenciasFavor += partida.AssistenciasFora;
            timeDetalhe.AssistenciasContra += partida.AssistenciasCasa;
        }


    }
}