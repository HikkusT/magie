using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers;
    
    public static bool isColliding = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            isColliding = true;
            Debug.Log("Collision detected with: " + collision.gameObject.name);
        }
        else
        {
            isColliding = false;
        }
    }
}
