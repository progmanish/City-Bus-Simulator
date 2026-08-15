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
	const string selectedSteeringMode = "Steering_Mode";

    private bool fuelInput = false;

    void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			Application.targetFrameRate = 60;

#if UNITY_ANDROID
			// 1. Mobile Resolution Clamping (Huge GPU Fill-rate & Thermal Saver)
			int targetWidth = 1280;
			if (Screen.currentResolution.width > targetWidth)
			{
				float ratio = (float)Screen.currentResolution.height / Screen.currentResolution.width;
				int newHeight = Mathf.RoundToInt(targetWidth * ratio);
				Screen.SetResolution(targetWidth, newHeight, true);
			}

			// 2. Hardware Level Graphics Optimizations
			QualitySettings.shadowDistance = 15f; 
			QualitySettings.shadows = ShadowQuality.HardOnly; 
			QualitySettings.antiAliasing = 0;
			QualitySettings.vSyncCount = 0;

			// 3. Physics Timestep Optimization (Saves CPU cycles)
			Time.fixedDeltaTime = 0.0333f;
#endif
		}
		else
		{
			Destroy(gameObject);
		}
		LoadData();
	}

	private void OnEnable()
	{
		PassengerSystem.OnAddMoneyRequested += AddMoney;
	}

	private void OnDisable()
	{
		PassengerSystem.OnAddMoneyRequested -= AddMoney;
	}

	public void LoadData()
	{
		playerMoney = SaveService.GetMoney(50000);
		steeringMode = (SteeringMode)SaveService.GetSteeringMode((int)SteeringMode.Button);
	}

	public string GetSelectedSteeringMode() => selectedSteeringMode;

	public void AddMoney(int amount)
	{
		playerMoney += amount;
		SaveMoney(playerMoney);
	}

	public void SaveMoney(int amount)
	{
		SaveService.SetMoney(amount);
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
		return playerMoney;
	}

	public void SetSelectedBus(string busID)
	{
		SaveService.SetSelectedBus(busID);
	}

	public string GetSelectedBus()
	{
		return SaveService.GetSelectedBus("");
	}

	public void UnlockedBus(string busID)
	{
		SaveService.UnlockBus(busID);
	}

	public bool IsBusUnlocked(string busID)
	{
		return SaveService.IsBusUnlocked(busID);
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