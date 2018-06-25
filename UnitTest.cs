
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
        /// Tests from beginning to end that larger correct input can be processed correctly,
        /// </summary>
        [TestMethod]
        public void ProcessLargestInput() {
            try {
                _routeGraph = new RouteGraph();
                _routeGraph.Process(LargestTestInput);
                CompareGraphs(finishedLargestTestGraph);
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
        /// Tests from beginning to end that larger correct input can be processed correctly,
        /// </summary>
        [TestMethod]
        public void ProcessLargerShuffledInput() {
            try {
                _routeGraph = new RouteGraph();
                _routeGraph.Process(largerShuffledInput);
                CompareGraphs(finishedLargerShuffledGraph);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }



        /// <summary>
        /// Tests the if the RouteGraph.MapPath() to see if a single line can be added correctly
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

        [TestMethod]
        public void IsCircularChainReferenceTest() {
            _routeGraph = new RouteGraph();
            try {
                _routeGraph.Process(invalidChainInput);
            }
            catch (ArgumentException exc) {
                Assert.IsTrue(exc.GetType() == typeof(ArgumentException));
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void SplitRouteInputTest() {
            try { 
                _routeGraph = new RouteGraph();
                _routeGraph.Process(shuffledInput);
                CompareGraphs(finishedShuffledInput);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void TraverseGraphTest() {
            SetupForTest(finishedSampleInputGraph, finishedSampleInputIdentifiers);
            List<string> traversalResult = _routeGraph.TraverseGraph();
            for (int i = 0; i < traversalResult.Count; i++) {
                Assert.IsTrue(traversalResult[i].Equals(sampleOutput[i]));
            }
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

        List<string> sampleOutput = new List<string>() {
            "/home",
            "/our-ceo.html -> /about-us.html -> /about",
            "/product-1.html -> /seo"
        };

        List<List<int>> finishedSampleInputGraph = new List<List<int>>() {
            new List<int>() {0 },
            new List<int>() {1, 2, 3 },
            new List<int>() {4, 5 }
        };

        Dictionary<string, int> finishedSampleInputIdentifiers = new Dictionary<string, int> {
            { "/home", 0},
            { "/our-ceo.html", 1 },
            { "/about-us.html", 2 },
            { "/about", 3 },
            { "/product-1.html", 4 },
            { "/seo", 5 }
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

        List<string> largerShuffledInput = new List<string>() {
            "/kitchen ->/sink -> /dishes",
            "/home -> /kitchen",
            "/faq -> /info",
            "/about-us.html -> /about -> /faq",
            "/our-ceo.html -> /about-us.html",
            "/google -> /images -> /filter",
            "/product-1.html -> /seo -> /google",
        };

        List<List<int>> finishedLargerShuffledGraph = new List<List<int>>() {
            new List<int>() {3, 0, 1, 2},
            new List<int>() {8, 6, 7, 4, 5},
            new List<int>() {12, 13, 9, 10, 11}
        };

        List<string> invalidInput = new List<string>() {
            "/info -> /faq",
            "/faq -> /info",
        };

        List<string> invalidChainInput = new List<string>() {
            "/faq -> /chain -> /info",
            "/info -> /secondchain -> /faq",
        };

        Dictionary<string, int> midProcessIdentifiers = new Dictionary<string, int> {
            { "/home", 0},
            { "/our-ceo.html", 1 },
            { "/about-us.html", 2 }
        };

        List<string> splitInput = new List<string>() {
            "/info -> /faq",
            "/about-us.html -> /about -> /info",
            "/info -> /fail",
            "/info -> /git-gud"
        };

        List<List<int>> finishedSplitInput = new List<List<int>>() {
            new List<int>() {2, 3, 0, 1, 4, 5 }
        };

        List<List<int>> midProcessGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2},
        };

        List<List<int>> midProcessFinishedGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2, 3},
        };

        List<string> LargestTestInput = new List<string> {
            "/kitchen ->/sink -> /dishes",
            "/home -> /kitchen",
            "/faq -> /info",
            "/about-us.html -> /about -> /faq",
            "/our-ceo.html -> /about-us.html",
            "/google -> /images -> /filter",
            "/product-1.html -> /seo -> /google",
            "/faq -> /split",
            "/info -> /fail",
            "/info -> /git-gud"
        };

        List<List<int>> finishedLargestTestGraph = new List<List<int>>() {
            new List<int>() {3, 0, 1, 2},
            new List<int>() {8, 6, 7, 4, 5, 14, 15, 16},
            new List<int>() {12, 13, 9, 10, 11}
        };

        List<List<int>> finishedDuplicatesGraph = new List<List<int>> {
            new List<int>() {0, 1, 2},
            new List<int>() {3, 4},
        };

        #endregion
    }
}

