using PlanarGraphString;

Graph<int> graph = new Graph<int>();

for (int i = 0; i < 17; i++)
{
    graph.AddVertex(i);
    if (i != 0)
        graph.AddEdge(0, i);
}

Console.WriteLine(graph.Neighbourhood(0));

graph.AddVertex(100);
graph.AddEdge(0, graph.VertexCount()-1);


Console.WriteLine(graph.Neighbourhood(0));

