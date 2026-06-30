using UnityEngine;

public class StopCameraController : MonoBehaviour
{
    [SerializeField] private Camera stopCam;
    [SerializeField] private Camera mainCam;

    public void EnableCamera()
    {
        if (mainCam != null) mainCam.enabled = false;
        if (stopCam != null) stopCam.enabled = true;
    }

    public void DisableCamera()
    {
        if (stopCam != null) stopCam.enabled = false;
        if (mainCam != null) mainCam.enabled = true;
    }
}
