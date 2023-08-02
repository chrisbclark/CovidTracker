using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CovidTracker.State.DataAccess;
using CovidTracker.State.Models;

namespace CovidTracker.Test.State.Models
{
    [TestClass]
    public class StateModelTest
    {
        [TestMethod]
        public void Fields_Test()
        {
            StateModel test = new StateModel()
            {
                state = "MA",
                positive = 5,
                negative = 15,
                totalTestResults = 20,
                hospitalizedCurrently = 5,
                dateChecked="2021-03-07T12:59:00Z"

            };

            Assert.AreEqual(test.state, "MA");
            Assert.AreEqual(test.Name, "Massachusetts");
            Assert.AreEqual(test.positive, 5);
            Assert.AreEqual(test.negative, 15);
            Assert.AreEqual(test.totalTestResults, 20);
            Assert.AreEqual(test.PositiveRate, 25);
            Assert.AreEqual(test.hospitalizedCurrently, 5);
            Assert.AreEqual(test.DateString, "3/7/2021");
        }

        [TestMethod]
        public void StateModel_Sort()
        {
            List<StateModel> testData = new List<StateModel> {
            new StateModel() {state="MA", dateChecked="3/7/2021", positive=1, negative=19, totalTestResults=20},
            new StateModel() {state="NH", dateChecked="3/5/2021", positive=10, negative=10, totalTestResults=20},
            new StateModel() {state="VT", dateChecked="3/7/2021", positive=5, negative=15, totalTestResults=20}
            };

            testData.Sort();

            Assert.AreEqual(testData.ElementAt(0).state, "NH");
            Assert.AreEqual(testData.ElementAt(1).state, "VT");
            Assert.AreEqual(testData.ElementAt(2).state, "MA");

        }

    }
}
