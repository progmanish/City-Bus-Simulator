using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
	[Header("Target")]
	public Transform target;

	[Header("Offset")]
	public Vector3 offset = new Vector3(0, 0, -10);
	public float followSpeed = 5f;

	[Header("Rotation Settings")]
	public float rotationSpeed = 20f;
	public float autoRotationSpeed = 4f;
	public float autoRotationDelay = 5f;

	[Header("GearBox")]
	public Slider gearBox;

	private float currentYaw = 0f;
	private float lastInputTime = 0f;
	private float lastGear = -1f;

	private void LateUpdate()
	{
		if (target == null) return;
		HandleManualRot();
		HandleAutoRot();
		HandlePosition();

		DragArea.swipeDelta = 0f;
	}

	void HandleManualRot()
	{
		if (Mathf.Abs(DragArea.swipeDelta) > 0.01f)
		{
			currentYaw += DragArea.swipeDelta * rotationSpeed * Time.deltaTime;
			lastInputTime = Time.time;
		}
	}

	void HandleAutoRot()
	{
		if (gearBox == null) return;

		bool gearChange = gearBox.value != lastGear;
		bool swipeIdle = Time.time - lastInputTime > autoRotationDelay;

		if (!gearChange && !swipeIdle) return;

		lastGear = gearBox.value;

		float targetYaw = (gearBox.value == 1) ? target.eulerAngles.y - 180f : target.eulerAngles.y;
		currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, autoRotationSpeed * Time.deltaTime);
	}

	void HandlePosition()
	{
		Quaternion targetRot = Quaternion.Euler(0, currentYaw, 0);
		Vector3 targetPos = target.position + targetRot * offset;

		transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
		transform.LookAt(target.position + Vector3.up * 1.5f);
	}
}