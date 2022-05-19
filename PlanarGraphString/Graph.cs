namespace PlanarGraphString;

public class Graph<T>
{
    private List<T> V;
    private List<List<Edge>> E;

    public Graph()
    {
        V = new List<T>();
        E = new List<List<Edge>>();
    }
    
    /// <summary>
    /// The amount of vertexes in the graph
    /// </summary>
    public int VertexCount() => V.Count();
    
    /// <summary>
    /// The amount of edges going out of the vertex at the given index
    /// </summary>
    /// <param name="index">The index of the vertex to be checked</param>
    public int EdgeCount(int index) => E[index].Count();
    
    /// <summary>
    /// Adds a new vertex to the graph
    /// </summary>
    /// <param name="value"></param>
    public void AddVertex(T value)
    {
        V.Add(value);
        E.Add(new List<Edge>());
    }
    
    /// <summary>
    /// Adds an edge to the graph
    /// </summary>
    /// <param name="from">The index of the source vertex</param>
    /// <param name="to">The index of the target vertex</param>
    /// <param name="undirected">If set to true, an edge from target to source will also be added</param>
    public void AddEdge(int from, int to, bool undirected = true)
    {
        E[from].Add(new Edge(from, to));
        
        if (undirected)
            E[to].Add(new Edge(to, from));
    }
    
    /// <summary>
    /// Show the neighbourhood around a certain vertex
    /// </summary>
    /// <param name="index">Index to the vertex</param>
    /// <param name="displayIndexValue">The value to be displayed as the center vertex. If none is passed, the vertex value is used</param>
    /// <returns>A multiline string containing a visual representation all the vertexes connected to the vertex</returns>
    public string Neighbourhood(int index, string displayValue = null, bool drawEdges = false)
    {
        int n = EdgeCount(index);
        // Calculate the minimum square lengths to represent the neighbourhood
        int outputDimensions = (Math.Ceiling(Math.Sqrt(n + 1)) % 2 == 0)
            ? (int)Math.Ceiling(Math.Sqrt(n + 1)) + 1
            : (int)Math.Ceiling(Math.Sqrt(n + 1));
        if (outputDimensions < 3) outputDimensions = 3;

        string[,] values = new string[outputDimensions, outputDimensions];
        int[] colLengths = new int[outputDimensions];

        values[outputDimensions / 2, outputDimensions / 2] = (displayValue == null) ? V[index].ToString() : displayValue;
        colLengths[outputDimensions / 2] = values[outputDimensions / 2, outputDimensions / 2].Length;
        
        for (int i = 0; i < n; i++)
        {
            string valueString = V[E[index][i].To].ToString();

            int writeIndex = (i < outputDimensions * outputDimensions / 2) ? i : i + 1;
                
            values[writeIndex % outputDimensions, writeIndex / outputDimensions] = valueString;
            if (valueString.Length > colLengths[writeIndex % outputDimensions]) colLengths[writeIndex % outputDimensions] = valueString.Length;
        }
        
        string[] outputLines = new string[4 * outputDimensions];
        for (int i = 0; i < outputLines.Length; i++) outputLines[i] = "";
        
        for (int y = 0; y < outputDimensions; y++)
        {
            for (int x = 0; x < outputDimensions; x++)
            {
                int offset = y * 4;
                int distanceBetweenNodes = 3;
                
                if (values[x, y] == null)
                {
                    if (y * outputDimensions + x > outputDimensions * outputDimensions / 2) continue;

                    else 
                    {
                        string vertstring = new string(' ', colLengths[x] + 2 + distanceBetweenNodes);
                        
                        outputLines[offset] += vertstring;
                        outputLines[offset+1] += vertstring;
                        outputLines[offset+2] += vertstring;
                        outputLines[offset+3] += vertstring;
                    }
                    continue;
                }

                string leftOffset = new string(' ', (colLengths[x] - values[x, y].Length) / 2);
                string rightOffset = new string(' ', (colLengths[x] - values[x, y].Length + 1) / 2);
                string betweenNodes = new string(' ', distanceBetweenNodes);


                outputLines[offset] += leftOffset + "┌" + new string('─', values[x, y].Length) + "┐" +
                                       rightOffset + betweenNodes;
                outputLines[offset + 1] += leftOffset + "│" + values[x, y] + "│" +
                                           rightOffset + betweenNodes;
                outputLines[offset + 2] += leftOffset + "└" + new string('─', values[x, y].Length) + "┘" +
                                           rightOffset + betweenNodes;
                outputLines[offset + 3] += new string(' ', colLengths[x] + distanceBetweenNodes);

            }
        }
        
        return string.Join("\n", outputLines);
    }
}

internal class Edge
{
    public int To;
    public int From;

    public Edge(int from, int to)
    {
        From = from;
        To = to;
    }
}