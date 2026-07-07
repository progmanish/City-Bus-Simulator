using UnityEngine;

public class RouteData : MonoBehaviour
{
	public string routeName;
	public BusStop[] busStops;
	public Transform goalTrigger;
	public int FarePerStop = 10;
}