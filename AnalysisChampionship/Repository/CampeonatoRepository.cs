using AnalysisChampionship.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Repository
{
    public class CampeonatoRepository : BaseRepository
    {
        public Campeonato Get(int id)
        {
            Campeonato campeonato;
            var sql = "Select * from Campeonato where ID=@id";

            using (var connection = GetConnection())
            {
                connection.Open();
                campeonato = connection.Query<Campeonato>(sql, new { id = id }).FirstOrDefault();
            }

            return campeonato;
        }

        public List<Campeonato> Get()
        {
            List<Campeonato> campeonatos;
            var sql = "Select * from Campeonato";

            using (var connection = GetConnection())
            {
                connection.Open();
                campeonatos = connection.Query<Campeonato>(sql).ToList();
            }

            return campeonatos;
        }

        public void Insert(Campeonato campeonato)
        {
            var sql = @"INSERT INTO Campeonato
                        (Nome, Pais, Active)
                        VALUES
                        (@Nome, @Pais, 1)";

            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(sql, campeonato);
            }
        }
    }
}