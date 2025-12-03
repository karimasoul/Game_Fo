using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private bool canAttack = true;
    private Transform target = null;

    //
    private List<Node> path;
    private int index = 0;
    public float moveSpeed = 2f;
    private Vector2 lastTargetPos;
    public float repathCooldown = 0.5f;
    private float reparthTimer = 0f;




    void Update()
    {

        GameObject[] allies = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(obj => obj.name.StartsWith("allie_attack_distance_prefab"))
            .ToArray();


        if (allies.Length == 0)
        {
            target = null;
            return;
        }


        GameObject closest = allies
            .OrderBy(obj => Vector2.Distance(transform.position, obj.transform.position))
            .FirstOrDefault();

        target = closest.transform;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist <= attackRange && canAttack)
        {
            canAttack = false;
            Invoke(nameof(Attack), 2f);
            Invoke(nameof(ResetAttack), attackCooldown);
            return;
            // Attack();
        }
        if (dist > attackRange)
        {
            reparthTimer -= Time.deltaTime;
            
            if (path == null || path.Count == 0 || reparthTimer <= 0f)
            {
                ComputePathTo(target);
                reparthTimer = repathCooldown;
            }
            MoveOnPath();
        }
    }
        void MoveOnPath()
        {
            if (path == null || path.Count == 0 || index >= path.Count) return;

            Vector2 next = path[index].worldPos;
            transform.position = Vector2.MoveTowards(transform.position, next, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, next) < 0.1f)
                index++;
        }
        void ComputePathTo(Transform target)
        {
            Node start = GridManager.Instance.WorldToNode(transform.position);
            Node end = GridManager.Instance.WorldToNode(target.position);

        //path = AStarPathfinder.Instance.FindPath(start, end);
        path = Pathfinder.Instance.Dijkstra(start, end);
        if (path == null)
        {
            Debug.Log("A n a pas trouve");
            return;
        }
            index = 0;
        }

        
    void Attack()
    {
        if (target == null) return;

        AllyHealth allyHealth = target.GetComponent<AllyHealth>();
        if (allyHealth != null)
        {
           //new WaitForSeconds(2f);
            allyHealth.TakeDamage(damage);
        }

        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}