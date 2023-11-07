using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Repository
{
    public static class PaisRepository
    {
        public static List<string> Get()
         => new List<string> { "Alemanha", "Argentina", "Belgica", "Brasil", "Espanha", 
                               "França", "Grecia", "Holanda", "Inglaterra", "Italia", 
                               "Portugal", "Turquia" };
    }
}