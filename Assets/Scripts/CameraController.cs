using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject exteriorCamera;
	private GameObject interiorCamera;

	private InteriorCameraController interiorCameraController;

	bool isInterior;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(FindAndSet());
	}

	IEnumerator FindAndSet()
	{
		yield return new WaitForSeconds(0.1f);
        exteriorCamera = GameObject.FindAnyObjectByType<FollowCamera>().gameObject;
        if (interiorCamera == null) interiorCameraController = GameObject.FindAnyObjectByType<InteriorCameraController>();
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