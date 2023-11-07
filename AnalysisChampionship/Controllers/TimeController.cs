using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class TimeController : Controller
    {
        private readonly TimeRepository _repository;
        public TimeController()
        {
            _repository = new TimeRepository();
        }
        public ActionResult Index()
        {
            Times times = new Times(_repository.Get());
            return View(times);
        }
        public ActionResult Editar(string pais = "")
        {
            if (string.IsNullOrEmpty(pais))
                return RedirectToAction("Editar", new { pais = "Brasil" });

            return View(new Time { Pais = pais });
        }
        public ActionResult EditarVarios(string pais = "")
        {
            if (string.IsNullOrEmpty(pais))
                return RedirectToAction("EditarVarios", new { pais = "Brasil" });

            return View(new Time { Pais = pais });
        }
        [HttpPost]
        public ActionResult Salvar(Time time)
        {
            if (time.ID == 0)
                _repository.Insert(time);
            return RedirectToAction("Editar", new { pais = time.Pais });
        }

        [HttpPost]
        public ActionResult SalvarVarios(string times, string pais)
        {
            foreach (var time in times.Split('\n'))
            {
                _repository.Insert(new Time(time.Replace("\r",""), pais));
            }
            return RedirectToAction("Editar", new { pais = pais });
        }

        public ActionResult Excluir(int id)
        {
            _repository.Excluir(id);
            return RedirectToAction("Index");
        }
    }
}