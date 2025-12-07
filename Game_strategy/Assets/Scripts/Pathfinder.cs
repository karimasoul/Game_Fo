using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance;

    void Awake() => Instance = this;

    public List<Node> Dijkstra(Node start, Node target)
    {
       
        Dictionary<Node, float> dist = new();
        Dictionary<Node, Node> prev = new();
        List<Node> unvisited = new();

        foreach (Node node in GridManager.Instance.grid)
        {
            dist[node] = Mathf.Infinity;
            unvisited.Add(node);
        }

        dist[start] = 0;

        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => dist[a].CompareTo(dist[b]));
            Node current = unvisited[0];
            unvisited.RemoveAt(0);

            if (current == target)
                break;

            foreach (Node neigh in GetNeighbors(current))
            {
                if (!neigh.walkable) continue;

                float alt = dist[current] + Vector2.Distance(current.worldPos, neigh.worldPos);
                

                if (alt < dist[neigh])
                {
                    dist[neigh] = alt;
                    prev[neigh] = current;
                }
            }
        }

        List<Node> path = new();
        Node step = target;

        while (prev.ContainsKey(step))
        {
            path.Add(step);
            step = prev[step];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> list = new();
        int x = node.x;
        int y = node.y;
        Node[,] grid = GridManager.Instance.grid;

        void TryAdd(int nx, int ny)
        {
            if (nx >= 0 && ny >= 0 && nx < GridManager.Instance.width && ny < GridManager.Instance.height)
                list.Add(grid[nx, ny]);
        }


        TryAdd(x + 1, y);
        TryAdd(x - 1, y);
        TryAdd(x, y + 1);
        TryAdd(x, y - 1);
        return list;
    }
}



    
    
