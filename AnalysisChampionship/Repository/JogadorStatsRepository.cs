using AnalysisChampionship.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisChampionship.Repository
{
    public class JogadorStatsRepository : BaseRepository
    {
        public void Insert(JogadorStats jogador)
        {
            var sql = @"INSERT INTO JogadorStats
                        (JogadorID, Pontos, Rebotes, Assistencia, Roubo, Toco, Tres)
                         VALUES
                         (@JogadorID, @Pontos, @Rebotes, @Assistencia, @Roubo, @Toco, @Tres)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, jogador);
            }
        }

        public void Deletar(int id)
        {
            var sql = @"DELETE FROM JogadorStats
                        Where ID = @id";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, new { ID = id });
            }
        }

        public List<JogadorStats> Get(int jogadorID)
        {
            List<JogadorStats> jogadores;
            var sql = @"Select * from JogadorStats js
                        INNER JOIN Jogador j on j.ID = js.JogadorID
                        WHERE js.JogadorID = @jogadorID
                        order by 1 desc";

            using (var connection = GetConnection())
            {
                connection.Open();
                jogadores = connection.Query<JogadorStats,Jogador, JogadorStats>(sql, (jogadorStats, jogador) =>
                {
                    jogadorStats.Jogador = jogador;
                    return jogadorStats;
                }, new { jogadorID = jogadorID }).ToList();
            }

            return jogadores;
        }
    }
}