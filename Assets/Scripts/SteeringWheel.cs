using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[Header("Steering Settings")]
	public float maxRotation = 180f;
	public float returnSpeed = 400f;

	public float steeringAmount { get; private set; }

	private RectTransform rectTransform;
	private bool isDragging;
	private float startAngle;
	private float currentAngle;

    // Start is called before the first frame update
    void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update()
	{
		if(!isDragging)
		{
			if (currentAngle != 0f)
			{
				currentAngle = Mathf.MoveTowards(currentAngle, 0f, returnSpeed * Time.deltaTime);
				ApplyRotation();
			}
		}
	}

    public void OnDrag(PointerEventData eventData)
    {
        float currentFingerAngle = GetAngle(eventData.position);
		float deltaAngle = Mathf.DeltaAngle(startAngle, currentFingerAngle);
		currentAngle += deltaAngle;
		steeringAmount = Mathf.Clamp(currentAngle / maxRotation, -1f, 1f);

		startAngle = currentFingerAngle;
		ApplyRotation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        startAngle = GetAngle(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

	void ApplyRotation()
	{
		rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(currentAngle, -maxRotation, maxRotation));
		steeringAmount = -currentAngle / maxRotation;
		GameManager.instance.SetSteering(steeringAmount);
	}

	float GetAngle(Vector2 screenPosition)
	{
		Vector2 center = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
		Vector2  direction = screenPosition - center;
		return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	}
}