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
        }

        used = true;
        MissionManager.instance.CompleteMission();

        Debug.Log("<color=cyan>Mission Completed!</color>");
    }
}