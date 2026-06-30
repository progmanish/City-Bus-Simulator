using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
	public float rotationSpeed = 0.5f;
	public float maxRotation = 60;
	public float returnSpeed = 0.5f;
	public float returnTime = 3f;
	float timer;
	float currentRotation;
	bool isActive = true;
	
	public void SetActive(bool active)
	{
		isActive = active;
		currentRotation = 0;
		Quaternion rot = Quaternion.Euler(7f, 180, 0);
		transform.localRotation = rot;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isActive) return;
		float swipeX = DragArea.swipeDelta;

		if (Mathf.Abs(swipeX) > 0.01f)
		{
			timer = 0f;
            currentRotation += swipeX * rotationSpeed;
            currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);
        }
		else
		{
			timer += Time.deltaTime;
			if (timer > returnTime)
			{
				currentRotation = Mathf.Lerp(currentRotation, 0, returnSpeed * Time.deltaTime);
			}
		}
		transform.localRotation = Quaternion.Euler(7f, 180 + currentRotation, 0);
	}
}