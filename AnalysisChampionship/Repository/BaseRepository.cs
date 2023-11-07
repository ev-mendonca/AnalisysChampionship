using System.Data.SqlClient;

namespace AnalysisChampionship.Repository
{
    public class BaseRepository
    {

        protected SqlConnection GetConnection()
            => new SqlConnection("Server=localhost\\SQLEXPRESS;Database=AnalysisChampionship;Connection timeout=30;Integrated Security=SSPI;");
    }
}