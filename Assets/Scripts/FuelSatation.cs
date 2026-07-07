using UnityEngine;

public class FuelSatation : MonoBehaviour
{
	public float refuelSpeed = 2f;
	private BusController bus;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (bus == null)
		{
			UIManager.instance.refuelButton.SetActive(false);
		}
		else
		{
			UIManager.instance.refuelButton.SetActive(true);
		}

		if (bus != null && GameManager.instance.GetFuelInput())
		{
			bus.currentFuel += refuelSpeed * Time.deltaTime;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		bus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>())
		{
			bus = null;
		}
    }
}