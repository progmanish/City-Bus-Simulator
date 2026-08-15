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

	private int cachedMoney = -1;
	private int cachedSpeed = -1;
	private float cachedFuel = -1f;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		insideStopTrigger = false;
		gatesOpen = false;
		HideHUD();
		ExitStopZone();
		UpdateSteeringUI();
		LightsOnOff(false);
	}

	private void OnEnable()
	{
		PassengerSystem.OnBoardingStarted += HandleBoardingStarted;
		PassengerSystem.OnStatusTextChanged += SetStatusText;
		PassengerSystem.OnPassengerCountChanged += SetPassengerText;
		PassengerSystem.OnShowSummaryRequested += ShowSummary;
	}

	private void OnDisable()
	{
		PassengerSystem.OnBoardingStarted -= HandleBoardingStarted;
		PassengerSystem.OnStatusTextChanged -= SetStatusText;
		PassengerSystem.OnPassengerCountChanged -= SetPassengerText;
		PassengerSystem.OnShowSummaryRequested -= ShowSummary;
	}

	private void HandleBoardingStarted(PassengerSystem ps)
	{
		ShowHUD();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance != null)
		{
			if (GameManager.instance.state == GameState.Driving)
			{
				if (pauseUI != null && pauseUI.activeSelf) pauseUI.SetActive(false);
				if (drivingDashboard != null && !drivingDashboard.activeSelf) drivingDashboard.SetActive(true);
			}
			else if (GameManager.instance.state == GameState.Pause)
			{
				if (pauseUI != null && !pauseUI.activeSelf) pauseUI.SetActive(true);
				if (drivingDashboard != null && drivingDashboard.activeSelf) drivingDashboard.SetActive(false);
			}

			int currentMoney = GameManager.instance.GetPlayerMoney();
			if (currentMoney != cachedMoney)
			{
				cachedMoney = currentMoney;
				if (text_Money != null) text_Money.text = currentMoney.ToString();
			}

			if (GameManager.instance.activeBusController != null)
			{
				float speedKmh = GameManager.instance.activeBusController.GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f;
				int speedInt = Mathf.RoundToInt(speedKmh);
				if (speedInt != cachedSpeed)
				{
					cachedSpeed = speedInt;
					if (text_SpeedInKmPerhour != null) text_SpeedInKmPerhour.text = speedInt.ToString();
				}

				float currentFuel = GameManager.instance.activeBusController.currentFuel;
				if (Mathf.Abs(currentFuel - cachedFuel) > 0.05f)
				{
					cachedFuel = currentFuel;
					if (text_Liter != null) text_Liter.text = currentFuel.ToString("F1");
				}
			}
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