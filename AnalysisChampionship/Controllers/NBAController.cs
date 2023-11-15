using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class NBAController : Controller
    {
        private readonly PartidaNBARepository _repository;
        public NBAController()
        {
            _repository = new PartidaNBARepository();
        }
        
        public ActionResult Index()
        {
            var participantes = _repository.Get();
            return View(participantes);
        }
        public ActionResult Analise()
        {
            var times = new TimeNBARepository().Get();
            return View(times);
        }
    }
}