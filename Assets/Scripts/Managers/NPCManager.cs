using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NPCManager : MonoBehaviour
{
	public static NPCManager instance;

	[Header("Prefabs")]
	public GameObject outsideNPCPrefab;
	public GameObject insideNPCPrefab;

	[Header("Spawn Points")]
	public Transform outsideSpawnPoint;
	public Transform insideSpawnPonit;
	public Transform sidewalkPoint;

	[Header("Seated NPC Slots")]
	public List<GameObject> seatedNPCs = new List<GameObject>();

    private void Awake()
    {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
    }

    private void OnEnable()
    {
        PassengerSystem.OnSpawnDeboardNPC += SpawnDeboardNPC;
        PassengerSystem.OnSpawnBoardNPC += SpawnBoardNPC;
    }

    private void OnDisable()
    {
        PassengerSystem.OnSpawnDeboardNPC -= SpawnDeboardNPC;
        PassengerSystem.OnSpawnBoardNPC -= SpawnBoardNPC;
    }

    void Start()
    {
        PassengerSystem ps = FindAnyObjectByType<PassengerSystem>();
        int initialPassengers = ps != null ? ps.currentPassengers : 0;

        for (int i = 0; i < seatedNPCs.Count; i++)
        {
            if (seatedNPCs[i] != null)
            {
                seatedNPCs[i].SetActive(i < initialPassengers);
            }
        }
    }

    public void SpawnBoardNPC()
	{
		var _npc = Instantiate(outsideNPCPrefab, outsideSpawnPoint.position, outsideSpawnPoint.rotation);
		StartCoroutine(MoveNPC(_npc.transform, insideSpawnPonit.position, true));
	}	
    public void SpawnDeboardNPC()
    {
        var _npc = Instantiate(insideNPCPrefab, insideSpawnPonit.position, insideSpawnPonit.rotation);
        StartCoroutine(MoveNPC(_npc.transform, sidewalkPoint.position, false));
    }

    IEnumerator MoveNPC(Transform npc, Vector3 target, bool boarding)
	{
		float speed = 1.5f;

		while(npc != null && Vector3.Distance(npc.position, target) > 0.05f)
		{
			npc.position = Vector3.MoveTowards(npc.position, target, speed * Time.deltaTime);
			yield return null;
		}

		if (npc == null) yield break;

		if (boarding)
			EnableSeatedNPC();
		else
			DisableSeatedNPC();

		Destroy(npc.gameObject);
	}

	void EnableSeatedNPC()
	{
		foreach (var npc in seatedNPCs)
		{
			if (!npc.activeSelf)
			{
				npc.SetActive(true);
				break;
			}
		}
	}

	void DisableSeatedNPC()
	{
		for (int i = seatedNPCs.Count - 1; i >= 0;  i--)
		{
			if (seatedNPCs[i].activeSelf)
			{
				seatedNPCs[i].SetActive(false);
				break;
			}
		}
	}
}