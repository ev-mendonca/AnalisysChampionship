using AnalysisChampionship.Models;
using Dapper;

namespace AnalysisChampionship.Repository
{
    public class TimeCampeonatoRepository : BaseRepository
    {
        public void Insert(TimeCampeonato timeCampeonato)
        {
            var sql = @"INSERT INTO TimeCampeonato
                        (TimeID, CampeonatoID)
                        VALUES
                        (@TimeID, @CampeonatoID)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, timeCampeonato);
            }
        }
        public void Excluir(int timeID, int campeonatoID)
        {
            var sql = @"DELETE FROM TimeCampeonato WHERE timeid = @timeID and campeonatoid = @campeonatoID";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { timeid = timeID, campeonatoid = campeonatoID });
            }
        }
    }
}