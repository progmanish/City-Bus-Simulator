using UnityEngine;

public class BusDirectionArrow : MonoBehaviour
{
	public Transform target;
	public float rotationSpeed = 8f;

	void Update()
	{
		if (target == null) return;
		Vector3 direction = target.position - transform.position;
		direction.y = 0f;

		if (direction.sqrMagnitude < 0.001f) return;

		Quaternion targetRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

	}

    public void SetTarget(Transform stop) => target = stop;
    public void ClearTarget() => target = null;
}