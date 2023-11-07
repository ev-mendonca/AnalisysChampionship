using AnalysisChampionship.Models;
using AnalysisChampionship.Repository;
using AnalysisChampionship.Services;
using AnalysisChampionship.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalysisChampionship.Controllers
{
    public class HomeController : Controller
    {
        IAnaliseService service;
        public HomeController()
        {
            service = new AnaliseServiceMod2();
        }
        public ActionResult Index(int timeCasaID = 0, int timeForaID = 0, int campeonatoID = 0)
        {
            Analise analise;
            if (timeCasaID > 0 && timeForaID > 0 && campeonatoID > 0)
            {
                analise = service.GetAnalise(campeonatoID, timeCasaID, timeForaID);
            }
            else
                analise = new Analise(new AnaliseTime(), new AnaliseTime());
            return View(analise);
        }

    }
}