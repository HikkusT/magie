using UnityEngine; // Include UnityEngine for basic functionality
using System.Collections; // Include System.Collections for using coroutines

// This class handles the movement of a spell object in Unity
public class SpellMovement : MonoBehaviour
{
    
    // ------------------------------------------------

    private Vector3 target; // Target position for the spell to move to
    private float speed; // Speed at which the spell moves
    private float destroyDelay; // Time before the spell is destroyed
    private float distanceThreshold; // Distance to check for stopping the spell

    // Initialize the spell movement with target position, speed, destroy delay, and distance threshold
    public void Init(Vector3 target, float speed, float destroyDelay, float distanceThreshold)
    {
        this.target = target;
        this.speed = speed;
        this.destroyDelay = destroyDelay;
        this.distanceThreshold = distanceThreshold;

        // Start the coroutine to move the spell towards the target
        StartCoroutine(MoveToTarget());

    }

    // ------------------------------------------------

    // Coroutine to move the spell towards the target position
    IEnumerator MoveToTarget()
    {
        
        // Move the spell towards the target position until it reaches the distance threshold
        while (Vector3.Distance(transform.position, target) > distanceThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        // Destroy the spell object after the specified delay
        Destroy(gameObject, destroyDelay);

    }

}
