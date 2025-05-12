using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public Transform xrRig; // OVRCameraRig
    public float rotationSpeed = 0f; // How fast the camera rotates
    public float speed = 3f;
    public float sprintMultiplier = 6f; // Speed multiplier when sprinting

    void Update()
    {
        // Get the device of the left hand controller
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftHand.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftRotation))
        {
            // Extract the forward direction in the horizontal plane
            Vector3 forward = leftRotation * Vector3.forward;
            forward.y = 0; // Only rotation on the XZ plane
            forward.Normalize();

            if (forward.sqrMagnitude > 0.01f)
            {
                // Calculate the target rotation towards that direction
                Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);

                // Smoothly rotate the XR Rig
                xrRig.rotation = Quaternion.Slerp(xrRig.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        Vector2 inputAxis = Vector2.zero;
        if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            // Get movement direction based on player's view (main camera)
            Vector3 headForward = Camera.main.transform.forward;
            headForward.y = 0;
            headForward.Normalize();

            Vector3 headRight = Camera.main.transform.right;
            headRight.y = 0;
            headRight.Normalize();

            // Check if the grip button is pressed
            bool isSprinting = false;
            if (leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
            {
                isSprinting = gripPressed;
            }

            float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

            Vector3 moveDirection = (headForward * inputAxis.y + headRight * inputAxis.x) * currentSpeed * Time.deltaTime;
            xrRig.position += moveDirection;
        }
    }
}
