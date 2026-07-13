using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("Core reference")]
	public BusController activeBusController;
	public LoadingScene loadingScene;

	public GameState state = GameState.Driving;
	public SteeringMode steeringMode = SteeringMode.Button;
	public Gear gear = Gear.Driving;

	public int playerMoney;
	const string moneyID = "Player_Money";
	const string selectedBus = "Selected_Bus";
	const string selectedSteeringMode = "Steering_Mode";

    private bool fuelInput = false;

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

	public void LoadData()
	{
		playerMoney = PlayerPrefs.GetInt(moneyID, 1000);
		SaveMoney(playerMoney);

		int savedMode = PlayerPrefs.GetInt(selectedSteeringMode, (int)SteeringMode.Button);
		steeringMode = (SteeringMode)savedMode;
	}

	public string GetSelectedSteeringMode() => selectedSteeringMode;

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
        Time.timeScale = 1f;
    }

	public void SetLoadingSceneOnGameplay(LoadingScene _loadingScene)
	{
		loadingScene = _loadingScene;
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

    public void LoadGameScene(string _name) => loadingScene.LoadScene(_name);
    public void ReadFuelInput(bool input) => fuelInput = input;
    public bool GetFuelInput() => fuelInput;
	public void DrirectionIndictor(bool _value) => activeBusController.OnOffDirectionIndictor(_value);
}