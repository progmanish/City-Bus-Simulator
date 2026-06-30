using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BusStop : MonoBehaviour
{
	private bool busInside;
	private bool completed;

	private PassengerSystem passengerSystem;

	[SerializeField] private StopCameraController stopCamera;

	// Start is called before the first frame update
	void Start()
	{
		if (MissionManager.instance != null)
			MissionManager.instance.RegisterStop(this);

        if (stopCamera != null)
        {
            stopCamera.DisableCamera();
            Debug.Log("Camera Disable!!");
        }
    }

	// Update is called once per frame
	void Update()
	{
		if (!busInside || completed) return;
		if (!MissionManager.instance.missionActive) return;

		if (passengerSystem != null && passengerSystem.boardingFinished && 
			!UIManager.instance.gatesOpen)
		{
			completed = true;
			busInside = false;

			UIManager.instance.ExitStopZone();
			MissionManager.instance.NotifyStopCompleted(this);
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		passengerSystem = other.GetComponentInParent<PassengerSystem>();
		if (passengerSystem == null) return;

		passengerSystem.boardingFinished = false;
		busInside = true;
		UIManager.instance.EnterStopZone();
		UIManager.instance.ShowHUD();
        UIManager.instance.SetStatusText("Arrived at bus stop...");
        UIManager.instance.SetPassengerText(passengerSystem.currentPassengers, passengerSystem.maxCapacity);
        if (stopCamera != null)
		{
			stopCamera.EnableCamera();
			Debug.Log("Camera Enable!!!");
		}

		MissionManager.instance.NotifyStopEntered(this);
		Debug.Log("<color=cyan>" + this.gameObject.ToString() + "</color>");
    }

    private void OnTriggerExit(Collider other)
    {
		PassengerSystem _ps = other.GetComponentInParent<PassengerSystem>();
		if(_ps == null) return;

		busInside = false;
		UIManager.instance.HideHUD();
		if(stopCamera != null)
		{
			stopCamera.DisableCamera();
		}
		UIManager.instance.ExitStopZone();
		_ps.ResetBoardingState();
    }

	public void ResetStop()
	{
		busInside = false;
		completed = false;
	}
}