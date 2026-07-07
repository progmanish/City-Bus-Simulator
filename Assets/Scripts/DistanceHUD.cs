using UnityEngine;
using TMPro;

public class DistanceHUD : MonoBehaviour
{
	public Transform target;
	public TMP_Text text_Distance;
	public float updateInterval = 0.25f;

	Transform bus;
	float timer;

	// Start is called before the first frame update
	void Start()
	{
		if (GameManager.instance != null && GameManager.instance.activeBusController != null)
		{
			bus = GameManager.instance.activeBusController.transform;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (bus == null && GameManager.instance != null && GameManager.instance.activeBusController != null)
		{
			bus = GameManager.instance.activeBusController.transform;
		}
        if (target == null || bus == null)
		{
			if (text_Distance != null)
			{
				text_Distance.text = "0m";
			}
			return;
		}
		timer += Time.deltaTime;
		if (timer < updateInterval) return;
		timer = 0f;

		float distance = Vector3.Distance(bus.position, target.position);
		if (text_Distance != null)
		{
			if (distance >= 1000f)
			{
				text_Distance.text = (distance / 1000f).ToString("F1") + "km";
			}
			else
			{
				text_Distance.text = Mathf.RoundToInt(distance) + "m";
			}
		}
	}

	public void SetTarget(Transform stop)
	{
		target = stop;
	}

	public void ClearTarget()
	{
		target = null;
	}
}