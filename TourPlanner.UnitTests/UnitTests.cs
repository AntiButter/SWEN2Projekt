using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
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
            mockTourLogDataAccess.Setup(x => x.getTourLogAmountTotal()).Returns(20); //Returns that 20 logs exist, arbitrary amount chosen for popularity calculation in Test

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
        public void test_timePenalty()
        {
            //check the timePenalty function, which calculates a malus for the ChildFriendliness

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 80, 3, 1, 2)); //totalTime = 80
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 100, 3, 1, 2)); //totalTime = 100
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 0, 120, 3, 1, 2)); //totalTime = 120
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 140, 3, 1, 2)); //totalTime = 140
            test.setLogs(logsTest);
            //total average time = (80+100+120+140)/4 = 440/4 = 110
            //penalty breakpoints: longer than 90 minutes => 1 penalty point --- longer than 180 minutes => 2 penalty points  
            //penalty should be 1 here

            Tour test2 = new Tour(2, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest2 = new List<TourLogs>();
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 2, 20, 3, 1, 2)); //totalTime = 20
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 1, 20, 3, 1, 2)); //totalTime = 20
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 0, 20, 3, 1, 2)); //totalTime = 20
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 1, 20, 3, 1, 2)); //totalTime = 20
            test2.setLogs(logsTest2);
            //total average time = (20+20+20+20)/4 = 80/4 = 20
            //penalty breakpoints: longer than 90 minutes => 1 penalty point --- longer than 180 minutes => 2 penalty points  
            //penalty should be 0 here

            //act
            int penalty1 = tourGetter.timePenalty(test);
            int penalty2 = tourGetter.timePenalty(test2);

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, penalty1);
                Assert.AreEqual(0, penalty2);
            });
        }


        [Test]
        public void test_calculateChildFriendliness()
        {
            //check the calculateChildFriendliness function as a whole

            //arrange 
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 1000, "111"); //distance = 1000
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 1000, 5, 1, 2)); //totalTime = 1000  //difficulty = 5
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 1000, 5, 1, 2)); //totalTime = 1000  //difficulty = 5
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 0, 1000, 5, 1, 2)); //totalTime = 1000  //difficulty = 5
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 1000, 5, 1, 2)); //totalTime = 1000  //difficulty = 5
            test.setLogs(logsTest);
            //total childFriendliness should be 1 (because all negatives go to -2, and its set to 1 because childFriendliness can not go below 1)
            

            Tour test2 = new Tour(2, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 5, "111");  //distance = 5
            List<TourLogs> logsTest2 = new List<TourLogs>();
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 2, 20, 3, 1, 2)); //totalTime = 20  //difficulty = 3
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 1, 20, 1, 1, 2)); //totalTime = 20  //difficulty = 1
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 0, 20, 3, 1, 2)); //totalTime = 20  //difficulty = 3 
            logsTest2.Add(new TourLogs("testTimeStamp", "testComment", 1, 20, 1, 1, 2)); //totalTime = 20  //difficulty = 1
            test2.setLogs(logsTest2);
            //total childFriendliness should be 5 (because everything is below the breakpoints)

            //act
            tourGetter.calculateChildFriendliness(test);
            tourGetter.calculateChildFriendliness(test2);

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, test.ChildFriendliness);
                Assert.AreEqual(5, test2.ChildFriendliness);
            });
        }

        [Test]
        public void test_calculatePopularityWithLogs()
        {
            //check the calculatePopularity function, which calculates the popularity in comparison to the total logs that exist across all tours

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 80, 3, 1, 2)); 
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); 
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 0, 80, 3, 1, 2)); 
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); 
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); 
            test.setLogs(logsTest);
            //5 logs. when compared to the total logs (getTourLogAmountTotal() = 20), it equals 25%
            //25% of logs, should be a popularity of 3

            //act
            tourGetter.calculatePopularity(test);

            //assert
            Assert.AreEqual(3, test.Popularity);
        }        

        [Test]
        public void test_calculatePopularityWithoutLogs()
        {
            //check the calculatePopularity function, which calculates the popularity in comparison to the total logs that exist across all tours

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111");
            List<TourLogs> logsTest = new List<TourLogs>();
            test.setLogs(logsTest);
            //0 logs. when compared to the total logs (getTourLogAmountTotal() = 20), it equals 0%
            //0% of logs, should be a popularity of 1 (minimum)

            //act
            tourGetter.calculatePopularity(test);

            //assert
            Assert.AreEqual(1, test.Popularity);
        }

        [Test]
        public void test_calculateParameters()
        {
            //check the calculateParameters function, which calculates childfriendliness and popularity

            //arrange
            Tour test = new Tour(1, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111"); //distance = 111.11
            List<TourLogs> logsTest = new List<TourLogs>();
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 2, 80, 3, 1, 2)); //totalTime = 80  //difficulty = 3
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); //totalTime = 80  //difficulty = 3
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 0, 80, 3, 1, 2)); //totalTime = 80  //difficulty = 3
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); //totalTime = 80  //difficulty = 3
            logsTest.Add(new TourLogs("testTimeStamp", "testComment", 1, 80, 3, 1, 2)); //totalTime = 80  //difficulty = 3
            test.setLogs(logsTest);
            List<Tour> list = new List<Tour>();
            list.Add(test);
            IEnumerable<Tour> enumerable = list;
            //5 logs. when compared to the total logs (getTourLogAmountTotal() = 20), it equals 25%
            //25% of logs, should be a popularity of 3
            //total childFriendliness should be 5 

            //act
            tourGetter.calculateParameters(enumerable);

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, test.ChildFriendliness);
                Assert.AreEqual(3, test.Popularity);
            });
        }

        [Test]
        public void test_exportJSON()
        {
            //check if the export to JSON works correctly

            //arrange
            string expectedResult = "{\r\n  \"tour\": [\r\n    {\r\n      \"tourname\": \"testTour1\",\r\n      \"description\": \"testDescription1\",\r\n      \"from\": \"Wien\",\r\n      \"to\": \"Linz\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": []\r\n    },\r\n    {\r\n      \"tourname\": \"testTour2\",\r\n      \"description\": \"testDescription2\",\r\n      \"from\": \"Linz\",\r\n      \"to\": \"Burgenland\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": [\r\n        {\r\n          \"timestamp\": \"testTimeStamp\",\r\n          \"comment\": \"testComment\",\r\n          \"difficulty\": 5,\r\n          \"rating\": 5,\r\n          \"totaltime\": 80\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            //act
            string resultJSON = importExport.export();

            //assert
            Assert.AreEqual(expectedResult, resultJSON);
        }

        [Test]
        public void test_exportSave()
        {
            //check if the exported JSON file is saved correctly

            //arrange
            string expectedResult = "{\r\n  \"tour\": [\r\n    {\r\n      \"tourname\": \"testTour1\",\r\n      \"description\": \"testDescription1\",\r\n      \"from\": \"Wien\",\r\n      \"to\": \"Linz\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": []\r\n    },\r\n    {\r\n      \"tourname\": \"testTour2\",\r\n      \"description\": \"testDescription2\",\r\n      \"from\": \"Linz\",\r\n      \"to\": \"Burgenland\",\r\n      \"transporttype\": \"running\",\r\n      \"logs\": [\r\n        {\r\n          \"timestamp\": \"testTimeStamp\",\r\n          \"comment\": \"testComment\",\r\n          \"difficulty\": 5,\r\n          \"rating\": 5,\r\n          \"totaltime\": 80\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            //act
            string resultJSON = importExport.export();
            importExport.save(resultJSON);
            //because the filename contains the current timestamp, we cannot directly open the file by name, so we have to use a roundabout way to test it
            var dir = new DirectoryInfo("../../../../ExportFiles");
            var myFile = (from file in dir.GetFiles() orderby file.LastWriteTime descending select file).First();

            string JSONfromFile = File.ReadAllText(myFile.FullName);

            //assert
            Assert.AreEqual(expectedResult, JSONfromFile);

            //cleanup
            File.Delete(myFile.FullName);
        }

        [Test]
        public void test_getPicture_filler()
        {
            //check if the fallback filler image works correctly, incase no image is found

            //arrange
            int testTourID = 0; //Tour ID 0 should never exist, the Database starts at 1, so it can be used to test
                                //get the filler image that should be returned if the function works as expected
            BitmapImage expectedImage = new BitmapImage();
            expectedImage.BeginInit();
            expectedImage.CacheOption = BitmapCacheOption.OnLoad;
            expectedImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            expectedImage.UriSource = new Uri(Path.GetFullPath("../../../../Pictures/filler.png"));
            expectedImage.EndInit();

            //act
            BitmapImage image = tourGetter.getPicture(testTourID);

            //assert
            Assert.AreEqual(expectedImage.UriSource, image.UriSource); 
            //because BitmapImages, even if created from the same path, are not equal if directly compared, we compare the .UriSource parameter of both

        }

        [Test]
        public void test_configAccess_DB()
        {
            //check if the access to the config file works

            //arrange
            string expectedDBString = "Host=localhost;Username=postgres;Password=tour;Database=postgres";

            //act
            string resultDBString = ConfigAccess.getDatabaseString();

            //assert
            Assert.AreEqual(expectedDBString, resultDBString);
        }

        [Test]
        public void test_tourManager_search_fullName()
        {
            //check if the search function for the tours works

            //arrange
            //already done in Mock above, testTour1 and testTour2 exist
            string expectedTourname = "testTour2";

            //act
            string searchString = "testTour2";
            List<Tour> resultList = new List<Tour>(tourGetter.Search(searchString));

            //assert
            Assert.AreEqual(expectedTourname, resultList[0].Name);
        }        
        [Test]
        public void test_tourManager_search_partialName()
        {
            //check if the search function for the tours works

            //arrange
            //already done in Mock above, testTour1 and testTour2 exist
            string expectedTourname1 = "testTour1";
            string expectedTourname2 = "testTour2";

            //act
            string searchString = "tt"; //tt from testTour, but lowercase
            List<Tour> resultList = new List<Tour>(tourGetter.Search(searchString));

            //assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedTourname1, resultList[0].Name);
                Assert.AreEqual(expectedTourname2, resultList[1].Name);
            });
        }        
        [Test]
        public void test_tourReport()
        {
            //check if a PDF file is created in the right folder

            //arrange
            Tour test = new Tour(0, "testTour1", "testDescription1", "Wien", "Linz", Models.Enum.TransportType.running, 111.11, "111"); //ID = 0, because that can normally not happen, it can be tested without problem
            List<TourLogs> emptyLogs = new List<TourLogs>();
            test.setLogs(emptyLogs);

            //act
            PDFGenerator.tourReport(test);

            //assert
            Assert.IsTrue(File.Exists("../../../../PDF/TourID0.pdf"));

            //cleanup
            File.Delete("../../../../PDF/TourID0.pdf");
        }

        [Test]
        public void test_callAPI_workingTour()
        {
            //check how the API responds with correct data inout

            //arrange
            Tour test = new Tour("testTour", "", "Wien", "Linz", Models.Enum.TransportType.running);
            test.setID(-1);
            var API = new MapQuestAPICall();

            //act
            bool result = API.callAPI(test);

            //assert
            Assert.AreEqual(true, result);

            //cleanup
            File.Delete("../../../../Pictures/TourID-1.png");
        }        
        [Test]
        public void test_callAPI_wrongTour()
        {
            //check how the API responds with correct data inout

            //arrange
            Tour test = new Tour("testTour", "", "Wien", "New York", Models.Enum.TransportType.running);
            var API = new MapQuestAPICall();

            //act
            bool result = API.callAPI(test);

            //assert
            Assert.AreEqual(false, result);
        }

        //unique feature beide
    }
}