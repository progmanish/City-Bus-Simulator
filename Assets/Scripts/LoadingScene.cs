using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider loadingBarFill;

    private void Start()
    {
        GameManager.instance.SetLoadingSceneOnGameplay(this);
		loadingScreen.SetActive(false);
    }

    public void LoadScene(string _name) => StartCoroutine(LoadSceneAsync(_name));

	IEnumerator LoadSceneAsync(string _name)
	{
		AsyncOperation _op = SceneManager.LoadSceneAsync(_name);
		loadingScreen.SetActive(true);

		while (!_op.isDone)
		{
			float _progressValue = Mathf.Clamp01(_op.progress / 0.9f);
			loadingBarFill.value = _progressValue;
			yield return null;
		}
	}
}