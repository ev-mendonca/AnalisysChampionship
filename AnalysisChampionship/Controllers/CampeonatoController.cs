using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class CampeonatoController : Controller
    {
        private readonly CampeonatoRepository _repository;
        public CampeonatoController()
        {
            _repository = new CampeonatoRepository();
        }
        
        public ActionResult Index()
        {
            Campeonatos campeonatos = new Campeonatos(_repository.Get());
            return View(campeonatos);
        }

        public ActionResult Editar(string pais = "")
        {
            if (string.IsNullOrEmpty(pais))
                return RedirectToAction("Editar", new { pais = "Brasil" });

            return View(new Campeonato { Pais = pais });
        }

        public ActionResult Participantes(int id)
        {
            Campeonato camp = _repository.Get(id);
            camp.Participantes = new TimeRepository().GetByCampeonato(id);
            return View(camp);
        }

        public ActionResult Partidas(int id)
        {
            Campeonato camp = _repository.Get(id);
            camp.Partidas = new PartidaRepository().GetByCampeonato(id);
            return View(camp);
        }
        public ActionResult Classificacao(int id)
        {
            Classificacao classificacao = new ClassificacaoRepository().Get(id);
            classificacao.Times = classificacao.Times.OrderByDescending(x => x.Pontuacao)
                                                     .ThenByDescending(x => x.Vitorias).ToList();
            return View(classificacao);
        }

        public ActionResult Analise(int id)
        {
            Campeonato campeonato = _repository.Get(id);
            ViewData["Times"] = new TimeRepository().GetByCampeonato(id);
            return View(campeonato);
        }


        [HttpPost]
        public ActionResult Salvar(Campeonato campeonato)
        {
            if (campeonato.ID == 0)
                _repository.Insert(campeonato);

            return RedirectToAction("Index",new {pais = campeonato.Pais });
        }
    }
}