using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirectsExercise
{
    public class RouteGraph : RouteAnalyzer
    {
        public List<List<int>> Graph { get; set; } = new List<List<int>>();
        public Dictionary<string, int> Identifiers { get; set; } = new Dictionary<string, int>();

        private int uniqueIdentifier = 0;
        private string[] delimiters = { "->", " "};

        public RouteGraph() {

        }
    
        //I have to inject these values for the class to be unit testable... 
        public RouteGraph(List<List<int>> graph, Dictionary<string, int> identifiers) {
            Graph = graph;
            Identifiers = identifiers;
            uniqueIdentifier = identifiers.Count;
        }

        public IEnumerable<string> Process(IEnumerable<string> routes) {
            foreach (string path in routes) {
                MapPath(path);
            }
            return TraverseGraph();
        }

        public void MapPath(string path) { 
            List<string> routeNames = ParseRoutes(path);
            //if the end of this route path already exists in the graph
            if (Identifiers.Keys.Contains(routeNames[routeNames.Count - 1])) {
                int id = Identifiers[routeNames[routeNames.Count - 1]];
                int pathIndex = GetPathIndex(id);

                //add the new path to the graph
                Graph.Add(new List<int>());
                for (int i = 0; i < routeNames.Count - 1; i++) {
                    if (CreatesCircularReference(routeNames[i], pathIndex)) {
                        throw new ArgumentException("Circular Path Detected, infinite loop eminent");
                    }
                    else {
                        Identifiers[routeNames[i]] = uniqueIdentifier;
                        Graph[EndGraphIndex].Add(uniqueIdentifier++);
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
            else if (Identifiers.Keys.Contains(routeNames[0])) {
                int id = Identifiers[routeNames[0]];
                int pathIndex = GetPathIndex(id);

                for (int i = 1; i < routeNames.Count; i++) {
                    if (CreatesCircularReference(routeNames[i], pathIndex)) { 
                        throw new ArgumentException("Circular Path Detected, infinite loop eminent");
                    }
                    else {
                        Identifiers[routeNames[i]] = uniqueIdentifier;
                        Graph[pathIndex].Add(uniqueIdentifier++);
                    }
                }
            }
            //this route path is unique
            else {
                Graph.Add(new List<int>());
                foreach (string route in routeNames) {
                    Identifiers[route] = uniqueIdentifier;
                    Graph[EndGraphIndex].Add(uniqueIdentifier++);
                }
            }
        }

        public int GetPathIndex(int id) {
            for (int i = 0; i < Graph.Count; i++) {
                List<int> path = Graph[i];
                if (id == path[path.Count - 1] || id < path[path.Count - 1]) {
                    return i;
                }
            }
            return -1; // couldn't find path, return invalid path index
        }

        public List<string> TraverseGraph() {
            List<string> output = new List<string>();
            string redirects = "";
            foreach(List<int> path in Graph) {
                redirects = string.Join(" <- ", path);
                output.Add(redirects);
            }
            return output;
        }
        
        public bool CreatesCircularReference(string routeName, int pathIndex) {
            if (Identifiers.Keys.Contains(routeName)) {
                int id = Identifiers[routeName];
                return Graph[pathIndex].Contains(Identifiers[routeName]);
            }
            return false;
        }
        
        public List<string> ParseRoutes(string path) {
            return path.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries).ToList(); 
        }

        public int EndGraphIndex {
            get {
                return Graph.Count - 1;
            }
        }
    }
}