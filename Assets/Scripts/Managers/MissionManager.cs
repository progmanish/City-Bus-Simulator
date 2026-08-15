using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
	public static MissionManager instance;

	public static event System.Action<RouteData> OnMissionStarted;
	public static event System.Action OnMissionCompleted;
	public static event System.Action OnMissionFailed;
	public static event System.Action<BusStop, int> OnStopCompleted;

	[Header("Mission Status")]
	public bool missionActive;
	public bool missionComplete;
	public bool missionFailed;

	[Header("Stop System")]
	[SerializeField] private int currentStopIndex;
	public List<RouteData> routeData = new List<RouteData>();
	public Transform goal;
	private BusDirectionArrow[] directionArrow;
	private RouteData currentRoute;
	private int passengerServed = 0;
	private int totalIncome = 0;

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

    private void OnEnable()
    {
        PassengerSystem.OnBoardPassenger += BoardPassenger;
        PassengerSystem.OnAddIncome += AddIncone;
    }

    private void OnDisable()
    {
        PassengerSystem.OnBoardPassenger -= BoardPassenger;
        PassengerSystem.OnAddIncome -= AddIncone;
    }

	public void SelectRoute(int index)
	{
		if (index < 0 || index >= routeData.Count) return;
		currentRoute = routeData[index];
		StartMission();
		UIManager.instance.routeSelectionPanel.SetActive(false);
	}

	public void StartMission()
	{
		if (missionActive) return;

		missionActive = true;
		missionComplete = false;
		missionFailed = false;
		passengerServed = 0;
		totalIncome = 0;
		directionArrow = null;
		directionArrow = GameManager.instance.activeBusController.GetComponentsInChildren<BusDirectionArrow>();

		if (currentRoute != null && currentRoute.busStops != null && currentRoute.busStops.Length > 0)
		{
			goal = currentRoute.busStops[currentRoute.busStops.Length - 1].transform;
		}

		if (UIManager.instance != null)
		{
			UIManager.instance.isFinalStop = false;
		}

		currentStopIndex = 0;
		if (currentRoute != null && currentRoute.busStops != null)
		{
			Transform busTransform = GameManager.instance.activeBusController != null ? GameManager.instance.activeBusController.transform : null;
			if (busTransform != null)
			{
				for (int i = 0; i < currentRoute.busStops.Length; i++)
				{
					if (currentRoute.busStops[i] != null)
					{
						float dist = Vector3.Distance(busTransform.position, currentRoute.busStops[i].transform.position);
						if (dist < 20f)
						{
							currentStopIndex = i + 1;
							break;
						}
					}
				}
			}
			currentStopIndex = Mathf.Clamp(currentStopIndex, 0, currentRoute.busStops.Length - 1);
		}

		if (directionArrow != null)
		{
			foreach (var arrow in directionArrow)
			{
				arrow.SetTarget(currentRoute.busStops[currentStopIndex].transform); 
			}
		}

		UIManager.instance.distanceHUD.SetTarget(currentRoute.busStops[currentStopIndex].transform);

		foreach(var stop in currentRoute.busStops)
		{
			stop.ResetStop();
		}

		OnMissionStarted?.Invoke(currentRoute);
		//Debug.Log($"<color=cyan>Mission Activated!</color>");
	}

    public void BoardPassenger(int amount) => passengerServed += amount;
    public void AddIncone(int amount) => totalIncome += (amount * currentRoute.FarePerStop);
    public string MissionName() => currentRoute.name;
	public int Passengers() => passengerServed;
	public int Income() => totalIncome;

    public void CompleteMission()
	{
		if (!missionActive) return;

		missionActive = false;
		missionComplete = true;

		if (UIManager.instance != null)
		{
			UIManager.instance.isFinalStop = true;
		}
		UIManager.instance.distanceHUD.ClearTarget();
        GameManager.instance.activeBusController.OnOffDirectionIndictor(missionActive);
		UIManager.instance.distanceHUD.ClearTarget();
		OnMissionCompleted?.Invoke();
        //Debug.Log($"<color=green>Mission Completed!</color>");

    }

    public void FailMission()
	{
		if (!missionActive) return;

		missionFailed = true;
		missionActive = false;
		GameManager.instance.activeBusController.OnOffDirectionIndictor(missionActive);
		OnMissionFailed?.Invoke();
        //Debug.Log($"<color=red>Mission Failed!</color>");
    }



    public void NotifyStopEntered(BusStop stop)
    {
        if (!missionActive) return;
        if (currentStopIndex < 0 || currentStopIndex >= currentRoute.busStops.Length) return;

        if (currentRoute.busStops[currentStopIndex] != stop)
        {
            FailMission();
            return;
        }

    }

    public void NotifyStopCompleted(BusStop stop)
    {
        if (!missionActive) return;
        if (currentStopIndex < 0 || currentStopIndex >= currentRoute.busStops.Length) return;

        if (currentRoute.busStops[currentStopIndex] != stop) return;

        currentStopIndex++;
        if (currentStopIndex < currentRoute.busStops.Length)
        {
            if (directionArrow != null)
			{
                foreach (var arrow in directionArrow)
                {
                    arrow.SetTarget(currentRoute.busStops[currentStopIndex].transform);
                }
				UIManager.instance.distanceHUD.SetTarget(currentRoute.busStops[currentStopIndex].transform);
			}
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

        }
        OnStopCompleted?.Invoke(stop, currentStopIndex);
    }

    public bool AllStopsComplated()
	{
		return currentStopIndex >= currentRoute.busStops.Length;
	}

	public bool IsLastStop(BusStop stop)
	{
		if (currentRoute == null || currentRoute.busStops == null || currentRoute.busStops.Length == 0) return false;
		return currentRoute.busStops[currentRoute.busStops.Length - 1] == stop;
	}

	public void ResetMission()
	{
		missionActive = false;
		missionComplete = false;
		missionFailed = false;
		currentStopIndex = 0;

		if (UIManager.instance != null)
		{
			UIManager.instance.isFinalStop = false;
		}

		foreach(var stop in currentRoute.busStops)
		{
			stop.ResetStop();
		}
	}
}