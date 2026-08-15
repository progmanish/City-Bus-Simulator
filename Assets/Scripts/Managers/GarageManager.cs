using TMPro;
using UnityEngine;

public class GarageManager : MonoBehaviour
{
	public BusDatabase busDatabase;
	public Transform previewSpawnPoint;

	public GameObject purchaseButton;
	public GameObject selecteButton;
	public GameObject lockIcon;
	public GameObject selectIcon;

	public GameObject info;
	public GameObject board;

    [Header("UI References")]
	public TextMeshProUGUI text_BusName;
	public TextMeshProUGUI text_capacity;
	public TextMeshProUGUI text_Fuel;
	public TextMeshProUGUI text_Efficiency;
	public TextMeshProUGUI text_Price;
	public TextMeshProUGUI text_Money;

    int currentIndex = 0;
	GameObject currentPreview;
	private int cachedMoney = -1;

    private void Start()
    {
		// Unlock the first bus by default
		if (GameManager.instance != null && busDatabase != null && busDatabase.buses != null && busDatabase.buses.Length > 0)
		{
			GameManager.instance.UnlockedBus(busDatabase.buses[0].busID);

			// Load the previously selected bus index and prefab
			string savedBusID = GameManager.instance.GetSelectedBus();
			if (!string.IsNullOrEmpty(savedBusID))
			{
				for (int i = 0; i < busDatabase.buses.Length; i++)
				{
					if (busDatabase.buses[i] != null && busDatabase.buses[i].busID == savedBusID)
					{
						currentIndex = i;
						if (BusManager.instance != null)
						{
							BusManager.instance.SelectBus(busDatabase.buses[i].busPrefabWorking);
						}
						break;
					}
				}
			}
			else
			{
				// If no bus was selected, select the first one by default
				currentIndex = 0;
				GameManager.instance.SetSelectedBus(busDatabase.buses[0].busID);
				if (BusManager.instance != null)
				{
					BusManager.instance.SelectBus(busDatabase.buses[0].busPrefabWorking);
				}
			}
		}

		ShowBus(currentIndex);
		if (busDatabase != null && busDatabase.buses != null && currentIndex >= 0 && currentIndex < busDatabase.buses.Length)
		{
			UpdatePurchaseState(busDatabase.buses[currentIndex]);
		}
    }

    private void Update()
    {
		int currentMoney = GameManager.instance.GetPlayerMoney();
		if (currentMoney != cachedMoney)
		{
			cachedMoney = currentMoney;
			if (text_Money != null) text_Money.text = currentMoney.ToString();
		}
    }

    public void NextBus()
	{
		currentIndex++;
		if (currentIndex >= busDatabase.buses.Length) currentIndex = 0;
		SoundManager.instance.PlayUIButtonClicks();
		ShowBus(currentIndex);
		UpdatePurchaseState(busDatabase.buses[currentIndex]);
	}

	public void PreviewBus()
	{
        currentIndex--;
        if (currentIndex < 0) currentIndex = busDatabase.buses.Length - 1;
        SoundManager.instance.PlayUIButtonClicks();
        ShowBus(currentIndex);
        UpdatePurchaseState(busDatabase.buses[currentIndex]);
    }

    void ShowBus(int index)
	{
		if (currentPreview != null)
			Destroy(currentPreview);

		var busInfo = busDatabase.buses[index];
		currentPreview = Instantiate(
			busInfo.busPrefab,
			previewSpawnPoint.position,
			previewSpawnPoint.rotation
		);
		UpdateUI(busInfo);
	}

	void UpdateUI(BusData info)
	{
		text_BusName.text = info.displayName;
		text_capacity.text = "Capacity : " + info.passengerCapacity;
		text_Fuel.text = "Fuel : " + info.fuelCapacity;
		text_Efficiency.text = "Fuel Efficieny : " + info.fuelEfficiency + "%";
		text_Price.text = "Price : $" + info.price;
	}

	public void SelectCurrentBus()
	{
		if (busDatabase == null || busDatabase.buses == null || currentIndex < 0 || currentIndex >= busDatabase.buses.Length)
		{
			Debug.LogWarning("Bus Database is null or index out of range!");
			return;
		}

		BusData bus = busDatabase.buses[currentIndex];
		bool purchased = SaveService.IsBusUnlocked(bus.busID);
        SoundManager.instance.PlayUIButtonClicks();

        if (!purchased) return;

		BusManager.instance.SelectBus(bus.busPrefabWorking);
		GameManager.instance.SetSelectedBus(bus.busID);
		UpdatePurchaseState(bus);
	}

	public void StartGame(string SceneName)
	{
		if (BusManager.instance == null || BusManager.instance.selectedBusPrefab == null)
		{
			Debug.LogWarning("No Bus Selected !!");
			return;
		}
		BusManager.instance.LoadGameScene(SceneName);
        SoundManager.instance.PlayUIButtonClicks();
	}

	public void OpenOptionsMenu(string SceneName)
	{
        BusManager.instance.LoadGameScene(SceneName);
        SoundManager.instance.PlayUIButtonClicks();
    }

	public void PurchaseBus()
	{
		BusData bus = busDatabase.buses[currentIndex];
		if (GameManager.instance.SpendMoney(bus.price))
		{
			SoundManager.instance.PlayUIButtonClicks();
			GameManager.instance.UnlockedBus(bus.busID);
			UpdatePurchaseState(bus);
		}
		else
		{
			// popup message
			//Debug.Log("Not enough money!!");
		}
	}
	
	void UpdatePurchaseState(BusData bus)
	{
		bool purchase = SaveService.IsBusUnlocked(bus.busID);
		lockIcon.SetActive(!purchase);

		purchaseButton.gameObject.SetActive(!purchase);
		selecteButton.SetActive(purchase);
		selectIcon.SetActive(GameManager.instance.GetSelectedBus() == bus.busID);

		if (info != null)
		{
			info.SetActive(!purchase);
		}
		if (board != null)
		{
			board.SetActive(!purchase);
		}
	}

}