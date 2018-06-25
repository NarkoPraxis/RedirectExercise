
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


        /// <summary>
        /// The current graph being used for testing
        /// </summary>
        RouteGraph _routeGraph = new RouteGraph();

        //sets up a test before running it.
        public void SetupForTest(List<List<int>> initialGraph, Dictionary<string, int> identifiers) {
            _routeGraph = new RouteGraph(initialGraph, identifiers);
        }

        

        [TestMethod]
        public void ProcessSimpleInputTest() {
            try {
                _routeGraph = new RouteGraph();
                _routeGraph.Process(sampleInput);
                CompareGraphs(finishedSampleInputGraph);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ProcessLargerInput() {
            try {
                _routeGraph = new RouteGraph();
                _routeGraph.Process(largerInput);
                CompareGraphs(finishedLargerInputGraph);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests from beginning to end that correct input can be processed correctly,
        /// </summary>
        [TestMethod]
        public void ProcessShuffledInput() {
            try {
                _routeGraph = new RouteGraph();
                _routeGraph.Process(shuffledInput);
                CompareGraphs(finishedShuffledInput);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests the if the RouteGraph.Insert() function can find the right posi
        /// Does this happen as i expect?
        /// </summary>
        [TestMethod]
        public void MapPathTest() {
            SetupForTest(midProcessGraph, midProcessIdentifiers);
            _routeGraph.MapPath("/about-us.html -> /about");
            CompareGraphs(midProcessFinishedGraph);
            
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
            _routeGraph = new RouteGraph();
            try {
                _routeGraph.Process(invalidInput);
            }
            catch (ArgumentException exc) {
                Assert.IsTrue(exc.GetType() == typeof(ArgumentException));
                return;
            }
            Assert.Fail();
        }

        private void CompareGraphs(List<List<int>> testGraph)
        {
            for (int i = 0; i < _routeGraph.Graph.Count; i++) {
                List<int> path = _routeGraph.Graph[i];
                for (int k = 0; k < path.Count; k++) {
                    Assert.AreEqual(path[k], testGraph[i][k]);
                }
            }
        }

        #region Lists for testing
        List<string> sampleInput = new List<string>() {
            "/home",
            "/our-ceo.html -> /about-us.html",
            "/about-us.html -> /about",
            "/product-1.html -> /seo"
        };

        List<List<int>> finishedSampleInputGraph = new List<List<int>>() {
            new List<int>() {0 },
            new List<int>() {1, 2, 3 },
            new List<int>() {4, 5 }
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

        List<List<int>> finishedLargerInputGraph = new List<List<int>>() {
            new List<int>() {0, 1, 2, 3},
            new List<int>() {4, 5, 6, 7, 8},
            new List<int>() {9, 10, 11, 12, 13}
        };

        List<string> shuffledInput = new List<string>() {
            "/google -> /images -> /filter",
            "/product-1.html -> /seo -> /google",
        };

        List<List<int>> finishedShuffledInput = new List<List<int>>() {
            new List<int>() {3, 4, 0, 1, 2},
        };

        List<string> invalidInput = new List<string>() {
            "/info -> /faq",
            "/faq -> /info",
        };

        Dictionary<string, int> midProcessIdentifiers = new Dictionary<string, int> {
            { "/home", 0},
            { "/our-ceo.html", 1 },
            { "/about-us.html", 2 }
        };


      List<List<int>> midProcessGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2},
        };

        List<List<int>> midProcessFinishedGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2, 3},
        };



        #endregion
    }
}