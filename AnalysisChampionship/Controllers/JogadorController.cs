using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class JogadorController : Controller
    {
        private readonly JogadorRepository _repository;
        private readonly TimeNBARepository _timeNBARepository;
        public JogadorController()
        {
            _repository = new JogadorRepository();
            _timeNBARepository = new TimeNBARepository();
        }
        
        public ActionResult Index()
        {
            return View(_repository.Get());
        }
        public ActionResult Editar()
        {
            ViewData["Times"] = _timeNBARepository.Get();
            Jogador model = new Jogador();
            return View(model);
        }
        [HttpPost]
        public ActionResult Salvar(Jogador jogador)
        {
            _repository.Insert(jogador);
            return RedirectToAction("Editar");
        }

        public ActionResult Deletar(int ID)
        {
            _repository.Deletar(ID);
            return RedirectToAction("Index");
        }
    }
}