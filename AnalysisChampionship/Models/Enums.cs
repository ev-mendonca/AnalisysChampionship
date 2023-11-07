using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public enum enumTipoCalculo
    {
        VitoriaCasa,
        VitoriaFora,
        Empate,
        VitoriaEmpateCasa,
        VitoriaEmpateFora,
        VitoriaCasaFora,
        Over15,
        Over25,
        Under25,
        Under35,
        Ambas

    }
    public enum enumTipoResultado
    {
        Vitoria,
        Empate,
        Derrota,
        VitoriaEmpate,
        DerrotaEmpate,
        VitoriaDerrota,
        Over15,
        Over25,
        Under25,
        Under35,
        Ambas
    }
}