using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelSound;
    public GameObject panelControls;
    public GameObject panelAbout;
    public GameObject panelReset;

    [Header("Sound")]
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject sfxOn;
    public GameObject sfxOff;

    [Header("Controls")]
    public GameObject selectedLogo;

    private void Start()
    {
        panelSound.SetActive(true);
        panelControls.SetActive(false);
        panelAbout.SetActive(false);
        panelReset.SetActive(false);

        RefreshLogoPosition();
    }

    public void ClickSound()
    {
        panelSound.SetActive(true);
        panelControls.SetActive(false);
        panelAbout.SetActive(false);
        panelReset.SetActive(false);
    }

    public void ClickControls()
    {
        panelSound.SetActive(false);
        panelControls.SetActive(true);
        panelAbout.SetActive(false);
        panelReset.SetActive(false);

        RefreshLogoPosition();
    }

    public void ClickAbout()
    {
        panelSound.SetActive(false);
        panelControls.SetActive(false);
        panelAbout.SetActive(true);
        panelReset.SetActive(false);
    }

    public void ClickReset()
    {
        panelSound.SetActive(false);
        panelControls.SetActive(false);
        panelAbout.SetActive(false);
        panelReset.SetActive(true);
    }

    public void OpenScene(string SceneName)
    {
        BusManager.instance.LoadGameScene(SceneName);
        SoundManager.instance.PlayUIButtonClicks();
    }

    public void MusicOnOff(bool _value)
    {
        if (_value)
        {
            musicOff.SetActive(true);
            musicOn.SetActive(false);
            Debug.Log("music onn");
        }
        else
        {
            musicOff.SetActive(false);
            musicOn.SetActive(true);
            Debug.Log("music offf");

        }
    }

    public void SfxOnOff(bool _value)
    {
        if (_value)
        {
            sfxOff.SetActive(true);
            sfxOn.SetActive(false);
            Debug.Log("sfx onn");
        }
        else
        {
            sfxOff.SetActive(false);
            sfxOn.SetActive(true);
            Debug.Log("sfx offf");

        }
    }

    public void ChangeSteeringMode(int _value)
    {
        SteeringMode newMode = (_value == ((int)SteeringMode.Button)) ? SteeringMode.Button : SteeringMode.Wheel;

        if (GameManager.instance != null)
        {
            GameManager.instance.steeringMode = newMode;
        }

        PlayerPrefs.SetInt(GameManager.instance.GetSelectedSteeringMode(), (int)newMode);
        PlayerPrefs.Save();

        if (selectedLogo != null)
        {
            RectTransform rect = selectedLogo.GetComponent<RectTransform>();
            Vector3 targetPos = (newMode == SteeringMode.Button) ? new Vector3(-215f, 0, 0) : new Vector3(215f, 0, 0);
            if (rect != null)
            {
                rect.anchoredPosition = new Vector2(targetPos.x, targetPos.y);
            }
            else
            {
                selectedLogo.transform.localPosition = targetPos;
            }
        }
    }

    public void ResetGameData(bool _value)
    {
        if (_value)
        {
            PlayerPrefs.DeleteAll();
            if (GameManager.instance != null)
            {
                GameManager.instance.LoadData();
            }
            if (BusManager.instance != null)
            {
                BusManager.instance.SelectBus(null);
            }
            PlayerPrefs.Save();
            Debug.Log("Data Reset Success");
        }
        OpenScene("SelectionMenu");
    }

    private void RefreshLogoPosition()
    {
        int savedMode = PlayerPrefs.GetInt(GameManager.instance.GetSelectedSteeringMode(), (int)SteeringMode.Button);
        if (selectedLogo != null)
        {
            RectTransform rect = selectedLogo.GetComponent<RectTransform>();
            Vector3 targetPos = (savedMode == (int)SteeringMode.Button) ? new Vector3(-240f, 0, 0) : new Vector3(240f, 0, 0);
            if (rect != null)
            {
                rect.anchoredPosition = new Vector2(targetPos.x, targetPos.y);
            }
            else
            {
                selectedLogo.transform.localPosition = targetPos;
            }
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}