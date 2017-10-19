using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GameStore.Domain.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Concrete
{
    class StorageProcedures
    {
        static string connectionString = @"Data Source=SQL6001.SmarterASP.NET;Initial Catalog=DB_A2C34A_GameStore;User Id=DB_A2C34A_GameStore_admin;Password=q1o0o0q1h6;";

        public static IEnumerable<Game> GetUsers()
        {
            string sqlExpression = "sp_GetGames";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                List<Game> games = new List<Game>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Game game = new Game();

                        game.GameId = reader.GetInt32(0);
                        game.Name = reader.GetString(1);
                        game.Description = reader.GetString(2);
                        game.Category = reader.GetString(3);
                        game.Price = reader.GetDecimal(4);

                        games.Add(game);
                    }
                }
                reader.Close();
                return games;
            }
        }
    }
}
