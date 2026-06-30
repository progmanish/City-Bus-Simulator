using UnityEngine;

public class MissionActiveTrigger : MonoBehaviour
{
	private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        BusController bus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
        if (bus == null) return;

        if (MissionManager.instance == null) return;

        used = true;
        MissionManager.instance.StartMission();

        Debug.Log("<color=cyan>Mission Active!</color>");
    }
}