using UnityEngine;

public class StopCameraController : MonoBehaviour
{
    [SerializeField] private Camera stopCam;
    [SerializeField] private Camera mainCam;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void EnableCamera()
    {
        CameraController cc = FindFirstObjectByType<CameraController>();
        if (cc != null)
        {
            cc.EnterStopMode();
        }
        else
        {
            // Fallback default logic if CameraController is missing
            if (mainCam == null) mainCam = Camera.main;
            if (mainCam != null) mainCam.enabled = false;
        }

        if (stopCam != null) stopCam.enabled = true;
    }

    public void DisableCamera()
    {
        if (stopCam != null) stopCam.enabled = false;

        CameraController cc = FindFirstObjectByType<CameraController>();
        if (cc != null)
        {
            cc.ExitStopMode();
        }
        else
        {
            // Fallback default logic
            if (mainCam == null) mainCam = Camera.main;
            if (mainCam != null) mainCam.enabled = true;
        }
    }
}
