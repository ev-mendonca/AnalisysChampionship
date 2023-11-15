using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class TimeNBARepository : BaseRepository
    {
        public TimeNBA Get(int id)
        {
            TimeNBA time;
            var sql = "Select * from TimeNBA where ID=@id";
            
            using(var connection = GetConnection())
            {
                connection.Open();
                time = connection.Query<TimeNBA>(sql, new { id = id}).FirstOrDefault();
            }

            return time;
        }
        public List<TimeNBA> Get()
        {
            List<TimeNBA> times;
            var sql = @"Select * from TimeNBA t
                        order by t.Nome";

            using (var connection = GetConnection())
            {
                connection.Open();
                times = connection.Query<TimeNBA>(sql).ToList();
            }

            return times;
        }
        public void Insert(TimeNBA Time)
        {
            var sql = @"INSERT INTO Time
                        (Nome)
                        VALUES
                        (@Nome)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, Time);
            }
        }

        public void Excluir(int id)
        {
            var sql = @"DELETE FROM TimeNBA WHERE ID=@id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { id = id });
            }
        }
    }
}