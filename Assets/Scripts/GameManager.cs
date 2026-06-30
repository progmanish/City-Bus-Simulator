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
	}

	// Update is called once per frame
	void Update()
	{

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
}