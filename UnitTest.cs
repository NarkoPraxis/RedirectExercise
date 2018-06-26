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
		/// Tests with basic input, nothing special
		/// </summary>
        [TestMethod]
        public void ProcessSimpleInputTest() {
           TestProcessWith(sampleInput, finishedSampleInputGraph);
        }

		/// <summary>
		/// Tests with larger, but still basic input
		/// </summary>
        [TestMethod]
        public void ProcessLargerInput() {
            TestProcessWith(largerInput, finishedLargerInputGraph);
        }

        /// <summary>
        /// the order of the routes has been re-ordered
        /// </summary>
        [TestMethod]
        public void ProcessShuffledInput() {
			TestProcessWith(shuffledInput, finishedShuffledGraph);
        }

        /// <summary>
        /// there are more routes that have been shuffled more
        /// </summary>
        [TestMethod]
        public void ProcessLargerShuffledInput() {
			TestProcessWith(largerShuffledInput, finishedLargerShuffledGraph);
        }

		/// <summary>
		/// testing if I can catch multiple routes splitting from one destination
		/// </summary>
		[TestMethod]
		public void SplitRouteInputTest() {
			TestProcessThrowsExceptionWith(splitInput);
		}

		/// <summary>
		/// testing If I can catch multiple routes splitting from one destination
		/// </summary>
		[TestMethod]
		public void SimpleSplitRouteInputTest() {
			TestProcessThrowsExceptionWith(simpleSplitInput);
		}

		/// <summary>
		/// after considering the complexity of allowing two routes to merge into 
		/// a single route, I decided not to support that feature at this time. 
		/// Thus, I am treating two routes merging as invalid input even though
		/// a more robust route analyzer should be able to handle it.
		/// </summary>
		[TestMethod]
		public void MergedRouteInputTest() {
			TestProcessThrowsExceptionWith(mergedInput);
		}

		/// <summary>
		/// largest, most complicated input test, order has been rearranged and
		/// some input is given multiple reroutes
		/// </summary>
		[TestMethod]
		public void ProcessLargestInput() {
			TestProcessThrowsExceptionWith(LargestTestInput);
		}

		/// <summary>
		/// edge detection, what happens when the input is small and has a duplicate
		/// </summary>
		[TestMethod]
		public void ProcessSmallestInput() {
			TestProcessWith(smallestTestInput, finishedSmallestTestGraph);
		}

		/// <summary>
		/// Tests the RouteGraph.MapPath() to see if a single line can be added correctly
		/// </summary>
		[TestMethod]
        public void MapPathTest() {
            RouteGraph routeGraph = new RouteGraph(midProcessGraph, midProcessIdentifiers);
			RouteGraph testGraph = new RouteGraph(finishedMidProcessGraph, null);
			routeGraph.MapPath("/about-us.html -> /about");
			routeGraph.Equals(testGraph);
        }

        /// <summary>
        /// This tests wether or not I can determine if a given input
        /// would create a cirular dependancy. 
        /// A circular Dependancy is defined as any route that eventually routes to itself
        /// </summary>
        [TestMethod]
        public void IsCircularReferenceTest() {
			TestProcessThrowsExceptionWith(invalidInput);
		}

		/// <summary>
		/// This test a circular dependancy with other routes between the 
		/// ends of the chain of redirects
		/// </summary>
		[TestMethod]
        public void IsCircularChainReferenceTest() {
			TestProcessThrowsExceptionWith(invalidChainInput);
        }

		/// <summary>
		/// tests if the output graph can be parsed back into the expected output
		/// </summary>
        [TestMethod]
        public void TraverseGraphTest() {
            RouteGraph routeGraph = new RouteGraph(finishedSampleInputGraph, finishedSampleInputIdentifiers);
            List<string> traversalResult = routeGraph.TraverseGraph();
            for (int i = 0; i < traversalResult.Count; i++) {
                Assert.IsTrue(traversalResult[i].Equals(sampleOutput[i]));
            }
        }


		/// <summary>
		/// tests if the Equals method can be expected to return valid results
		/// </summary>
		[TestMethod]
        public void CompareGraphs()
        {
			RouteGraph one = new RouteGraph(finishedSampleInputGraph, null);
			RouteGraph two = new RouteGraph(finishedLargerInputGraph, null);
			RouteGraph three = new RouteGraph(finishedShuffledGraph, null);

			Assert.IsTrue(one.Equals(one));
			Assert.IsTrue(two.Equals(two));
			Assert.IsTrue(three.Equals(three));
			Assert.IsFalse(one.Equals(two));
			Assert.IsFalse(one.Equals(three));
			Assert.IsFalse(two.Equals(three));
        }

		/// <summary>
		/// Used for running tests on the process function
		/// </summary>
		/// <param name="input">input to be processed</param>
		/// <param name="output">graph to be validated</param>
        private void TestProcessWith(List<string> input, List<List<int>> output) {
			RouteGraph routeGraph = new RouteGraph();
			RouteGraph testGraph = new RouteGraph(output, null);
            try {
                routeGraph.Process(input);
            }
            catch (Exception exc) {
                Assert.Fail(exc.Message);
            }
			Assert.IsTrue(routeGraph.Equals(testGraph));
        }

		/// <summary>
		/// Used for running tests when the process is supposed to throw and exception
		/// </summary>
		/// <param name="input">input to be processed</param>
		private void TestProcessThrowsExceptionWith(List<string> input) {
			RouteGraph routeGraph = new RouteGraph();
			try {
				routeGraph.Process(input);
			}
			catch (ArgumentException exc) {
				Assert.IsTrue(exc.GetType() == typeof(ArgumentException));
				return;
			}
			Assert.Fail("Test should have thrown an ArgumentException, it did not");
		}

		// This region contains all of the data structures needed for running the tests
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

        List<List<int>> finishedShuffledGraph = new List<List<int>>() {
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
            "/about-us.html -> /info -> /about",
        };

		List<string> simpleSplitInput = new List<string>() {
			"/info -> /faq",
			"/info -> /about",
		};

		List<string> mergedInput = new List<string>() {
			"/info -> /faq",
			"/about -> /faq",
			"/faq -> /question",
		};

		List<List<int>> midProcessGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2},
        };

        List<List<int>> finishedMidProcessGraph = new List<List<int>>() {
            new List<int>() {0,},
            new List<int>() {1, 2, 3},
        };

        List<string> LargestTestInput = new List<string> {
            "/kitchen ->/sink -> /dishes",
            "/home -> /kitchen",
            "/faq -> /info",
            "/about-us.html -> /about -> /faq",
            "/our-ceo.html -> /about-us.html",
            "/google -> /faq -> /filter",
            "/product-1.html -> /seo -> /google",
            "/faq -> /split",
        };


        List<List<int>> finishedDuplicatesGraph = new List<List<int>> {
            new List<int>() {0, 1, 2},
            new List<int>() {3, 4},
        };

		List<string> smallestTestInput = new List<string>() {
			"/kitchen",
			"/kitchen",
		};

		List<List<int>> finishedSmallestTestGraph = new List<List<int>> {
			new List<int>() {0 }
		};

        #endregion
    }
}