using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitAttackDistance : MonoBehaviour
{
    private Transform[] target;
    public float moveSpeed = 2f;
    public float attackRange = 2.5f;
    public float attackCooldown = 1f;

    public GameObject projectilePrefab;
    public Transform shootPoint;

    private bool canAttack = true;
    string[] names;

    
    private List<Node> currentPath;
    private int pathIndex = 0;
    private float repathCooldown = 0.5f;
    private float repathTimer = 0f;


    

    private void Start()
    {
        Invoke(nameof(SnapToClosesNode), 0.05f);
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<Transform> list = new List<Transform>();

        names = System.Array.FindAll(allObjects, obj => obj.name.StartsWith("ok"))
                            .Select(obj => obj.name)
                            .ToArray();

        foreach (string n in names)
        {
            GameObject obj = GameObject.Find(n);
            if (obj != null)
                list.Add(obj.transform);
        }

        target = list.ToArray();


        foreach (string n in names)
        {

            GameObject obj = GameObject.Find(n);
            if(obj != null)
            {
                list.Add (obj.transform);
            }
        }

    }
    void SnapToClosesNode()
    {
      Node nearest = GridManager.Instance.WorldToNode(transform.position);
        if (nearest == null) { return; }
        transform.position = new Vector3(nearest.worldPos.x, nearest.worldPos.y);
    }
    
    void Update()
    {
        List<Transform> aliveTargets = new List<Transform>();
        foreach (Transform t in target)
            if (t != null)
                aliveTargets.Add(t);

        target = aliveTargets.ToArray();
        if (target.Length == 0) return;

        Transform closest = GetClosestTarget();
        if (closest == null) return;

        Vector2 dir = (closest.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            1f,
            LayerMask.GetMask("Obstacle")
        );

        if (hit.collider != null)
        {
            GameObject rempart = hit.collider.gameObject;

            
            if (!names.Contains(rempart.name))
            {
                
                var temp = names.ToList();
                temp.Add(rempart.name);
                names = temp.ToArray();

                
                var newList = target.ToList();
                newList.Add(rempart.transform);
                target = newList.ToArray();
            }

           
            closest = rempart.transform;

           
            float distRempart = Vector2.Distance(transform.position, closest.position);
            if (distRempart <= attackRange)
            {
                if (canAttack)
                    StartCoroutine(Shoot(closest));

                return; 
            }
        }
       

        float dist = Vector3.Distance(transform.position, closest.position);

        if (dist > attackRange)
        {
          
            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                ComputePathTo(closest);
                repathTimer = repathCooldown;
            }

            if (currentPath != null && currentPath.Count > 0 && pathIndex < currentPath.Count)
            {
                //a supp
                //
                Node nextNode = currentPath[pathIndex];
                if (!nextNode.walkable)
                {
                    GameObject rempart = null;
                    Collider2D col = Physics2D.OverlapCircle(nextNode.worldPos, 0.1f, LayerMask.GetMask("Obstacle"));
                    if (col != null)
                        rempart = col.gameObject;
                    if (rempart != null && canAttack)
                    {
                        StartCoroutine(Shoot(rempart.transform));
                    }
                    return;

                }
                Vector2 currentPos = transform.position;
                Vector2 targetPos = nextNode.worldPos;

               
                Vector2 moveStep = currentPos;

                if (Mathf.Abs(targetPos.x - currentPos.x) > Mathf.Abs(targetPos.y - currentPos.y))
                {
                    
                    moveStep.x = Mathf.MoveTowards(currentPos.x, targetPos.x, moveSpeed * Time.deltaTime);
                }
                else
                {
                    moveStep.y = Mathf.MoveTowards(currentPos.y, targetPos.y, moveSpeed * Time.deltaTime);
                }

                transform.position = moveStep;

                
                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                    pathIndex++;

                return; 
            }
            
           
        }
        else
        {
            if (canAttack)
                StartCoroutine(Shoot(closest));
        }
    }

    // a sup
    Node FindClosestWalkableToTarget(Node target)
    {
        if (target.walkable) return target;

        Node[,] grid = GridManager.Instance.grid;
        int searchRadius = 1;

        while (true)
        {
            for (int dx = -searchRadius; dx <= searchRadius; dx++)
            {
                for (int dy = -searchRadius; dy <= searchRadius; dy++)
                {
                    int nx = target.x + dx;
                    int ny = target.y + dy;

                    if (nx >= 0 && ny >= 0 && nx < GridManager.Instance.width && ny < GridManager.Instance.height)
                    {
                        Node n = grid[nx, ny];
                        if (n.walkable) return n;
                    }
                }
            }
            searchRadius++;
            if (searchRadius > Mathf.Max(GridManager.Instance.width, GridManager.Instance.height))
                break;
        }

        return target; 
    }
    //a supp
    void ComputePathTo(Transform target)
    {
        Node start = GridManager.Instance.WorldToNode(transform.position);
        Node end = GridManager.Instance.WorldToNode(target.position);

       
        float startTime = Time.realtimeSinceStartup;
        
        currentPath = Pathfinder.Instance.Dijkstra(start, end);

        float endTime = Time.realtimeSinceStartup;
        float elapsed = endTime - startTime;

        pathIndex = 0;

        Debug.Log($"ALLIE: Chemin trouvé avec Dijikstra en {elapsed:F4} secondes, {currentPath.Count} nodes");
    }

    Transform GetClosestTarget()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in target)
        {
            if (t == null) continue;

            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }
        return closest;
    }

    private System.Collections.IEnumerator Shoot(Transform closest)
    {
        canAttack = false;
        Transform tgt = GetClosestTarget();

        if (tgt != null)
        {
            Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity)
                .GetComponent<Projectile>()
                .SetTarget(tgt);
        }
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}