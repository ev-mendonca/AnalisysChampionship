using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class TimeCampeonatoController : Controller
    {
        private readonly TimeCampeonatoRepository _repository;
        public TimeCampeonatoController()
        {
            _repository = new TimeCampeonatoRepository();
        }
        public ActionResult Editar(int id)
        {
            ViewData["Times"] = new TimeRepository().GetHabilitadosParaCampeonato(id);
            TimeCampeonato model = new TimeCampeonato
            {
                CampeonatoID = id,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(TimeCampeonato timeCampeonato)
        {
            _repository.Insert(timeCampeonato);
            return RedirectToAction("Editar", "TimeCampeonato", new { id = timeCampeonato.CampeonatoID });
        }


        public ActionResult Excluir(int timeID, int campeonatoID)
        {
            _repository.Excluir(timeID, campeonatoID);
            return RedirectToAction("Participantes", "Campeonato", new { id = campeonatoID });
        }
    }
}