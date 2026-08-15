using System.Collections;
using UnityEngine;

public class PassengerSystem : MonoBehaviour
{
	public int maxCapacity = 40;
	public int currentPassengers;

	public int minBoard = 1;
	public int maxBoard = 8;

	[Range(0f, 1f)]
	public float maxDeboardPercent = 0.4f;

	public float boardInterval = 1f;
	public float deboardInterval = 1f;

	public bool boardingFinished { get; set; }
	private int toBoard;
	private int toDeboard;

	// Observer Events
	public static event System.Action<PassengerSystem> OnBoardingStarted;
	public static event System.Action<string> OnStatusTextChanged;
	public static event System.Action<int, int> OnPassengerCountChanged;
	public static event System.Action OnSpawnDeboardNPC;
	public static event System.Action OnSpawnBoardNPC;
	public static event System.Action<int> OnBoardPassenger;
	public static event System.Action<int> OnAddIncome;
	public static event System.Action<string, int, int> OnShowSummaryRequested;
	public static event System.Action<int> OnAddMoneyRequested;

	public void StartBoardingProcess()
	{
		boardingFinished = false;

		OnBoardingStarted?.Invoke(this);

		bool isFinal = (UIManager.instance != null && UIManager.instance.isFinalStop);
		toDeboard = isFinal ? currentPassengers : Mathf.RoundToInt(currentPassengers * Random.Range(0f, maxDeboardPercent));
		toBoard = Random.Range(minBoard, maxBoard + 1);
		toBoard = Mathf.Min(toBoard, maxCapacity - currentPassengers + toDeboard);

		if (isFinal) toBoard = 0;

		if (toDeboard > 0)
		{
			OnStatusTextChanged?.Invoke("Deboarding Passengers...");
		}
		else
		{
			OnStatusTextChanged?.Invoke("Boarding Passengers...");
		}

		OnPassengerCountChanged?.Invoke(currentPassengers, maxCapacity);
		StartCoroutine(DeboardRoutine());
	}

	IEnumerator DeboardRoutine()
	{
		for (int i = 0; i < toDeboard; i++)
		{
			currentPassengers--;
			OnSpawnDeboardNPC?.Invoke();
			OnPassengerCountChanged?.Invoke(currentPassengers, maxCapacity);
            yield return new WaitForSeconds(deboardInterval);
		}
		StartCoroutine(BoardRoutine());
	}

	IEnumerator BoardRoutine()
	{
		if (toBoard > 0)
		{
			OnStatusTextChanged?.Invoke("Boarding Passengers...");
		}

		for (int i = 0; i < toBoard; i++)
		{
			currentPassengers++;
			OnSpawnBoardNPC?.Invoke();
			OnPassengerCountChanged?.Invoke(currentPassengers, maxCapacity);
			OnBoardPassenger?.Invoke(1);
            yield return new WaitForSeconds(boardInterval);
        }

		boardingFinished = true;
		OnAddIncome?.Invoke(currentPassengers);
		OnStatusTextChanged?.Invoke("Boarding Complete. Close the doors...");

		if (UIManager.instance != null && UIManager.instance.isFinalStop)
		{
			if (MissionManager.instance != null)
			{
				OnShowSummaryRequested?.Invoke(
					MissionManager.instance.MissionName(), MissionManager.instance.Passengers(), MissionManager.instance.Income());
				OnAddMoneyRequested?.Invoke(MissionManager.instance.Income());
			}
		}
    }

    public void ResetBoardingState()
	{
		boardingFinished = false;
		OnStatusTextChanged?.Invoke("Boarding Complete. Close the doors...");
	}
}