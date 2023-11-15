using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class PartidaNBARepository : BaseRepository
    {
        public void Insert(PartidaNBA timeCampeonato)
        {
            var sql = @"INSERT INTO PartidaNBA
                        (TimeCasaID,TimeForaID,PontosCasa,PontosFora,RebotesCasa,RebotesFora,AssistenciasCasa,AssistenciasFora)
                         VALUES
                         (@TimeCasaID,@TimeForaID,@PontosCasa,@PontosFora,@RebotesCasa,@RebotesFora,@AssistenciasCasa,@AssistenciasFora)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, timeCampeonato);
            }
        }

        public void Deletar(int id)
        {
            var sql = @"DELETE FROM PartidaNBA
                        Where ID = @id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { ID = id });
            }
        }

        public List<PartidaNBA> GetByTime(int timeID)
        {
            List<PartidaNBA> partidas;
            var sql = @"Select * from PartidaNBA p WHERE (TimeCasaID = @timeID or TimeForaID = @timeID) order by ID desc";

            using (var connection = GetConnection())
            {
                connection.Open();
                partidas = connection.Query<PartidaNBA>(sql, new {timeID = timeID }).ToList();
            }

            return partidas;
        }
        public List<PartidaNBA> Get()
        {
            List<PartidaNBA> partidas;
            var sql = @"Select * from PartidaNBA p
                        INNER JOIN TimeNBA t1 on p.TimeCasaID = t1.ID
                        INNER JOIN TimeNBA t2 on p.TimeForaID = t2.ID
                        order by p.ID desc";

            using (var connection = GetConnection())
            {
                connection.Open();
                partidas = connection.Query<PartidaNBA, TimeNBA, TimeNBA, PartidaNBA>(sql,
                    (partida, mandante, visitante) => 
                    {
                        partida.TimeCasa = mandante;
                        partida.TimeFora = visitante;
                        return partida;
                    }).ToList();
            }

            return partidas;
        }
    }
}