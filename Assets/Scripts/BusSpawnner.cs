using UnityEngine;

public class BusSpawnner : MonoBehaviour
{
	public Transform spawnPoint;

    private void Start()
    {
        if (BusManager.instance == null || BusManager.instance.selectedBusPrefab == null)
        {
            Debug.LogError("No bus Selected !");
            return;
        }

        Instantiate(BusManager.instance.selectedBusPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}