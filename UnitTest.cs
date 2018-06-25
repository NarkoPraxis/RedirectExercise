using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedirectsExercise;

namespace RedirectUnitTests
{
    /// <summary>
    /// Tests the functions of the RouteGraph Tests
    /// </summary>
    [TestClass]
    public class RouteGraphTests
    {
        #region Lists for testing
        List<string> sampleInput = new List<string>() {
            "/home",
            "/our-ceo.html -> /about-us.html",
            "/about-us.html -> /about",
            "/product-1.html -> /seo"
        };

        List<List<string>> finishedSampleInputGraph = new List<List<string>>() {
            new List<string>() {"/home" },
            new List<string>() {"/our-ceo.html", "/about-us.html", "/about" },
            new List<string>() {"/product-1.html", "seo" }
        };

        List<string> largerInput = new List<string>() {
            "/home -> /kitchen",
            "/kitchen ->/sink -> /dishes",
            "/our-ceo.html -> /about-us.html",
            "/about-us.html -> /about -> /faq",
            "/faq -> /info",
            "/product-1.html -> /seo -> /google",
            "/google -> /images -> /filter",
        };

        List<List<string>> finishedLargerInputGraph = new List<List<string>>() {
            new List<string>() {"/home", "/kitchen", "/sink", "/dishes" },
            new List<string>() {"/our-ceo.html", "/about-us.html", "/about", "/faq", "/info"},
            new List<string>() {"/product-1.html", "/seo", "/google", "/images", "filter"}
        };

        List<string> shuffledInput = new List<string>() {
            "/google -> /images -> /filter",
            "/our-ceo.html -> /about-us.html",
            "/kitchen -> /sink -> /dishes",
            "/product-1.html -> /seo -> /google",
            "/about-us.html -> /about -> /faq",
            "/faq -> /info",
            "/home -> /kitchen",
        };

        List<string> invalidInput = new List<string>() {
            "/google -> /images -> /filter",
            "/our-ceo.html -> /about-us.html",
            "/info -> /faq",
            "/kitchen -> /sink -> /dishes",
            "/product-1.html -> /seo -> /google",
            "/about-us.html -> /about -> /faq",
            "/faq -> /info",
            "/home -> /kitchen",
        };


        List<List<string>> midProcessGraph = new List<List<string>>() {
            new List<string>() { "/home" },
            new List<string>() {"/our-ceo.html", "/about-us.html" }
        };

        List<List<string>> midProcessFinishedGraph = new List<List<string>>() {
            new List<string>() { "/home" },
            new List<string>() {"/our-ceo.html", "/about-us.html", "/about" }
        };



        #endregion

        /// <summary>
        /// The current graph being used for testing
        /// </summary>
        RouteGraph _routeGraph = new RouteGraph();

        //sets up a test before running it.
        public void SetupForTest(List<List<string>> initialGraph = null) {
            _routeGraph.Graph = initialGraph;
        }

        /// <summary>
        /// Tests from beginning to end that correct input can be processed correctly,
        /// </summary>
        [TestMethod]
        public void ProcessCorrectInputTest() {
            try {
                _routeGraph.Process(shuffledInput);
                CompareGraphs(finishedLargerInputGraph);
            }
            catch {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests the if the RouteGraph.Insert() function can find the right posi
        /// Does this happen as i expect?
        /// </summary>
        [TestMethod]
        public void InsertTest() {
            SetupForTest(midProcessGraph);
            _routeGraph.MapRoute("/about-us.html -> /about");
            CompareGraphs(midProcessFinishedGraph);
            
        }

        /// <summary>
        /// This Tests putting the graph back into the expected output for a redirect path
        /// Can I find the route path I want to find?
        /// </summary>
        [TestMethod]
        public void GetPathTest() {
            SetupForTest(finishedLargerInputGraph);
            Assert.Equals(_routeGraph.GetPath(0), "/home -> /kitchen -> /sink -> /dishes");
            Assert.Equals(_routeGraph.GetPath(1), "/our-ceo.html -> /about-us.html -> /about -> /faq -> /info");
            Assert.Equals(_routeGraph.GetPath(2), "/product-1.html -> /seo -> /google -> /images -> /filter");
        }

        /// <summary>
        /// This tests gettings a single redirect from the graph
        /// </summary>
        [TestMethod]
        public void GetRedirectTest() {
            SetupForTest(finishedLargerInputGraph);
            Assert.Equals(_routeGraph.GetRedirect(0, 0), "/home");
            Assert.Equals(_routeGraph.GetRedirect(2, 5), "/filter");
            Assert.Equals(_routeGraph.GetRedirect(1, 0), "/our-ceo.html");
        }

        /// <summary>
        /// This tests wether or not I can determine if a given input
        /// would create a cirular dependancy. I need to write specific tests, but
        /// I don't understand enough about how to do this yet.
        /// 
        /// validate this.
        /// </summary>
        [TestMethod]
        public void IsCircularReferenceTest() {
            SetupForTest();
            try {
                _routeGraph.Process(invalidInput);
            }
            catch (Exception exc) {
                Assert.IsTrue(exc.GetType() == typeof(ArgumentException));
            }
            Assert.Fail();
        }

        private void CompareGraphs(List<List<string>> testGraph)
        {
            for (int i = 0; i < _routeGraph.Graph.Count; i++) {
                List<string> path = _routeGraph.Graph[i];
                for (int k = 0; k < path.Count; k++) {
                    Assert.AreEqual(path[k], testGraph[i][k]);
                }
            }
        }

    }
}