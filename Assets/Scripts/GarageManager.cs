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

    [Header("UI References")]
	public TextMeshProUGUI text_BusName;
	public TextMeshProUGUI text_capacity;
	public TextMeshProUGUI text_Fuel;
	public TextMeshProUGUI text_Economy;
	public TextMeshProUGUI text_Price;
	public TextMeshProUGUI text_Money;

    int currentIndex = 0;
	GameObject currentPreview;

    private void Start()
    {
		ShowBus(currentIndex);
		GameManager.instance.UnlockedBus(busDatabase.buses[0].busID);
		UpdatePurchaseState(busDatabase.buses[currentIndex]);
    }

    private void Update()
    {
		text_Money.text = GameManager.instance.GetPlayerMoney().ToString();
    }

    public void NextBus()
	{
		currentIndex++;
		if (currentIndex >= busDatabase.buses.Length) currentIndex = 0;

		ShowBus(currentIndex);
		UpdatePurchaseState(busDatabase.buses[currentIndex]);
	}

	public void PreviewBus()
	{
        currentIndex--;
        if (currentIndex < 0) currentIndex = busDatabase.buses.Length - 1;

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
		text_Economy.text = "Economy : " + info.fuelEfficiency;
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
		bool purchased = PlayerPrefs.GetInt(bus.busID, 0) == 1;

		if (!purchased) return;

		BusManager.instance.SelectBus(bus.busPrefabWorking);
		GameManager.instance.SetSelectedBus(bus.busID);
		UpdatePurchaseState(bus);
	}

	public void StartGame(string gameplaySceneName)
	{
		if (BusManager.instance == null || BusManager.instance.selectedBusPrefab == null)
		{
			Debug.LogWarning("No Bus Selected !!");
			return;
		}
		BusManager.instance.LoadGameplay(gameplaySceneName);
	}

	public void PurchaseBus()
	{
		BusData bus = busDatabase.buses[currentIndex];
		int playerMoney = PlayerPrefs.GetInt("Player_Money", 0);

		if (playerMoney < bus.price)
		{
			//	popup message
			Debug.Log("Not enough money!!");
			return;
		}
		playerMoney -= bus.price;

		PlayerPrefs.SetInt("Player_Money", playerMoney);
		GameManager.instance.UnlockedBus(bus.busID);
		UpdatePurchaseState(bus);
	}
	
	void UpdatePurchaseState(BusData bus)
	{
		bool purchase = PlayerPrefs.GetInt(bus.busID, 0) == 1;
		lockIcon.SetActive(!purchase);

		purchaseButton.gameObject.SetActive(!purchase);
		selecteButton.SetActive(purchase);
		selectIcon.SetActive(GameManager.instance.GetSelectedBus() == bus.busID);
	}

}