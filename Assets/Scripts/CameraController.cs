using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject exteriorCamera;
	private GameObject interiorCamera;
	private InteriorCameraController interiorCameraController;

	private Camera exteriorCameraComp;
	private bool wasInteriorBeforeStop;

	public bool isInterior
	{
		get
		{
			if (interiorCamera != null) return interiorCamera.activeSelf;
			return false;
		}
	}

	void Start()
	{
		StartCoroutine(InitializeCameras());
	}

	private System.Collections.IEnumerator InitializeCameras()
	{
		while (!FindAndSet())
		{
			yield return null;
		}
		SetExterior(); 
	}

	private bool FindAndSet()
	{
		if (exteriorCamera != null && interiorCamera != null) return true;

		FollowCamera fc = FindFirstObjectByType<FollowCamera>();
		if (fc != null)
		{
			exteriorCamera = fc.gameObject;
			exteriorCameraComp = fc.GetComponent<Camera>();
		}

		interiorCameraController = FindFirstObjectByType<InteriorCameraController>();
		if (interiorCameraController != null) interiorCamera = interiorCameraController.gameObject;

		return (exteriorCamera != null && interiorCamera != null);
	}

	public void ToogleCamera()
	{
		if (!FindAndSet()) return;

		if (isInterior)
		{
			SetExterior();
		}
		else
		{
			SetInterior();
		}
	}

	public void SetExterior()
	{
		if (!FindAndSet()) return;

		if (exteriorCameraComp != null)exteriorCameraComp.enabled = true;

		exteriorCamera.SetActive(true);
		interiorCamera.SetActive(false);

		if (interiorCameraController != null) interiorCameraController.SetActive(false);

	}

    public void SetInterior()
    {
		if (!FindAndSet()) return;

		if (exteriorCameraComp != null) exteriorCameraComp.enabled = true; 

        interiorCamera.SetActive(true);
        exteriorCamera.SetActive(true);

		if (interiorCameraController != null) interiorCameraController.SetActive(true);

    }

	public void EnterStopMode()
	{
		if (!FindAndSet()) return;

		wasInteriorBeforeStop = isInterior;
		if (exteriorCameraComp != null) exteriorCameraComp.enabled = false;
		if (interiorCamera != null) interiorCamera.SetActive(false);
		if (interiorCameraController != null) interiorCameraController.SetActive(false);
	}

	public void ExitStopMode()
	{
		if (wasInteriorBeforeStop)
		{
			SetInterior();
		}
		else
		{
			SetExterior();
		}
	}
}