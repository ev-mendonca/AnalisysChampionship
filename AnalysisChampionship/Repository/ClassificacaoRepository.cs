using AnalysisChampionship.Models;
using Dapper;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class ClassificacaoRepository : BaseRepository
    {
        public Classificacao Get(int campeonatoID)
        {
            Classificacao classificacao = new Classificacao();
            classificacao.Competicao = new CampeonatoRepository().Get(campeonatoID).Nome;
            
            var sql = @"SELECT tbl.Nome,
                                 SUM(CASE WHEN (Mando = 'Mandante' AND GolsCasa > GolsFora) 
                                            OR (Mando = 'Visitante' AND GolsCasa < GolsFora) 
                                            THEN 1 ELSE 0 END) Vitorias,
                                 SUM(CASE WHEN (Mando = 'Mandante' AND GolsCasa < GolsFora) 
                                            OR (Mando = 'Visitante' AND GolsCasa > GolsFora) 
                                            THEN 1 ELSE 0 END) Derrotas,
                                 Sum(CASE WHEN GolsCasa = GolsFora THEN 1 ELSE 0 END) Empates
                            FROM (SELECT t.ID, t.Nome, p.TimeCasaID, p.TimeForaID, 
                                         p.GolsCasa, p.GolsFora,
                                         CASE WHEN t.ID = p.TimeCasaID 
                                              THEN 'Mandante' ELSE 'Visitante' END AS Mando
                                  FROM Partida p
                                  INNER JOIN Time t ON t.ID = p.TimeCasaID OR t.ID = p.TimeForaID
                                  WHERE p.campeonatoID = @campeonatoID) AS tbl
                            GROUP BY tbl.Nome
                            ORDER BY Vitorias DESC, Empates DESC";

            using (var connection = GetConnection())
            {
                connection.Open();
                classificacao.Times = connection.Query<ClassificacaoTime>(sql,new { campeonatoID = campeonatoID }).ToList();
            }

            return classificacao;
        }
    }
}