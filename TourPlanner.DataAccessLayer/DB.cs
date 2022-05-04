using Npgsql;
using System;
using System.Collections.Generic;
using TourPlanner.Models;
using TourPlanner.Models.Enum;

namespace TourPlanner.DataAccessLayer
{
     class DB
    {
        //steup
        private string databaseConfig;
        private static NpgsqlConnection Connection;
        private static DB instance;

        public static DB getInstance()
        {
            if (instance == null)
            {
                instance = new DB();
            }
            return instance;
        }

        private DB()
        {
            //get the databaseConfig string from the config file in the future
            databaseConfig = "Host=localhost;Username=postgres;Password=tour;Database=postgres";
            Connection = new NpgsqlConnection(databaseConfig);
        }

        private static void Connect()
        {
            Connection.Open();
        }

        private static void Disconnect()
        {
            Connection.Close();
        }

        //functions for the program
        public List<Tour> getToursStatic()
        {

            return new List<Tour>()
            {
                new Tour("TestTour0", "Beschreibung", "Wien1", "Salzburg1", TransportType.running),
                new Tour("TestTour2", "Beschreibung", "Wien2", "Salzburg2", TransportType.running),
                new Tour("TestTour3", "Beschreibung", "Wien3", "Salzburg3", TransportType.running)
            };
        }

        public List<Tour> getTours()
        {
            Connect();
            using (var sql = new NpgsqlCommand("SELECT * FROM tour ORDER BY tourid ASC", Connection))
            {
                NpgsqlDataReader reader = sql.ExecuteReader();

                List<Tour> tourList = null;

                if (reader.HasRows)
                {
                    tourList = new List<Tour>();
                    while (reader.Read())
                    {
                        string? description = UtilityFunctions.checkNull(reader["description"].ToString());
                        


                        tourList.Add(new Tour((string)reader["name"],
                                              description,
                                              (string)reader["from"],
                                              (string)reader["to"],
                                              (TransportType)Enum.Parse(typeof(TransportType), reader["transportType"].ToString())));
                    }
                }

                Disconnect();
                return tourList;
            }
        }

        public int getNextValTour()
        {
            Connect();
            using (var sql = new NpgsqlCommand("SELECT nextval('tour_tourid_seq')", Connection))
            {
                NpgsqlDataReader reader = sql.ExecuteReader();

                reader.Read();
                int nextVal = (int)(long)reader["nextval"];

                Disconnect();
                return nextVal;
            }
        }

    }
}
