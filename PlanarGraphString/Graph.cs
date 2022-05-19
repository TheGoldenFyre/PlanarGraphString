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

    public string Neighbourhood(int index)
    {
        int n = EdgeCount(index);
        // Calculate the minimum square lengths to represent the neighbourhood
        int outputDimensions = (Math.Ceiling(Math.Sqrt(n + 1)) % 2 == 0)
            ? (int)Math.Ceiling(Math.Sqrt(n + 1)) + 1
            : (int)Math.Ceiling(Math.Sqrt(n + 1));
        if (outputDimensions < 3) outputDimensions = 3;

        string[,] values = new string[outputDimensions, outputDimensions];
        int[] colLengths = new int[outputDimensions];

        values[outputDimensions / 2, outputDimensions / 2] = "CURRENT";
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

        bool finalEdgeConnnected = false;
        for (int y = 0; y < outputDimensions; y++)
        {
            finalEdgeConnnected = false;
            for (int x = 0; x < outputDimensions; x++)
            {
                int offset = y * 4;
                int distanceBetweenNodes = 3;
                
                if (values[x, y] == null)
                {
                    if (y * outputDimensions + x > outputDimensions * outputDimensions / 2) continue;
                    
                    if (y > 0 && x == outputDimensions / 2 - 1)
                    {
                        string vertstring = new string(' ', colLengths[x] + 2) +
                                            new string(' ', (distanceBetweenNodes - 1) / 2) + "│" +
                                            new string(' ', distanceBetweenNodes / 2);
                        outputLines[offset] += vertstring;
                        outputLines[offset+1] += vertstring;
                        outputLines[offset+2] += vertstring;
                        outputLines[offset+3] += new string(' ',colLengths[x] + 2 + (distanceBetweenNodes - 1) / 2) + '└' + new string('─', distanceBetweenNodes / 2);
                    }
                    else
                    {
                        string s = new string(' ', colLengths[x] + 2 + distanceBetweenNodes);
                        outputLines[offset] += s;
                        outputLines[offset+1] += s;
                        outputLines[offset+2] += s;
                        outputLines[offset+3] += s;
                    };
                    continue;
                }

                bool finalInRow = x == outputDimensions - 1 || y * outputDimensions + x >= n;

                string toNextNode = (y > 0 && x == outputDimensions / 2 - 1) ? new string(' ', (distanceBetweenNodes - 1) / 2) + "│" + new string(' ', distanceBetweenNodes / 2) : new string(' ', distanceBetweenNodes);
                char vertEdgeConnect = (x != outputDimensions / 2 - 1) ? '─' : ((y == 0) ? '┬' : ((y == outputDimensions - 1) ? '┴' : '┼'));
                string toNextEdge = new string('─', (distanceBetweenNodes - 1) / 2) + vertEdgeConnect +
                                    new string('─', distanceBetweenNodes / 2);
                char edgeConnect = (x == 0) ? '└' : ((finalInRow) ? '┘' : '┴');
                char edgeRight = (finalInRow) ? ' ' : '─';
                if (vertEdgeConnect != '─' && edgeRight == '─') finalEdgeConnnected = true;
                char edgeLeft = (x == 0) ? ' ' : '─';
                
                
                outputLines[offset] += "┌" + new string('─', values[x, y].Length) + "┐" +
                                       new string(' ', colLengths[x] - values[x, y].Length) + toNextNode;
                outputLines[offset + 1] += "│" + values[x, y] + "│" +
                                           new string(' ', colLengths[x] - values[x, y].Length) + toNextNode;
                outputLines[offset + 2] += "└" + new string('─', (values[x, y].Length - 1) / 2) + "┬" + new string('─', (values[x, y].Length) / 2) + "┘" +
                                       new string(' ', colLengths[x] - values[x, y].Length) + toNextNode;
                outputLines[offset + 3] += new string(edgeLeft, (values[x, y].Length - 1) / 2 + 1) + edgeConnect +
                                           new string(edgeRight,
                                               (values[x, y].Length) / 2 + colLengths[x] - values[x, y].Length + 1) + ((edgeRight == '─') ? toNextEdge : "");

            }
        }
        
        if (!finalEdgeConnnected)
        {
            int connectionPoint = outputLines[outputLines.Length - 2].IndexOf("│");
            if (connectionPoint != -1)
            {
                string temp = outputLines[outputLines.Length - 1].TrimEnd();
                outputLines[outputLines.Length - 1] = temp + new string('─', connectionPoint - temp.Length) + "┘";
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