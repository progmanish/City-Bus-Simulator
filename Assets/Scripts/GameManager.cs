using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Core reference")]
	public BusController activeBusController;

	public static GameManager instance;
	public GameState state = GameState.Driving;
	public SteeringMode steeringMode = SteeringMode.Button;
	public Gear gear = Gear.Driving;

	public int playerMoney;
	const string moneyID = "Player_Money";
	const string selectedBus = "Selected_Bus";

    private bool fuelInput = false;

    // Start is called before the first frame update
    void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		LoadData();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LoadData()
	{
		playerMoney = PlayerPrefs.GetInt(moneyID, 20000);
		SaveMoney(playerMoney);
	}

	public void AddMoney(int amount)
	{
		playerMoney += amount;
		SaveMoney(playerMoney);
	}

	public void SaveMoney(int amount)
	{
		PlayerPrefs.SetInt(moneyID, amount);
	}

    public bool SpendMoney(int amount)
    {
		if (playerMoney < amount)
		{
			//	popup message
			return false;
		}
        playerMoney -= amount;
        SaveMoney(playerMoney);
		return true;
    }

	public int GetPlayerMoney()
	{
		return PlayerPrefs.GetInt(moneyID, 2000);
	}

	public void SetSelectedBus(string busID)
	{
		PlayerPrefs.SetString(selectedBus, busID);
	}

	public string GetSelectedBus()
	{
		return PlayerPrefs.GetString(selectedBus, "");
	}

	public void UnlockedBus(string busID)
	{
		PlayerPrefs.SetInt(busID, 1);
	}

	public bool IsBusUnlocked(string busID)
	{
		return PlayerPrefs.GetInt(busID, 0) == 1;
	}

    public void SetSteering(float _input)
	{
		if (activeBusController != null)
		{
			activeBusController.SteeringInput(_input);
		}
	}

	public void SetThrottle(bool _value)
	{
		if (activeBusController != null)
		{
			activeBusController.ThrotleInput(_value);
		}
	}

	public void SetBreaks(bool _value)
	{
		if (activeBusController != null)
		{
			activeBusController.BrakeInput(_value);
		}
	}

	public void SetGear(float _value)
	{
		if(_value == 1)
		{
			gear = Gear.Driving;
		}
		else
		{
			gear = Gear.Reverse;
		}
	}

	public void OnGameplaySceneLoaded(BusController busController)
	{
		state = GameState.Driving;
		activeBusController = busController;
	}

	public void PauseGame()
	{
		state = GameState.Pause;
		Time.timeScale = 0f;
	}

	public void ResumeGame()
	{
		state = GameState.Driving;
		Time.timeScale = 1f;
	}

	public void LoadGameScene(string name)
	{
		SceneManager.LoadScene(name);
	}

    public void ReadFuelInput(bool input)
    {
        fuelInput = input;
    }

    public bool GetFuelInput() => fuelInput;
}