using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class PartidaController : Controller
    {
        private readonly PartidaRepository _repository;
        private readonly PartidaNBARepository _NBARepository;
        private readonly TimeRepository _timeRepository;
        private readonly TimeNBARepository _timeNBARepository;
        private readonly CampeonatoRepository _campeonatoRepository;
        public PartidaController()
        {
            _repository = new PartidaRepository();
            _timeRepository = new TimeRepository();
            _campeonatoRepository = new CampeonatoRepository();
            _NBARepository = new PartidaNBARepository();
            _timeNBARepository = new TimeNBARepository();
        }
        public ActionResult Editar(int id)
        {
            ViewData["Times"] = new TimeRepository().GetByCampeonato(id);
            Partida model = new Partida
            {
                CampeonatoID = id,
            };
            return View(model);
        }

        public ActionResult EditarNBA()
        {
            ViewData["Times"] = _timeNBARepository.Get();
            PartidaNBA model = new PartidaNBA();
            return View(model);
        }

        public ActionResult EditarVarios(int id)
        {
            Partida model = new Partida
            {
                CampeonatoID = id,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(Partida partida)
        {
            _repository.Insert(partida);
            return RedirectToAction("Editar", "Partida", new { id = partida.CampeonatoID });
        }

        [HttpPost]
        public ActionResult SalvarNBA(PartidaNBA partida)
        {
            _NBARepository.Insert(partida);
            return RedirectToAction("EditarNBA", "Partida");
        }

        [HttpPost]
        public ActionResult SalvarVarios(string partidas, int campeonatoID)
        {
            string[] partidaList = partidas.Replace("\r","").Split('\n')
                .Where(x=>x != "FT" && !x.Contains("/") && !x.Contains(":")).ToArray();
            for (int i = 0; i + 5 <= partidaList.Length; i+=6)
            {
                Campeonato c = _campeonatoRepository.Get(campeonatoID);
                Partida p = new Partida
                {
                    CampeonatoID = campeonatoID,
                    GolsCasa = Convert.ToInt32(partidaList[i + 4].Replace("\r", "")),
                    GolsFora = Convert.ToInt32(partidaList[i + 5].Replace("\r", "")),
                    TimeCasaID = _timeRepository.GetByNome(partidaList[i + 1].Replace("\r", ""), c.Pais).ID,
                    TimeForaID = _timeRepository.GetByNome(partidaList[i + 3].Replace("\r", ""), c.Pais).ID
                };
                _repository.Insert(p);
            }
            
            return RedirectToAction("EditarVarios", "Partida", new { id = campeonatoID });
        }

        [HttpGet]
        public ActionResult Deletar(int id, int campeonatoId)
        {
            _repository.Deletar(id);
            return RedirectToAction("Partidas", "Campeonato", new { id = campeonatoId });
        }

        [HttpGet]
        public ActionResult DeletarNBA(int id, int campeonatoId)
        {
            _NBARepository.Deletar(id);
            return RedirectToAction("Index", "NBA");
        }
    }
}