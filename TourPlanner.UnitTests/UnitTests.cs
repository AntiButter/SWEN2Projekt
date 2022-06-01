using NUnit.Framework;
using TourPlanner;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            TourGetter tourGetter = new TourGetter();
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
        public void test_distancePenalty()
        {
            //check the distancePenalty function, which calculates a malus for the Child Friendliness

            //arrange
            //Tour testTour = new Tour()


            //act


            //assert

        }
        public void test_export_onlyTour()
        {
            //check the distancePenalty function, which calculates a malus for the Child Friendliness

            //arrange
            //Tour testTour = new Tour()


            //act


            //assert

        }

        public void test_export_tourWithLogs()
        {
            //check the distancePenalty function, which calculates a malus for the Child Friendliness

            //arrange
            //Tour testTour = new Tour()


            //act


            //assert

        }
    }
}