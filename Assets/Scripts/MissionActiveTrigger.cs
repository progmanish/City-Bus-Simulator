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
        UIManager.instance.routeSelectionPanel.SetActive(true);

        Debug.Log("<color=cyan>Mission Active!</color>");
    }

    private void OnTriggerExit(Collider other)
    {
        BusController bus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
        if (bus == null) return;

        UIManager.instance.routeSelectionPanel.SetActive(false);
    }
}