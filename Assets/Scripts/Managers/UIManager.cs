using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public GameObject drivingDashboard;
	public GameObject pauseUI;

	public GameObject gateOpenButton;
	public GameObject gateCloseButton;
	public GameObject movementControlsPanel;

    public GameObject lightsOnButton;
    public GameObject lightsOffButton;

    public GameObject engineOnButton;
    public GameObject engineOffButton;

    public DistanceHUD distanceHUD;
	public GameObject refuelButton;
	public TextMeshProUGUI text_info;

	[Header("Passenger HUD")]
	public GameObject passengerHUD;
	public Text statusText;
	public Text passengerCountText;

	[Header("Mission Summary")]
	public GameObject missionSummaryPanel;
	public GameObject missionFailedPanel;
	public Text routeName;
	public Text passengerServed;
	public Text income;

	public GameObject routeSelectionPanel;
	public TextMeshProUGUI text_Money;
	public TextMeshProUGUI text_SpeedInKmPerhour;
	public TextMeshProUGUI text_Liter;

    public bool gatesOpen;
	public bool isFinalStop = false;
	private bool insideStopTrigger;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	// Start is called before the first frame update
	void Start()
	{
		ExitStopZone();
		UpdateSteeringUI();
		LightsOnOff(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance != null)
		{
			if (GameManager.instance.state == GameState.Driving)
			{
				pauseUI.SetActive(false);
				drivingDashboard.SetActive(true);
			}
			else if (GameManager.instance.state == GameState.Pause)
			{
				pauseUI.SetActive(true);
				drivingDashboard.SetActive(false);
			}
		}
		text_Money.text = GameManager.instance.GetPlayerMoney().ToString();
		if (GameManager.instance.activeBusController != null)
		{
			text_SpeedInKmPerhour.text = GameManager.instance.activeBusController.GetSpeedInKmPerHour();
			text_Liter.text = GameManager.instance.activeBusController.currentFuel.ToString("F1");
        }
	}

	public void PauseButton()
	{
		GameManager.instance.PauseGame();
		SoundManager.instance.PlayUIButtonClicks();
	}

	public void Resumebutton()
	{
		GameManager.instance.ResumeGame();
		SoundManager.instance.PlayUIButtonClicks();
	}

	public void LoadSceneUsingName(string _name)
	{
		GameManager.instance.LoadGameScene(_name);
		SoundManager.instance.PlayUIButtonClicks();
	}

	public void EnterStopZone()
	{
		insideStopTrigger = true;

		if (!gatesOpen)
		{
			gateOpenButton.SetActive(true);
			gateCloseButton.SetActive(false);
		}
	}

	public void ExitStopZone()
	{
		insideStopTrigger = false;
		gatesOpen = false;

		gateOpenButton.SetActive(false);
		gateCloseButton.SetActive(false);

		movementControlsPanel.SetActive(true);
	}

	public void OpenGates()
	{
		if (!insideStopTrigger) return;

		gatesOpen = true;

		gateOpenButton.SetActive(false);
		gateCloseButton.SetActive(true);
		SoundManager.instance.PlayDoorSound();
		movementControlsPanel.SetActive(false);
		GameManager.instance.activeBusController.animator.SetBool("Open", true);
		PassengerSystem _ps = FindAnyObjectByType<PassengerSystem>();
		if (_ps != null)
		{
			_ps.StartBoardingProcess();
		}
	}

	public void CloseGates()
	{
		gatesOpen = false;

		gateOpenButton.SetActive(false);
		gateCloseButton.SetActive(false);
		SoundManager.instance.PlayDoorSound();
		movementControlsPanel.SetActive(true);
		GameManager.instance.activeBusController.animator.SetBool("Open", false);
	}

	public void ShowSummary(string name, int passenger, int missionEarning)
	{
		missionSummaryPanel.SetActive(true);
		routeName.text = $"Route : {name}";
		passengerServed.text = $"Passenger Served : {passenger}";
		income.text = $"Total Income : ${missionEarning}";
	}

	public void SelectRoute(int index)
	{
		GameManager.instance.DrirectionIndictor(true);
		MissionManager.instance.SelectRoute(index);
		SoundManager.instance.PlayUIButtonClicks();
	}

	public void ShowHUD()
	{
		passengerHUD.SetActive(true);
	}

	public void HideHUD()
	{
		passengerHUD.SetActive(false);
	}

	public void SetStatusText(string text)
	{
		statusText.text = "";
		statusText.text = text;
	}

	public void SetPassengerText(int current, int max)
	{
		passengerCountText.text = $"Passengers : {current}/{max}";
	}

	public void LightsOnOff(bool _value)
	{
		if (GameManager.instance.activeBusController)
		{
			SoundManager.instance.PlayDashboardClicks();
			if (_value)
			{
				lightsOffButton.SetActive(true);
				lightsOnButton.SetActive(false);
				GameManager.instance.activeBusController.headLights.SetActive(_value);
			}
			else
			{
				lightsOffButton.SetActive(false);
				lightsOnButton.SetActive(true);
				GameManager.instance.activeBusController.headLights.SetActive(_value);
			}
		}
	}

	public void EngineStartOff(bool _value)
	{
		if (GameManager.instance.activeBusController)
		{
			if (_value)
			{
                engineOffButton.SetActive(true);
                engineOnButton.SetActive(false);
                GameManager.instance.activeBusController.isEngineOn = _value;
			}
			else 
			{
                engineOffButton.SetActive(false);
                engineOnButton.SetActive(true);
                GameManager.instance.activeBusController.isEngineOn = _value;
			}
        }
	}

	private void UpdateSteeringUI()
	{
		if (GameManager.instance == null) return;

		GameObject steerButtonsObj = FindGameObjectInScene("SteerButtons");
		GameObject steeringWheelObj = FindGameObjectInScene("SteeringWheel");

		if (GameManager.instance.steeringMode == SteeringMode.Button)
		{
			if (steerButtonsObj != null) steerButtonsObj.SetActive(true);
			if (steeringWheelObj != null) steeringWheelObj.SetActive(false);
		}
		else if (GameManager.instance.steeringMode == SteeringMode.Wheel)
		{
			if (steerButtonsObj != null) steerButtonsObj.SetActive(false);
			if (steeringWheelObj != null) steeringWheelObj.SetActive(true);
		}
	}

	private GameObject FindGameObjectInScene(string name)
	{
		Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
		foreach (var t in allTransforms)
		{
			if (t.gameObject.name == name && t.gameObject.scene.isLoaded)
			{
				return t.gameObject;
			}
		}
		return null;
	}
}