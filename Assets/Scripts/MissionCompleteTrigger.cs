using UnityEngine;

public class MissionCompleteTrigger : MonoBehaviour
{
    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        BusController bus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
        if (bus == null) return;

        if (MissionManager.instance == null) return;
        if (!MissionManager.instance.missionActive) return;

        if (!MissionManager.instance.AllStopsComplated())
        {
            Debug.Log("<color=red>Mission failed: Skipped stops!!</color>");
            MissionManager.instance.FailMission();
            return;
        }

        used = true;
        bus.OnOffDirectionIndictor(false);
        MissionManager.instance.CompleteMission();

        if (UIManager.instance != null)
        {
            UIManager.instance.EnterStopZone();
            UIManager.instance.ShowHUD();
            UIManager.instance.SetStatusText("Arrived at final stop. Open doors to deboard...");
            PassengerSystem ps = bus.GetComponent<PassengerSystem>() ?? bus.GetComponentInParent<PassengerSystem>();
            if (ps != null)
            {
                UIManager.instance.SetPassengerText(ps.currentPassengers, ps.maxCapacity);
            }
        }

        Debug.Log("<color=cyan>Mission Completed!</color>");
    }
}