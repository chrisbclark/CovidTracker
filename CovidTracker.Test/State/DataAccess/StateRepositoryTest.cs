using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CovidTracker.State.DataAccess;
using CovidTracker.State.Models;
using System.Runtime.ConstrainedExecution;

namespace CovidTracker.Test.State.DataAccess
{
    [TestClass]
    public class StateRepositoryTest
    {
        private StateModel[] testData = new StateModel[] {
            new StateModel() {state="MA", dateChecked="3/7/2021", positive=5, negative=3, totalTestResults=8},
            new StateModel() {state="NH", dateChecked="3/5/2021", positive=25, negative=35, totalTestResults=60},
            new StateModel() {state="VT", dateChecked="3/7/2021", positive=10, negative=40, totalTestResults=50}
        };

        [TestMethod]
        public void GetAll_Test()
        {
            Mock<HttpClientWrapper> client = new Mock<HttpClientWrapper>();
            client.Setup(c => c.LoadData(It.IsAny<string>())).ReturnsAsync(testData);
            StateRepository repo = new StateRepository();

            repo.InjectClient(client.Object);
            repo.Initialize();

            var result = repo.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(testData[0]));
            Assert.IsTrue(result.Contains(testData[1]));
            Assert.IsTrue(result.Contains(testData[2]));

        }

        [TestMethod]
        [ExpectedException(typeof(NotInitializedException))]
        public void GetAll_Test_NotInitialized()
        {
            Mock<HttpClientWrapper> client = new Mock<HttpClientWrapper>();
            client.Setup(c => c.LoadData(It.IsAny<string>())).ReturnsAsync(testData);
            StateRepository repo = new StateRepository();

            repo.InjectClient(client.Object);

            var result = repo.GetAll();
        }

        [TestMethod]
        public void Get_Test()
        {
            Mock<HttpClientWrapper> client = new Mock<HttpClientWrapper>();
            client.Setup(c => c.LoadData(It.IsAny<string>())).ReturnsAsync(testData);

            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/ma/20210307.json")).ReturnsAsync(testData[0]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/nh/20210307.json")).ReturnsAsync(testData[1]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/vt/20210307.json")).ReturnsAsync(testData[2]);


            StateRepository repo = new StateRepository();

            repo.InjectClient(client.Object);
            repo.Initialize();

            var result = repo.Get(new StateModelSpec(new List<string>() { "MA", "NH", "VT" }, "20210307")).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Contains(testData[0]));
            Assert.IsTrue(result.Contains(testData[1]));
            Assert.IsTrue(result.Contains(testData[2]));
        }

        [TestMethod]
        public void Get_Test_NoDate()
        {
            Mock<HttpClientWrapper> client = new Mock<HttpClientWrapper>();
            client.Setup(c => c.LoadData(It.IsAny<string>())).ReturnsAsync(testData);

            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/ma/20210307.json")).ReturnsAsync(testData[0]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/nh/20210307.json")).ReturnsAsync(testData[1]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/vt/20210307.json")).ReturnsAsync(testData[2]);


            StateRepository repo = new StateRepository();

            repo.InjectClient(client.Object);
            repo.Initialize();

            var result = repo.Get(new StateModelSpec(new List<string>() { "MA", "NH"})).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(testData[0]));
            Assert.IsTrue(result.Contains(testData[1]));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void Get_Test_NotInitialized()
        {
            Mock<HttpClientWrapper> client = new Mock<HttpClientWrapper>();
            client.Setup(c => c.LoadData(It.IsAny<string>())).ReturnsAsync(testData);

            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/ma/20210307.json")).ReturnsAsync(testData[0]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/nh/20210307.json")).ReturnsAsync(testData[1]);
            client.Setup(c => c.LoadSingle("https://api.covidtracking.com/v1/states/vt/20210307.json")).ReturnsAsync(testData[2]);


            StateRepository repo = new StateRepository();

            repo.InjectClient(client.Object);

            var result = repo.Get(new StateModelSpec(new List<string>() { "MA", "NH" })).Result;
        }
    }
}
