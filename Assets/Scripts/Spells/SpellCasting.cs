using System;
using UnityEngine; // Include UnityEngine for basic functionality
using Oculus.Interaction.Input; // Include Oculus Interaction Input for hand tracking
using System.Collections.Generic; // Include System.Collections.Generic for using dictionaries
using System.Collections; // Include System.Collections for using coroutines

public class SpellCasting : MonoBehaviour
{
    
    // ------------------------------------------------

    // Initialize variables for hand tracking and reference objects
    [SerializeField] private Hand rightHand; // Right Hand tracking
    [SerializeField] private Transform magicCenterObject; // Reference object (sphere in the hand)
    [SerializeField] private Transform collisionDetectionPoint; // Sphere to show collision point
    [SerializeField] private LayerMask collisionLayers; // Layers to detect collisions with

    [SerializeField] private float offsetPerpendicularDistance = 0.06f; // Distance perpendicular to the hand
    [SerializeField] private float offsetTowardsFingertips = 0.08f; // Distance towards the fingertips
    [SerializeField] private float rayLength = 10f; // Length of the raycast
    [SerializeField] private float speedRotationDetectionPoint = 10f; // Speed of rotation for the collision detection point
    
    [SerializeField] private GameObject prefabSpellObjectCast; // Spell prefab to instantiate
    [SerializeField] private float spellMoveSpeed = 6f; // Speed at which the spell moves
    [SerializeField] private float secondsToDestroy = 3f; // Time before the spell is destroyed
    [SerializeField] private float distanceToTarget = 0.01f; // Distance to check for stopping the spell
    [SerializeField] private float timeNewSpellObjectCast = 0.5f; // Time to wait before casting a new spell

    private string spellObjectName; // Name of the spell object
    private int spellCounter = 0; // Counter for the number of spells cast
    
    // ------------------------------------------------

    // Class to store spell object positions
    [System.Serializable]
    public class SpellObjectPositions
    {
        public Vector3 initialPosition;
        public Vector3 finalPosition;

        public SpellObjectPositions(Vector3 initial, Vector3 final)
        {
            initialPosition = initial;
            finalPosition = final;
        }
    }

    // List to store spell object names and their target positions
    private Dictionary<string, SpellObjectPositions> dataBaseSpellObjectCast = new Dictionary<string, SpellObjectPositions>(); 

    // ------------------------------------------------

    // Coroutine to wait for a specified time before allowing a new spell to be cast
    IEnumerator WaitUntilSeconds()
    {
        yield return new WaitForSeconds(timeNewSpellObjectCast);
        spellCounter++;
    }

    // ------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        
        // Set the position and rotation of the magic center object based on the right hand's position
        SetSpellInitialFinalPositions();

    }

    // ------------------------------------------------

    void SetSpellInitialFinalPositions(){

        // Check if the right hand is connected and tracking
        if (rightHand != null && rightHand.IsConnected)
        {
            
            // Get the position and rotation of the palm
            Pose palmPose;
            rightHand.GetRootPose(out palmPose);

            // Calculate the position based on palm orientation
            Vector3 newPosition = palmPose.position - (palmPose.up * offsetPerpendicularDistance) + (palmPose.forward * offsetTowardsFingertips);

            // Position the reference object (sphere) in front of the palm
            magicCenterObject.position = newPosition;
            magicCenterObject.rotation = palmPose.rotation;

            // Define the ray direction as negative local Y (down from the palm)
            Vector3 rayDirection = -palmPose.up;

            // Perform the raycast
            Ray ray = new Ray(palmPose.position, rayDirection);
            RaycastHit hit;

            // Check if the ray hits any colliders in the specified layers
            if (Physics.Raycast(ray, out hit, rayLength, collisionLayers))
            {
                
                // Position the collision marker object at the hit point
                collisionDetectionPoint.position = hit.point;

                // Calculate the rotation to align the collision marker with the surface normal
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                collisionDetectionPoint.rotation = Quaternion.Slerp(collisionDetectionPoint.rotation, targetRotation, Time.deltaTime * speedRotationDetectionPoint);

                // Set the name of the spell object
                spellObjectName = "Spell_" + spellCounter;

                // Check if the spell object already exists in the scene
                if (!dataBaseSpellObjectCast.ContainsKey(spellObjectName))
                {

                    // Instantiate the spell object at the hit point
                    GameObject spell = Instantiate(prefabSpellObjectCast, magicCenterObject.position, Quaternion.identity);
                    spell.name = spellObjectName;

                    // Set the target position for the spell to move towards
                    SpellMovement movement = spell.AddComponent<SpellMovement>();
                    movement.Init(hit.point, spellMoveSpeed, secondsToDestroy, distanceToTarget);

                    // Save the initial and final positions of the spell object
                    dataBaseSpellObjectCast[spellObjectName] = new SpellObjectPositions(magicCenterObject.position, hit.point);

                    // Start the coroutine to wait before allowing a new spell to be cast
                    StartCoroutine(WaitUntilSeconds());

                }

            }

        }

    }

    private void OnEnable()
    {
        spellCounter = 0;
        dataBaseSpellObjectCast.Clear();
    }
}
