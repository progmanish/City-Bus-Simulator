using UnityEngine;

public class FuelSatation : MonoBehaviour
{
	public float refuelSpeed = 2f;
	private BusController bus;
	private int triggerCount = 0;

	void Update()
	{
		if (bus != null)
		{
			if (bus.isEngineOn)
			{
				if (UIManager.instance != null)
				{
					if (UIManager.instance.text_info != null)
					{
						UIManager.instance.text_info.gameObject.SetActive(true);
						UIManager.instance.text_info.text = "Please Turn Off Engine To Refuel";
					}
					if (UIManager.instance.refuelButton != null)
					{
						UIManager.instance.refuelButton.SetActive(false);
					}
				}

				if (GameManager.instance != null && GameManager.instance.GetFuelInput())
				{
					GameManager.instance.ReadFuelInput(false);
					if (SoundManager.instance != null)
					{
						SoundManager.instance.PlayRefuelingSound(false);
					}
				}
			}
			else
			{
				if (UIManager.instance != null)
				{
					if (UIManager.instance.text_info != null)
					{
						UIManager.instance.text_info.gameObject.SetActive(false);
					}
					if (UIManager.instance.refuelButton != null)
					{
						UIManager.instance.refuelButton.SetActive(bus.currentFuel < bus.maxFuel);
					}
				}

				if (GameManager.instance != null && GameManager.instance.GetFuelInput())
				{
					if (bus.currentFuel >= bus.maxFuel)
					{
						GameManager.instance.ReadFuelInput(false);
						if (SoundManager.instance != null)
						{
							SoundManager.instance.PlayRefuelingSound(false);
						}
						if (UIManager.instance != null && UIManager.instance.refuelButton != null)
						{
							UIManager.instance.refuelButton.SetActive(false);
						}
					}
					else
					{
						bus.currentFuel += refuelSpeed * Time.deltaTime;
					}
				}
			}
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		BusController foundBus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
		if (foundBus != null)
		{
			bus = foundBus;
			triggerCount++;
		}
    }

    private void OnTriggerExit(Collider other)
    {
		BusController foundBus = other.GetComponent<BusController>() ?? other.GetComponentInParent<BusController>();
		if (foundBus != null)
		{
			triggerCount--;
			if (triggerCount <= 0)
			{
				bus = null;
				triggerCount = 0;

				if (UIManager.instance != null)
				{
					if (UIManager.instance.refuelButton != null)
					{
						UIManager.instance.refuelButton.SetActive(false);
					}
					if (UIManager.instance.text_info != null)
					{
						UIManager.instance.text_info.gameObject.SetActive(false);
					}
				}

				if (GameManager.instance != null)
				{
					GameManager.instance.ReadFuelInput(false);
				}

				if (SoundManager.instance != null)
				{
					SoundManager.instance.PlayRefuelingSound(false);
				}
			}
		}
    }
}