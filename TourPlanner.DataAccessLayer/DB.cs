using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Models.Enum;
using Microsoft.Extensions.Configuration;
using TourPlanner.Logging;

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
            Logger.Info("Database connection was setup");

        }

        private static void Connect()
        {
            try
            {
                Connection.Open();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("FEHLER: " + ex.Message + "\n\n" +
                    "Bitte überprüfen Sie ob es Probleme mit der Datenbank, oder Fehler im config file gibt");

                Logger.Fatal("Database connection error\nMessage: "+ex.Message);

                Environment.Exit(0);
            }
            catch (Npgsql.PostgresException ex)
            {
                MessageBox.Show("FEHLER: " + ex.Message + "\n\n" +
                    "Bitte überprüfen Sie ob es Probleme mit der Datenbank, oder Fehler im config file gibt");

                Logger.Fatal("Database connection error\nMessage: " + ex.Message);

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

                foreach(Tour tour in tourList)
                {
                    tour.setLogs(getTourLogs((int)tour.ID));
                }

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
        public int getTourLogAmountTotal()
        {
            Connect();
            using (var sql = new NpgsqlCommand("SELECT count(*) AS countVal FROM tourlogs", Connection))
            {
                NpgsqlDataReader reader = sql.ExecuteReader();

                reader.Read();
                int logAmount = (int)(long)reader["countVal"];

                Disconnect();
                return logAmount;
            }
        }

        public void addTourToDB(Tour tour)
        {
            Connect();
            using (var sql = new NpgsqlCommand("INSERT INTO tour (tourid, name, description, fromdb, todb, transporttype, distance, time) VALUES (@tourid, @name, @description, @from, @to, @transportType, @distance, @time)", Connection))
            {
                sql.Parameters.AddWithValue("tourid", tour.ID);
                sql.Parameters.AddWithValue("name", tour.Name);
                sql.Parameters.AddWithValue("description", tour.Description.ToString());
                sql.Parameters.AddWithValue("from", tour.From);
                sql.Parameters.AddWithValue("to", tour.To);
                sql.Parameters.AddWithValue("transportType", tour.TransportType.ToString());
                sql.Parameters.AddWithValue("distance", tour.TourDistance);
                sql.Parameters.AddWithValue("time", tour.EstimatedTime);
                sql.ExecuteNonQuery();
            }
            Disconnect();
        }        
        public void addLogToDB(TourLogs log)
        {
            Connect();
            using (var sql = new NpgsqlCommand("INSERT INTO tourlogs (comment, difficulty, totaltime, rating, touridfk) VALUES (@comment, @difficulty, @totaltime, @rating, @touridfk)", Connection))
            {
                sql.Parameters.AddWithValue("comment", log.Comment);
                sql.Parameters.AddWithValue("difficulty", log.Difficulty);
                sql.Parameters.AddWithValue("totaltime", log.TotalTime);
                sql.Parameters.AddWithValue("rating", log.Rating);
                sql.Parameters.AddWithValue("touridfk", log.TourID);
                sql.ExecuteNonQuery();
            }
            Disconnect();
        }

        public List<TourLogs> getTourLogs(int TourID)
        {
            Connect();
            using (var sql = new NpgsqlCommand("SELECT * FROM tourlogs WHERE touridfk = @tourid ORDER BY logid ASC", Connection))
            {
                sql.Parameters.AddWithValue("tourid", TourID);
                NpgsqlDataReader reader = sql.ExecuteReader();

                List<TourLogs> tourLogList = new List<TourLogs>();

                if (reader.HasRows)
                {
                    //tourLogList = new List<TourLogs>();
                    while (reader.Read())
                    {
                        string? comment = UtilityFunctions.checkNull(reader["comment"].ToString());



                        tourLogList.Add(new TourLogs(reader["logtime"].ToString(), comment, (int)reader["difficulty"], (int)reader["totaltime"], (int)reader["rating"], (int)reader["logid"], (int)reader["touridfk"]));
                    }
                }

                Disconnect();
                return tourLogList;
            }
        }

        public void deleteTour(int tourid)
        {
            Connect();
            using (var sql = new NpgsqlCommand("DELETE FROM tour WHERE tourid = @tID", Connection))
            {
                sql.Parameters.AddWithValue("tID", tourid);
                sql.ExecuteNonQuery();
            }
            Disconnect();

            Logger.Info("Tour with ID:"+tourid+" was deleted");
        }        
        public void deleteTourLog(int logid)
        {
            Connect();
            using (var sql = new NpgsqlCommand("DELETE FROM tourlogs WHERE logid = @lID", Connection))
            {
                sql.Parameters.AddWithValue("lID", logid);
                sql.ExecuteNonQuery();
            }
            Disconnect();

            Logger.Info("Tour with ID:" + logid + " was deleted");
        }

        public void changeTourDB(Tour tour)
        {
            Connect();
            using (var sql = new NpgsqlCommand("UPDATE tour SET name = @name, description = @description, fromdb = @from, todb = @to, transporttype = @transportType, distance = @distance, time = @time WHERE tourid = @id", Connection))
            {
                sql.Parameters.AddWithValue("name", tour.Name);
                sql.Parameters.AddWithValue("description", tour.Description);
                sql.Parameters.AddWithValue("from", tour.From);
                sql.Parameters.AddWithValue("to", tour.To);
                sql.Parameters.AddWithValue("transportType", tour.TransportType.ToString());
                sql.Parameters.AddWithValue("distance", tour.TourDistance);
                sql.Parameters.AddWithValue("time", tour.EstimatedTime);
                sql.Parameters.AddWithValue("id", tour.ID);
                sql.ExecuteNonQuery();
            }
            Disconnect();

            Logger.Info("Tour with ID:" + tour.ID + " was changed");
        }

        public void changeLogDB(int oldLogID, TourLogs log)
        {
            Connect();
            using (var sql = new NpgsqlCommand("UPDATE tourlogs SET comment = @comment, difficulty = @difficulty, totaltime = @totaltime, rating = @rating WHERE logid = @id", Connection))
            {
                sql.Parameters.AddWithValue("comment", log.Comment);
                sql.Parameters.AddWithValue("difficulty", log.Difficulty);
                sql.Parameters.AddWithValue("totaltime", log.TotalTime);
                sql.Parameters.AddWithValue("rating", log.Rating);
                sql.Parameters.AddWithValue("id", oldLogID);
                sql.ExecuteNonQuery();
            }
            Disconnect();

            Logger.Info("Log with ID:" + oldLogID + " was changed");
        }
    }
}
