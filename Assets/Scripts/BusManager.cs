using UnityEngine;
using UnityEngine.SceneManagement;

public class BusManager : MonoBehaviour
{
	public static BusManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindAnyObjectByType<BusManager>();
				if (_instance == null)
				{
					GameObject go = new GameObject("BusManager");
					_instance = go.AddComponent<BusManager>();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
	private static BusManager _instance;
	public GameObject selectedBusPrefab;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SelectBus(GameObject busPrefab)
    {
        selectedBusPrefab = busPrefab;
    }

    public void LoadGameplay(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}