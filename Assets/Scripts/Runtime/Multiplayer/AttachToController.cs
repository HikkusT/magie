using UnityEngine;

public class AttachToController : MonoBehaviour
{
    public string controllerObjectName = "LeftControllerAnchor";

    void Start()
    {
        GameObject controller = GameObject.Find(controllerObjectName);
        if (controller != null)
        {
            transform.SetParent(controller.transform, worldPositionStays: false);
            transform.localPosition = new Vector3(0f, 0f, 0.15f);
        }
    }
}
