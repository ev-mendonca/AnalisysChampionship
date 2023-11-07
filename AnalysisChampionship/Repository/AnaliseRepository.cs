using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class AnaliseRepository : BaseRepository
    {
        public List<AnaliseTime> Get(int campeonatoID)
        {
            List<AnaliseTime> analises;

            var sql = @" SELECT Row_number() OVER (ORDER BY vitorias DESC, 
                                                            empates DESC) Posicao, *
                         FROM   (SELECT tbl.id, tbl.nome,
                                   Sum(CASE WHEN ( mando = 'Mandante' AND golscasa > golsfora )
                                              OR ( mando = 'Visitante' AND golscasa < golsfora ) 
                                            THEN 1 ELSE 0 END) Vitorias,
                                   Sum(CASE WHEN ( mando = 'Mandante' AND golscasa < golsfora )
                                              OR ( mando = 'Visitante' AND golscasa > golsfora ) 
                                            THEN 1 ELSE 0 END) Derrotas,
                                   Sum(CASE
                     WHEN golscasa = golsfora THEN 1
                     ELSE 0
                   END) Empates,
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
                       p.golscasa,
                       p.golsfora,
                       CASE
                         WHEN t.id = p.timecasaid THEN 'Mandante'
                         ELSE 'Visitante'
                       END AS Mando
                FROM   partida p
                       INNER JOIN time t
                               ON t.id = p.timecasaid
                                   OR t.id = p.timeforaid
                WHERE  p.campeonatoid = @campeonatoID) AS tbl
        GROUP  BY tbl.id, tbl.nome) tbl2
ORDER  BY vitorias DESC,
          empates DESC";

            using (var connection = GetConnection())
            {
                connection.Open();
                analises = connection.Query<AnaliseTime, AnaliseTimeDetalhe,AnaliseTime>(sql,
                    (time, detalhe) =>
                    {
                        time.Global = detalhe;
                        return time;
                    }, new { campeonatoID = campeonatoID }, splitOn: "Posicao, Vitorias").ToList();
            }
            return analises;
        }
    }
}