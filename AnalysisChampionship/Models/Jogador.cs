using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalysisChampionship.Models
{
    public class Jogador
    {
        public int ID { get; set; }
        public int TimeID { get; set; }
        public string Nome { get; set; }

        public int MediaPontos
            => Stats.Any() ?  (int)Math.Floor((decimal)Stats.Sum(x => x.Pontos) / (decimal)Stats.Count) : 0;
        public int MediaAssistencia
            => Stats.Any() ? (int)Math.Floor(((decimal)Stats.Sum(x => x.Assistencia) / (decimal)Stats.Count)) : 0;
        public int MediaRebotes
            => Stats.Any() ? (int)Math.Floor(((decimal)Stats.Sum(x => x.Rebotes) / (decimal)Stats.Count)) : 0;
        public int MediaTres
            => Stats.Any() ? (int)Math.Floor(((decimal)Stats.Sum(x => x.Tres) / (decimal)Stats.Count)) : 0;
        public int MediaRoubo
            => Stats.Any() ? (int)Math.Floor(((decimal)Stats.Sum(x => x.Roubo) / (decimal)Stats.Count)) : 0;
        public int MediaToco
            => Stats.Any() ? (int)Math.Floor(((decimal)Stats.Sum(x => x.Toco) / (decimal)Stats.Count)) : 0;
        public List<JogadorStats> Stats { get; set; } = new List<JogadorStats>();
        public TimeNBA Time { get; set; }

        public string GetSugestao()
        {
            int partidas = Stats.Count;
            string pontos = "-", assistencia = "-", rebotes = "-", roubos = "-", tocos = "-", tres = "-";
            for (int i = 15; i <= 30; i+= 5)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Pontos >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    pontos = $"{i}+";
                else
                    break;
            }
            
            for (int i = 6; i <= 12; i += 2)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Assistencia >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    assistencia = $"{i}+";
                else
                    break;
            }

            for (int i = 6; i <= 16; i += 2)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Rebotes >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    rebotes = $"{i}+";
                else
                    break;
            }

            for (int i = 2; i <= 5; i++)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Tres >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    tres = $"{i}+";
                else
                    break;
            }

            for (int i = 2; i <= 4; i++)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Toco >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    tocos = $"{i}+";
                else
                    break;
            }

            for (int i = 2; i <= 3; i++)
            {
                decimal percentual = (decimal)Stats.Count(x => x.Roubo >= i) / (decimal)partidas;
                if (percentual >= 0.6m)
                    roubos = $"{i}+";
                else
                    break;
            }

            return $@"<td>{pontos}</td>
                      <td>{rebotes}</td>
                      <td>{assistencia}</td>
                      <td>{tres}</td>
                      <td>{roubos}</td>
                      <td>{tocos}</td>";
        }
    }
}