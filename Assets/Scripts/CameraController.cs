using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject exteriorCamera;
	public GameObject interiorCamera;

	public InteriorCameraController interiorCameraController;

	bool isInterior;
	// Start is called before the first frame update
	void Start()
	{
		exteriorCamera = GameObject.FindAnyObjectByType<FollowCamera>().gameObject;
		interiorCameraController = Object.FindAnyObjectByType<InteriorCameraController>();
		interiorCamera = interiorCameraController.gameObject;
		SetExterior();
	}

	public void ToogleCamera()
	{
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
		isInterior = false;
		exteriorCamera.SetActive(true);
		interiorCamera.SetActive(false);

		interiorCameraController.SetActive(false);
	}

    public void SetInterior()
    {
        isInterior = true;
        interiorCamera.SetActive(true);
        exteriorCamera.SetActive(true);

        interiorCameraController.SetActive(true);
    }
}