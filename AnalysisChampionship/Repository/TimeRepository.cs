using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class TimeRepository : BaseRepository
    {
        public Time Get(int id)
        {
            Time time;
            var sql = "Select * from Time where ID=@id";
            
            using(var connection = GetConnection())
            {
                connection.Open();
                time = connection.Query<Time>(sql, new { id = id}).FirstOrDefault();
            }

            return time;
        }

        public Time GetByNome(string nome, string pais)
        {
            Time time;
            var sql = "Select * from Time where nome = @nome and pais = @pais";

            using (var connection = GetConnection())
            {
                connection.Open();
                time = connection.Query<Time>(sql, new { nome = nome, pais = pais }).FirstOrDefault();
            }

            return time;
        }

        public List<Time> GetHabilitadosParaCampeonato(int campeonatoId)
        {
            List<Time> times;
            var sql = @"Select t.* from Time t
                        INNER JOIN Campeonato c ON c.Pais = t.Pais
                        LEFT JOIN TimeCampeonato tc ON tc.timeID = t.id
                        where tc.timeID is null and c.ID = @campeonatoId";

            using (var connection = GetConnection())
            {
                connection.Open();
                times = connection.Query<Time>(sql, new { campeonatoId = campeonatoId }).ToList();
            }

            return times;
        }

        public List<Time> GetByCampeonato(int campeonatoId)
        {
            List<Time> times;
            var sql = @"Select t.* from Time t
                        INNER JOIN TimeCampeonato tc ON t.ID = tc.TimeID
                        WHERE tc.CampeonatoID = @campeonatoId
                        order by t.Nome";

            using (var connection = GetConnection())
            {
                connection.Open();
                times = connection.Query<Time>(sql, new { campeonatoId = campeonatoId }).ToList();
            }

            return times;
        }

        public List<Time> Get()
        {
            List<Time> times;
            var sql = "Select * from Time";

            using (var connection = GetConnection())
            {
                connection.Open();
                times = connection.Query<Time>(sql).ToList();
            }

            return times;
        }

        public void Insert(Time Time)
        {
            var sql = @"INSERT INTO Time
                        (Nome, Pais, Active)
                        VALUES
                        (@Nome, @Pais, 1)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, Time);
            }
        }

        public void Excluir(int id)
        {
            var sql = @"DELETE FROM Time WHERE ID=@id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { id = id });
            }
        }
    }
}