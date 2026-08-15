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
        SoundManager.instance.PlayUIButtonClicks();
        panelSound.SetActive(true);
        panelControls.SetActive(false);
        panelAbout.SetActive(false);
        panelReset.SetActive(false);
    }

    public void ClickControls()
    {
        SoundManager.instance.PlayUIButtonClicks();
        panelSound.SetActive(false);
        panelControls.SetActive(true);
        panelAbout.SetActive(false);
        panelReset.SetActive(false);

        RefreshLogoPosition();
    }

    public void ClickAbout()
    {
        SoundManager.instance.PlayUIButtonClicks();
        panelSound.SetActive(false);
        panelControls.SetActive(false);
        panelAbout.SetActive(true);
        panelReset.SetActive(false);
    }

    public void ClickReset()
    {
        SoundManager.instance.PlayUIButtonClicks();
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

        SaveService.SetSteeringMode((int)newMode);

        if (selectedLogo != null)
        {
            RectTransform rect = selectedLogo.GetComponent<RectTransform>();
            Vector3 targetPos = (newMode == SteeringMode.Button) ? new Vector3(-240f, 0, 0) : new Vector3(240f, 0, 0);
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
            SoundManager.instance.PlayUIButtonClicks();
            SaveService.ResetAll();
            if (GameManager.instance != null)
            {
                GameManager.instance.LoadData();
            }
            if (BusManager.instance != null)
            {
                BusManager.instance.SelectBus(null);
            }
            //Debug.Log("Data Reset Success");
        }
        OpenScene("SelectionMenu");
    }

    private void RefreshLogoPosition()
    {
        int savedMode = GameManager.instance != null ? (int)GameManager.instance.steeringMode : (int)SteeringMode.Button;
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
        SoundManager.instance.PlayUIButtonClicks();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}