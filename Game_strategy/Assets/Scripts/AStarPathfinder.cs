using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    public static AStarPathfinder Instance;

    void Awake() => Instance = this;

    public List<Node> FindPath(Node start, Node goal)
    {
        List<Node> openSet = new List<Node> { start };
        HashSet<Node> closedSet = new HashSet<Node>();

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        Dictionary<Node, float> fScore = new Dictionary<Node, float>();

        foreach (Node n in GridManager.Instance.grid)
        {
            gScore[n] = Mathf.Infinity;
            fScore[n] = Mathf.Infinity;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Node current = openSet[0];

            if (current == goal)
                return Reconstruct(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neigh in GetNeighbors(current))
            {
                if (!neigh.walkable || closedSet.Contains(neigh))
                    continue;

                float tentativeG = gScore[current] + Vector2.Distance(current.worldPos, neigh.worldPos);

                if (!openSet.Contains(neigh))
                    openSet.Add(neigh);
                else if (tentativeG >= gScore[neigh])
                    continue;

                cameFrom[neigh] = current;
                gScore[neigh] = tentativeG;
                fScore[neigh] = tentativeG + Heuristic(neigh, goal);
            }
        }

        return new List<Node>(); // No path
    }

    float Heuristic(Node a, Node b)
    {
        return Vector2.Distance(a.worldPos, b.worldPos);
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> list = new List<Node>();
        Node[,] grid = GridManager.Instance.grid;

        int x = node.x;
        int y = node.y;

        void Add(int nx, int ny)
        {
            if (nx >= 0 && ny >= 0 && nx < GridManager.Instance.width && ny < GridManager.Instance.height)
                list.Add(grid[nx, ny]);
        }

        // 4 directions (pas de diagonale)
        Add(x + 1, y);
        Add(x - 1, y);
        Add(x, y + 1);
        Add(x, y - 1);

        return list;
    }

    List<Node> Reconstruct(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> path = new List<Node> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}