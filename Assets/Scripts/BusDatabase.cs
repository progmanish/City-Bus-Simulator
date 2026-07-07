using System;
using UnityEngine;

[Serializable]
public class BusData
{
	public string busID;
	public string displayName;
	public GameObject busPrefab;
	public GameObject busPrefabWorking;

    [Header("UI Display only")]
	public int passengerCapacity;
	public float fuelCapacity;
	public float fuelEfficiency;
	public int price;
}

public class BusDatabase : MonoBehaviour
{
	public BusData[] buses;
}