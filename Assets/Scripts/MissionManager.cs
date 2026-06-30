using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
	public static MissionManager instance;

	[Header("Mission Status")]
	public bool missionActive;
	public bool missionComplete;
	public bool missionFailed;

	[Header("Stop System")]
	[SerializeField] private int currentStopIndex;
	public List<BusStop> stops = new List<BusStop>();
	public Transform goal;
	private BusDirectionArrow[] directionArrow;

	// Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
    }

	public void StartMission()
	{
		if (missionActive) return;

		missionActive = true;
		missionComplete = false;
		missionFailed = false;
		directionArrow = null;
		directionArrow = GameManager.instance.activeBusController.GetComponentsInChildren<BusDirectionArrow>();

		currentStopIndex = 0;
		if (directionArrow != null)
		{
			foreach (var arrow in directionArrow)
			{
				arrow.SetTarget(stops[currentStopIndex].transform); 
			}
		}

		UIManager.instance.distanceHUD.SetTarget(stops[currentStopIndex].transform);

		foreach(var stop in stops)
		{
			stop.ResetStop();
		}

		Debug.Log($"<color=cyan>Mission Activated!</color>");
	}

	public void CompleteMission()
	{
		if (!missionActive) return;

		missionActive = false;
		missionComplete = true;

        Debug.Log($"<color=green>Mission Completed!</color>");
    }

	public void FailMission()
	{
		if (!missionActive) return;

		missionFailed = true;
		missionActive = false;

        Debug.Log($"<color=red>Mission Failed!</color>");
    }

	public void RegisterStop(BusStop stop)
	{
		if (!stops.Contains(stop))
		{
			stops.Add(stop);
		}
	}

    public void NotifyStopEntered(BusStop stop)
    {
        if (!missionActive) return;

        // safety check: index valid hai?
        if (currentStopIndex < 0 || currentStopIndex >= stops.Count)
        {
            Debug.LogWarning("Invalid stop index!");
            return;
        }

        if (stops[currentStopIndex] != stop)
        {
            Debug.Log("Wrong stop order!");
            FailMission();
            return;
        }

        Debug.Log("Correct stop entered: " + stop.name);
    }

    public void NotifyStopCompleted(BusStop stop)
    {
        if (!missionActive) return;

        if (currentStopIndex < 0 || currentStopIndex >= stops.Count)
        {
            Debug.LogWarning("Invalid stop index!");
            return;
        }

        if (stops[currentStopIndex] != stop) return;

        currentStopIndex++;
        Debug.Log("Stop Completed: " + stop.name);

        if (currentStopIndex < stops.Count)
        {
            if (directionArrow != null)
			{
                foreach (var arrow in directionArrow)
                {
                    arrow.SetTarget(stops[currentStopIndex].transform);
                }
				UIManager.instance.distanceHUD.SetTarget(stops[currentStopIndex].transform);
			}
            Debug.Log("Next stop: " + stops[currentStopIndex].name);
        }
        else
        {
            if (directionArrow != null)
            {
                foreach (var arrow in directionArrow)
                {
                    arrow.SetTarget(goal);
                }
                UIManager.instance.distanceHUD.SetTarget(goal);

            }
            Debug.Log("All Stops Completed");
            //CompleteMission(); // mission complete handler
        }
    }



    public bool AllStopsComplated()
	{
		return currentStopIndex >= stops.Count;
	}

	public void ResetMission()
	{
		missionActive = false;
		missionComplete = false;
		missionFailed = false;
		currentStopIndex = 0;

		foreach(var stop in stops)
		{
			stop.ResetStop();
		}

		Debug.Log("Mission Reset!");
	}
}