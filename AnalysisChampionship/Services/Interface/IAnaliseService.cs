using AnalysisChampionship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisChampionship.Services.Interface
{
    interface IAnaliseService
    {
        Analise GetAnalise(int campeonatoID, int mandanteID, int visitanteID);
    }
}
