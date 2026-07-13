using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BusStop : MonoBehaviour
{
	private bool busInside;
	private bool completed;

	private PassengerSystem passengerSystem;

	[SerializeField] private StopCameraController stopCamera;
	[SerializeField] private GameObject NPCs;

	// Start is called before the first frame update
	void Start()
	{
        if (stopCamera != null)
        {
            stopCamera.DisableCamera();
            //Debug.Log("Camera Disable!!");
        }
		NPCs.SetActive(false);
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
		NPCs.SetActive(true);

		// Mission Active logic: Empty bus triggers route selection only if mission is not active
		if (passengerSystem.currentPassengers == 0 && MissionManager.instance != null && !MissionManager.instance.missionActive)
		{
			if (UIManager.instance != null && UIManager.instance.routeSelectionPanel != null)
			{
				UIManager.instance.routeSelectionPanel.SetActive(true);
			}
			//Debug.Log("<color=cyan>Mission Active Triggered (0 passengers)!</color>");
			return;
		}

		// Regular stop logic continues:
		passengerSystem.boardingFinished = false;
		busInside = true;
		UIManager.instance.EnterStopZone();
		UIManager.instance.ShowHUD();
        UIManager.instance.SetPassengerText(passengerSystem.currentPassengers, passengerSystem.maxCapacity);

		// Mission Complete logic: Last stop triggers mission completion
		if (MissionManager.instance != null && MissionManager.instance.IsLastStop(this))
		{
			MissionManager.instance.CompleteMission();
			UIManager.instance.SetStatusText("Arrived at final stop. Open the doors to deboard...");
		}
		else
		{
			UIManager.instance.SetStatusText("Arrived at bus stop...");
			if (stopCamera != null)
			{
				stopCamera.EnableCamera();
				//Debug.Log("Camera Enable!!!");
			}
			MissionManager.instance.NotifyStopEntered(this);
		}
		//Debug.Log("<color=cyan>" + this.gameObject.ToString() + "</color>");
    }

    private void OnTriggerExit(Collider other)
    {
		PassengerSystem _ps = other.GetComponentInParent<PassengerSystem>();
		if(_ps == null) return;
		NPCs.SetActive(false);

		// Hide route selection if empty bus leaves active trigger zone (only if mission is not active)
		if (_ps.currentPassengers == 0 && MissionManager.instance != null && !MissionManager.instance.missionActive)
		{
			if (UIManager.instance != null && UIManager.instance.routeSelectionPanel != null)
			{
				UIManager.instance.routeSelectionPanel.SetActive(false);
			}
		}

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