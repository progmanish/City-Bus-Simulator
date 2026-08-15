using UnityEngine;

[CreateAssetMenu(fileName = "NewBusConfig", menuName = "Bus Simulator/Bus Configuration")]
public class BusConfig : ScriptableObject
{
    [Header("Identities")]
    public string busID;
    public string displayName;

    [Header("Economic & Capacity Parameters")]
    public int passengerCapacity = 30;
    public float fuelCapacity = 100f;
    public float fuelEfficiency = 100f; // percentage
    public int price = 5000;

    [Header("Physics Tuning Parameters")]
    public float busTorque = 1500f;
    public float brakeForce = 4000f;
    public float maxSpeed = 40f;

    [Header("Fuel Consumption Settings")]
    public float idleConsumption = 0.02f;
    public float runningConsumption = 0.06f;
}
