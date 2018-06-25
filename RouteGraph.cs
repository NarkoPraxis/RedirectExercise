using System;
using System.Collections.Generic;
using System.Linq;

namespace RedirectsExercise
{
	/// <summary>
	/// takes a list of routes and prints out teh paths in the application without any duplicates
	/// </summary>
    public class RouteGraph : RouteAnalyzer
    {
        private List<List<int>> Graph { get; set; } = new List<List<int>>();
        private Dictionary<string, int> _identifiers { get; set; } = new Dictionary<string, int>();

        private int _uniqueIdentifier = 0;
        private static string[] _delimiters = { "->", " "};

		private const string _circularPathMessage = "Circular Path Detected";

        public RouteGraph() { }
    
        //This constructor allows the class to be unit testable
        public RouteGraph(List<List<int>> graph, Dictionary<string, int> identifiers) {
            Graph = graph;
			if (identifiers != null) {
				_identifiers = identifiers;
				_uniqueIdentifier = identifiers.Count;
			}
        }

		/// <summary>
		/// Processes a list of URL redirects and creates a simplified list of route paths
		/// </summary>
		public IEnumerable<string> Process(IEnumerable<string> routes) {
            foreach (string path in routes) {
                MapPath(path);
            }
            return TraverseGraph();
        }

		/// <summary>
		/// Maps a path into the graph of paths
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="ArgumentException">Thrown if a given route path creates a circular reroute path</exception>
        public void MapPath(string path) { 
            List<string> routeNames = ParseRoutes(path);
            //if the end of this route path already exists in the graph
            if (_identifiers.Keys.Contains(routeNames[routeNames.Count - 1])) {
                int id = _identifiers[routeNames[routeNames.Count - 1]];
                int pathIndex = GetPathIndex(id);

                //add the new path to the graph
                Graph.Add(new List<int>());
                for (int i = 0; i < routeNames.Count - 1; i++) {
                    if (CreatesCircularReference(routeNames[i], pathIndex)) {
                        throw new ArgumentException(_circularPathMessage);
                    }
                    else {
						AddRoute(routeNames[i], EndGraphIndex);
                    }
                }
                //add the old path on the end of the graph
                foreach(int routeID in Graph[pathIndex]) {
                    Graph[EndGraphIndex].Add(routeID);
                }
                //remove the old path
                Graph.RemoveAt(pathIndex);
            }
            //if the beginning of this route path already exists in the graph
            else if (_identifiers.Keys.Contains(routeNames[0])) {
                int id = _identifiers[routeNames[0]];
                int pathIndex = GetPathIndex(id);

                for (int i = 1; i < routeNames.Count; i++) {
                    if (CreatesCircularReference(routeNames[i], pathIndex)) { 
                        throw new ArgumentException(_circularPathMessage);
                    }
                    else {
						AddRoute(routeNames[i], pathIndex);
					}
				}
            }
            //this route path is unique
            else {
                Graph.Add(new List<int>());
                foreach (string route in routeNames) {
					AddRoute(route, EndGraphIndex);
                }
            }
        }

		/// <summary>
		/// Adds a route to the graph
		/// </summary>
		/// <param name="routeName">the name of the route</param>
		/// <param name="pathIndex">the index of the path to be added too</param>
		private void AddRoute(string routeName, int pathIndex) {
			_identifiers[routeName] = _uniqueIdentifier;
			Graph[pathIndex].Add(_uniqueIdentifier++);
		}

		/// <summary>
		/// Gets the path where the given id is stored in the graph
		/// </summary>
		/// <param name="id"></param>
		/// <returns>index of path if found, -1 if unfound</returns>
		public int GetPathIndex(int id) {
            for (int i = 0; i < Graph.Count; i++) {
                List<int> path = Graph[i];
                if (path.Contains(id)) {
                    return i;
                }
            }
            return -1; // couldn't find path, return invalid path index
        }

		/// <summary>
		/// Gets the graph as a list of strings
		/// </summary>
		/// <returns>a list of strings representing all the reroute paths</returns>
        public List<string> TraverseGraph() {
            List<string> output = new List<string>();
            string redirects = "";
                   
            foreach(List<int> path in Graph) {
                List<string> pathNames = new List<string>();
                foreach (int id in path) {
                    pathNames.Add(_identifiers.Single(o => o.Value == id).Key);
                }
                redirects = string.Join(" -> ", pathNames);
                output.Add(redirects);
            }
            return output;
        }
        
		/// <summary>
		/// Test if adding a given route name to the path would create a circular reference
		/// </summary>
		/// <param name="routeName">the route name to be added</param>
		/// <param name="pathIndex">the index of the path where it will be added</param>
		/// <returns>True if it is unsafe, false if it is safe</returns>
        public bool CreatesCircularReference(string routeName, int pathIndex) {
            if (_identifiers.Keys.Contains(routeName)) {
                int id = _identifiers[routeName];
                return Graph[pathIndex].Contains(_identifiers[routeName]);
            }
            return false;
        }
        
		/// <summary>
		/// Splits a path into a usable list
		/// </summary>
		/// <param name="path">the input path</param>
		/// <returns>the outpute list of strings from the path</returns>
        public static List<string> ParseRoutes(string path) {
            return path.Split(_delimiters, System.StringSplitOptions.RemoveEmptyEntries).ToList(); 
        }

		/// <summary>
		/// Test wether or not two graphs are equal to eachother, 
		/// used for unit testing
		/// </summary>
		/// <param name="routeGraph">the graph to be compared against</param>
		/// <returns>True if equal, false if not</returns>
		public bool Equals(RouteGraph routeGraph) {
			if (routeGraph == null || routeGraph.Graph == null) {
				return false;
			}
			for (int i = 0; i < routeGraph.Graph.Count; i++) {
				List<int> path = routeGraph.Graph[i];
				for (int k = 0; k < path.Count; k++) {
					if (!path[k].Equals(Graph[i][k])) {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// a short hand property to make code more readable
		/// </summary>
        private int EndGraphIndex {
            get {
                return Graph.Count - 1;
            }
        }
    }
}