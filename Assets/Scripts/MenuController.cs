using UnityEngine;

public class MenuController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameManager.instance.state = GameState.MainMenu;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartGame(string name)
	{
		GameManager.instance.LoadGameScene(name);
	}
}