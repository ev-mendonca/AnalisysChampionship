using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class JogadorStatsController : Controller
    {
        private readonly JogadorStatsRepository _repository;
        private readonly JogadorRepository _jogadorRepository;
        public JogadorStatsController()
        {
            _repository = new JogadorStatsRepository();
            _jogadorRepository = new JogadorRepository();
        }
        
        public ActionResult Index(int jogadorID)
        {
            TempData["JogadorID"] = jogadorID;
            return View(_repository.Get(jogadorID));
        }
        public ActionResult Editar(int jogadorID)
        {
            ViewData["Jogadores"] = _jogadorRepository.Get();
            JogadorStats model = new JogadorStats { JogadorID = jogadorID};
            return View(model);
        }
        [HttpPost]
        public ActionResult Salvar(JogadorStats jogadorStats)
        {
            _repository.Insert(jogadorStats);
            return RedirectToAction("Editar", new { jogadorID = jogadorStats.JogadorID });
        }

        public ActionResult Deletar(int ID, int jogadorID)
        {
            _repository.Deletar(ID);
            return RedirectToAction("Index", new { jogadorID = jogadorID });
        }
    }
}