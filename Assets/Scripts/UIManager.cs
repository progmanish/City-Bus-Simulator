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

	public DistanceHUD distanceHUD;

	public GameObject passengerHUD;
	public Text statusText;
	public Text passengerCountText;

	public bool gatesOpen;
	public bool isFinalStop = false;
	public bool insideStopTrigger;

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
	}

    public void PauseButton() => GameManager.instance.PauseGame();
    public void Resumebutton() => GameManager.instance.ResumeGame();

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

		movementControlsPanel.SetActive(true);
        GameManager.instance.activeBusController.animator.SetBool("Open", false);

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
}