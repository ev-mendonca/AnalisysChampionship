using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class AnaliseNBARepository : BaseRepository
    {
        public List<AnaliseTimeNBA> Get()
        {
            List<AnaliseTimeNBA> analises;

            var sql = @" SELECT *
                         FROM  (SELECT tbl.id, tbl.nome,                                   
               Sum(CASE
                     WHEN ( golscasa + golsfora ) > 1 THEN 1
                     ELSE 0
                   END) TotalOver15,
               Sum(CASE
                     WHEN ( golscasa + golsfora ) > 2 THEN 1
                     ELSE 0
                   END) TotalOver25,
               Sum(CASE
                     WHEN ( golscasa + golsfora ) < 3 THEN 1
                     ELSE 0
                   END) TotalUnder25,
               Sum(CASE
                     WHEN ( golscasa + golsfora ) < 4 THEN 1
                     ELSE 0
                   END) TotalUnder35,
               Sum(CASE
                     WHEN golscasa > 0
                          AND golsfora > 0 THEN 1
                     ELSE 0
                   END) TotalAmbas
        FROM   (SELECT t.id,
                       t.nome,
                       p.timecasaid,
                       p.timeforaid,
                       p.pontoscasa,
                       p.pontosfora,
                       p.rebotescasa,
                       p.rebotesfora,
                       p.assistenciascasa,
                       p.assistenciasfora,
                       CASE
                         WHEN t.id = p.timecasaid THEN 'Mandante'
                         ELSE 'Visitante'
                       END AS Mando
                FROM   partidaNBA p
                       INNER JOIN timeNBA t
                               ON t.id = p.timecasaid
                                   OR t.id = p.timeforaid
                ) AS tbl
        GROUP  BY tbl.id, tbl.nome) tbl2
ORDER  BY vitorias DESC,
          empates DESC";

            using (var connection = GetConnection())
            {
                connection.Open();
                analises = connection.Query<AnaliseTimeNBA, AnaliseTimeNBADetalhe,AnaliseTimeNBA>(sql,
                    (time, detalhe) =>
                    {
                        time.Global = detalhe;
                        return time;
                    }, splitOn: "Posicao, Vitorias").ToList();
            }
            return analises;
        }
    }
}