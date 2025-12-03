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
                //float alt = dist[current] + 1f;

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


        //bool up = y + 1 < GridManager.Instance.height && grid[x, y + 1].walkable;
        //bool down = y - 1 >= 0 && grid[x, y - 1].walkable;
        //bool right = x + 1 < GridManager.Instance.width && grid[x + 1, y].walkable;
        //bool left = x - 1 >= 0 && grid[x - 1, y].walkable;

        //if (right) TryAdd(x + 1, y);
        //if (left) TryAdd(x - 1, y);
        //if (up) TryAdd(x, y + 1);
        //if (down) TryAdd(x, y - 1);

        
        //if (up && right) TryAdd(x + 1, y + 1);
        //if (up && left) TryAdd(x - 1, y + 1);
        //if (down && right) TryAdd(x + 1, y - 1);
        //if (down && left) TryAdd(x - 1, y - 1);

        //return list;

        TryAdd(x + 1, y);
        TryAdd(x - 1, y);
        TryAdd(x, y + 1);
        TryAdd(x, y - 1);

        //TryAdd(x + 1, y + 1);
        //TryAdd(x - 1, y + 1);
        //TryAdd(x + 1, y - 1);
        //TryAdd(x - 1, y - 1);
        return list;
    }
}



    
    
