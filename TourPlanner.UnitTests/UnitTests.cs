using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TourPlanner;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.UnitTests
{
    public class Tests
    {
        TourGetter tourGetter;
        TourManager tourManager;
        ImportExport importExport;

        Tour tourWithoutLogs = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");

        Tour tourWithLog = new Tour(2, "testTour2", "testDescription2", "Linz", "Burgenland", Models.Enum.TransportType.running, 222.22, "222");
        TourLogs log = new TourLogs("testTimeStamp","testComment", 5, 80, 5,1, 2);
        List<TourLogs> Logs = new List<TourLogs>();
        List<TourLogs> emptyLogs = new List<TourLogs>();
        List<Tour> Tourlist = new List<Tour>();
        IEnumerable<Tour> tourIEnumarable;

        [OneTimeSetUp]
        public void Setup()
        {
            Logs.Add(log);
            tourWithLog.setLogs(Logs);

            tourWithoutLogs.setLogs(emptyLogs);

            Tourlist.Add(tourWithoutLogs);
            Tourlist.Add(tourWithLog);


            var mockTourDataAccess = new Mock<ITourDataAccess>();
            mockTourDataAccess.Setup(x => x.GetItems()).Returns(Tourlist);

            var mockTourLogDataAccess = new Mock<ITourLogDataAccess>();
            mockTourLogDataAccess.Setup(x => x.getTourLogAmountTotal()).Returns(6); //Returns that 6 logs exist, arbitrary amount chosen for calculations afterwards

            tourManager = new TourManager(mockTourDataAccess.Object);
            importExport = new ImportExport(mockTourDataAccess.Object);
            tourGetter = new TourGetter(mockTourDataAccess.Object, mockTourLogDataAccess.Object);
        }

        [Test]
        public void test_checkNull()
        {
            //test custom function, that checks if a string is empty ( "" ), if it is, return null
            //else, return the string back

            //arrange
            string notNull = "test";
            string isNull =  "";
            string? notNullReturn;
            string? isNullReturn;


            //act
            notNullReturn = UtilityFunctions.checkNull(notNull);
            isNullReturn = UtilityFunctions.checkNull(isNull);

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("test", notNullReturn);
                Assert.AreEqual(null, isNullReturn);
            });
        }
        
        [Test]
        public void test_difficultyPenaltyYes()
        {
            //check the difficultyPenalty function, which calculates a malus for the ChildFriendliness

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 5, 80, 3, 1, 2)); //difficulty = 5
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 3, 80, 3, 1, 2)); //difficulty = 3
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 6, 80, 3, 1, 2)); //difficulty = 6
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 80, 3, 1, 2)); //difficulty = 2
            test.setLogs(logsTest);
            //total average difficulty = (5+3+6+2)/4 = 16/4 = 4
            //Because a difficulty of 1-2 should not be considered for child friendliness, and each point above 2 should increasingly affect child friendliness, the value of 4 is substracted by 2

            //act
            int penalty = tourGetter.difficultyPenalty(test);

            //assert
            Assert.AreEqual(2, penalty);
        }        
        
        [Test]
        public void test_difficultyPenaltyNo()
        {
            //check the difficultyPenalty function, which calculates a malus for the ChildFriendliness

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 80, 3, 1, 2)); //difficulty = 2
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); //difficulty = 1
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 0, 80, 3, 1, 2)); //difficulty = 0
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); //difficulty = 1
            test.setLogs(logsTest);
            //total average difficulty = (2+1+0+1)/4 = 4/4 = 1
            //Because a difficulty of 1-2 should not be considered for child friendliness, and each point above 2 should increasingly affect child friendliness, the value of 1 is substracted by 2
            //Because 1 substracted by 2 

            //act
            int penalty = tourGetter.difficultyPenalty(test);

            //assert
            Assert.AreEqual(0 ,penalty);
        }

        [Test]
        public void test_distancePenalty()
        {
            //check the distancePenalty function, which calculates a malus for the Child Friendliness

            //arrange
            Tour test1 = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 8.5, "111"); //distance is 8.5
            Tour test2 = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 12, "111"); //distance is 12
            //penalty breakpoints: longer than 5km => 1 penalty point --- longer than 10km => 2 penalty points  

            //act
            int penalty1 = tourGetter.distancePenalty(test1);
            int penalty2 = tourGetter.distancePenalty(test2);

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, penalty1);
                Assert.AreEqual(2, penalty2);
            });
        }

        [Test]
        public void test_export()
        {
            //check if the export to JSON works correctly

            //arrange
            string expectedResult = "{\r\n  \"tour\": [\r\n    {\r\n      \"tourname\": \"testTour1\",\r\n      \"description\": \"testDescription1\",\r\n      \"from\": \"Wien\",\r\n      \"to\": \"Linz\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": []\r\n    },\r\n    {\r\n      \"tourname\": \"testTour2\",\r\n      \"description\": \"testDescription2\",\r\n      \"from\": \"Linz\",\r\n      \"to\": \"Burgenland\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": [\r\n        {\r\n          \"timestamp\": \"testTimeStamp\",\r\n          \"comment\": \"testComment\",\r\n          \"difficulty\": 5,\r\n          \"rating\": 5,\r\n          \"totaltime\": 80\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            //act
            string resultJSON = importExport.export();

            //assert
            Assert.AreEqual(expectedResult, resultJSON);
        }

        public void test_export_tourWithLogs()
        {
            //check the distancePenalty function, which calculates a malus for the Child Friendliness

            //arrange
            //Tour testTour = new Tour()
            //impor

            //act


            //assert

        }
    }
}