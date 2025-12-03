using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 50;
    public int height = 50;
    public float cellSize = 0.5f;

    public Node[,] grid;

    void Awake()
    {
        Instance = this;
        GenerateGrid();
    }
    public void SetNodeWalkable(Vector2 worldPos, bool walkable)
    {
        Node node = WorldToNode(worldPos);
        node.walkable = walkable;
    }
    void GenerateGrid()
    {
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = GridToWorld(x, y);
                float obstacleRadius = cellSize * 0.4f;
                bool isBlocked = Physics2D.OverlapCircle(worldPos, obstacleRadius, LayerMask.GetMask("Obstacle"));

                grid[x, y] = new Node(x, y, worldPos, !isBlocked);
            }
        }
    }

    public Vector2 GridToWorld(int x, int y)
    {
        return new Vector2(x * cellSize, y * cellSize);
    }

    public Node WorldToNode(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);



        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        return grid[x, y];
    }
    void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = grid[x, y];

                if (node == null) continue;

                // Choix de la couleur selon walkable ou pas
                Gizmos.color = node.walkable ? Color.green : Color.red;

                // Dessine un petit cube à la position du node
                Gizmos.DrawWireCube(node.worldPos, Vector3.one * (cellSize ));
                //* 0.9f
            }
        }
    }
}

public class Node
{
    public int x, y;
    public Vector2 worldPos;
    public bool walkable;

    public Node(int x, int y, Vector2 worldPos, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.worldPos = worldPos;
        this.walkable = walkable;
    }
}
