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
            //databaseConfig = "Host=localhost;Username=postgres;Password=tour;Database=postgres";
            databaseConfig = ConfigAccess.getDatabaseString();
            Connection = new NpgsqlConnection(databaseConfig);
            
        }

        private static void Connect()
        {
            try
            {
                Connection.Open();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                //log error in future
                Environment.Exit(0);
            }
            catch (Npgsql.PostgresException ex)
            {
                //log error in future
                Environment.Exit(0);
            }   
        }

        private static void Disconnect()
        {
            Connection.Close();
        }

        //functions for the program
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
                        


                        tourList.Add(new Tour((int)reader["tourid"], (string)reader["name"], description, (string)reader["fromDB"], (string)reader["toDB"], (TransportType)Enum.Parse(typeof(TransportType), reader["transportType"].ToString()), (double)reader["distance"], (string)reader["time"]));
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
        
        public void addTourToDB(Tour tour)
        {
            Connect();
            using (var sql = new NpgsqlCommand("INSERT INTO tour (tourid, name, description, fromdb, todb, transporttype, distance, time) VALUES (@tourid, @name, @description, @from, @to, @transportType, @distance, @time)", Connection))
            {
                sql.Parameters.AddWithValue("tourid", tour.ID);
                sql.Parameters.AddWithValue("name", tour.Name);
                sql.Parameters.AddWithValue("description", tour.Description);
                sql.Parameters.AddWithValue("from", tour.From);
                sql.Parameters.AddWithValue("to", tour.To);
                sql.Parameters.AddWithValue("transportType", tour.TransportType.ToString());
                sql.Parameters.AddWithValue("distance", tour.TourDistance);
                sql.Parameters.AddWithValue("time", tour.EstimatedTime);
                sql.ExecuteNonQuery();
            }
            Disconnect();
        }
        

        /*
        public void addTourToDB(Tour tour)
        {
            Connect();
            using (var sql = new NpgsqlCommand("INSERT INTO tour (name, fromdb, todb, transporttype) VALUES (@_name,  @_from, @_to, @_type)", Connection))
            {
                sql.Parameters.AddWithValue("_name", "fuck");
                sql.Parameters.AddWithValue("_from", "the");
                sql.Parameters.AddWithValue("_to", "fuckers");
                sql.Parameters.AddWithValue("_type", "fuckers");
                sql.ExecuteNonQuery();
            }
            Disconnect();
        }
        */
    }
}
