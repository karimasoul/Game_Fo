using UnityEngine;
using UnityEngine.Rendering;

public class teste_drag : MonoBehaviour
{
    [SerializeField] Transform mouse;
    public GameObject prefabToSpawn;
    public Transform ennemy;
   

    private void OnMouseDrag()
    {
       
        

            transform.position = mouse.position;
            
        
    }
    private void OnMouseUp()
    {
        if(prefabToSpawn != null)
        {
            GameObject newUnit= Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

}
