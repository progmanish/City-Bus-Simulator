using UnityEngine;
using UnityEngine.EventSystems;

public class DragArea : MonoBehaviour, IDragHandler
{
	public float sensetivity = 0.3f;
	public static float swipeDelta;

    public void OnDrag(PointerEventData eventData)
    {
        swipeDelta = eventData.delta.x * sensetivity;
    }

}