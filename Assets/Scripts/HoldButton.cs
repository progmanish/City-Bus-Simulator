using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public UnityEvent OnHold;
	public UnityEvent OnRelease;

	public void OnPointerDown(PointerEventData eventData)
	{
		OnHold.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnRelease.Invoke();
	}

}