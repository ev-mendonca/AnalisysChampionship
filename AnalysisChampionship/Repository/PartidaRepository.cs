using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class PartidaRepository : BaseRepository
    {
        public void Insert(Partida timeCampeonato)
        {
            var sql = @"INSERT INTO Partida
                        (TimeCasaID, TimeForaID, CampeonatoID, GolsCasa, GolsFora)
                        VALUES
                        (@TimeCasaID, @TimeForaID, @CampeonatoID, @GolsCasa, @GolsFora)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, timeCampeonato);
            }
        }

        public void Deletar(int id)
        {
            var sql = @"DELETE FROM Partida
                        Where ID = @id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { ID = id });
            }
        }

        public List<Partida> GetByCampeonato(int campeonatoID)
        {
            List<Partida> partidas;
            var sql = @"SELECT * FROM Partida P
                        INNER JOIN Time t1 on t1.ID = p.timeCasaID
                        INNER JOIN Time t2 on t2.ID = p.TimeForaID
                        WHERE P.campeonatoID = @campeonatoID
                        order by p.ID desc";

            using (var connection = GetConnection())
            {
                connection.Open();
               partidas = connection.Query<Partida, Time, Time, Partida>(sql,
                    (p, t1, t2) =>
                    {
                        p.TimeCasa = t1;
                        p.TimeFora = t2;
                        return p;
                    }, new { campeonatoID = campeonatoID }).ToList();
            }

            return partidas;
        }

        public List<Partida> GetByCampeonato_Time(int campeonatoID, int timeID)
        {
            List<Partida> partidas;
            var sql = @"Select * from Partida p where CampeonatoID = @campeonatoID and (TimeCasaID = @timeID or TimeForaID = @timeID) order by ID desc";

            using (var connection = GetConnection())
            {
                connection.Open();
                partidas = connection.Query<Partida>(sql, new { campeonatoID = campeonatoID, timeID = timeID }).ToList();
            }

            return partidas;
        }
    }
}