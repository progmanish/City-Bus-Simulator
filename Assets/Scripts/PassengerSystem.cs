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

	public float boardInterval = 0.6f;
	public float deboardInterval = 0.6f;

	public bool boardingFinished { get; set; }
	private int toBoard;
	private int toDeboard;

	public void StartBoardingProcess()
	{
		boardingFinished = false;

		UIManager.instance.ShowHUD();
		UIManager.instance.SetStatusText("Boarding Passengers...");
		UIManager.instance.SetPassengerText(currentPassengers, maxCapacity);

		toDeboard = UIManager.instance.isFinalStop ? currentPassengers : Mathf.RoundToInt(currentPassengers * Random.Range(0f, maxDeboardPercent));
		toBoard = Random.Range(minBoard, maxBoard + 1);
		toBoard = Mathf.Min(toBoard, maxCapacity - currentPassengers + toDeboard);

		if (UIManager.instance.isFinalStop) toBoard = 0;
		StartCoroutine(DeboardRoutine());
	}

	IEnumerator DeboardRoutine()
	{
		for (int i = 0; i < toDeboard; i++)
		{
			currentPassengers--;
			if(NPCManager.instance != null)
			{
				NPCManager.instance.SpawnDeboardNPC();
			}
			UIManager.instance.SetPassengerText(currentPassengers, maxCapacity);
            yield return new WaitForSeconds(deboardInterval);
		}
		StartCoroutine(BoardRoutine());

	}

	IEnumerator BoardRoutine()
	{
		for (int i = 0; i < toBoard; i++)
		{
			currentPassengers++;
            if (NPCManager.instance != null)
            {
                NPCManager.instance.SpawnBoardNPC();
            }
            UIManager.instance.SetPassengerText(currentPassengers, maxCapacity);
			MissionManager.instance.BoardPassenger(1);
            yield return new WaitForSeconds(boardInterval);
        }

		boardingFinished = true;
		MissionManager.instance.AddIncone(currentPassengers);
        UIManager.instance.SetStatusText("Boarding Complete. Close the doors...");

		if (UIManager.instance.isFinalStop)
		{
			UIManager.instance.ShowSummary(
				MissionManager.instance.MissionName(), MissionManager.instance.Passengers(), MissionManager.instance.Income());
			GameManager.instance.AddMoney(MissionManager.instance.Income());
		}
    }

    public void ResetBoardingState()
	{
		boardingFinished = false;
        UIManager.instance.SetStatusText("Boarding Complete. Close the doors...");

    }
}