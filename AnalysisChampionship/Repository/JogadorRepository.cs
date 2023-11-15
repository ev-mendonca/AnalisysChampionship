using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class JogadorRepository : BaseRepository
    {
        public void Insert(Jogador jogador)
        {
            var sql = @"INSERT INTO Jogador
                        (TimeID, Nome)
                         VALUES
                         (@TimeID, @Nome)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, jogador);
            }
        }

        public void Deletar(int id)
        {
            var sql = @"DELETE FROM Jogador
                        Where ID = @id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { ID = id });
            }
        }

        public List<Jogador> Get()
        {
            List<Jogador> jogadores;
            var sql = @"Select * from Jogador j
                        INNER JOIN TimeNBA t on j.TimeID = t.ID";

            using (var connection = GetConnection())
            {
                connection.Open();
                jogadores = connection.Query<Jogador,TimeNBA, Jogador>(sql, (jogador, time) =>
                {
                    jogador.Time = time;
                    return jogador;
                }).ToList();
            }

            return jogadores;
        }

        public List<Jogador> GetByTimes(params int[] ids)
        {
            List<Jogador> jogadores;
            var sql = @"Select * from Jogador j
                        INNER JOIN JogadorStats js on j.ID = js.JogadorID
                        WHERE j.timeId in @ids";

            using (var connection = GetConnection())
            {
                connection.Open();
                jogadores = connection.Query<Jogador, JogadorStats, Jogador>(sql, (jogador, stats) =>
                {
                    jogador.Stats = jogador.Stats ?? new List<JogadorStats>();
                    jogador.Stats.Add(stats);
                    return jogador;
                }, new { ids = ids }).ToList();
            }

            return jogadores.GroupBy(x=>x.ID).Select(x=> new Jogador
            {
                ID = x.Key,
                Nome = x.FirstOrDefault().Nome,
                Stats = x.SelectMany(y=>y.Stats).ToList()
            }).ToList();
        }
    }
}