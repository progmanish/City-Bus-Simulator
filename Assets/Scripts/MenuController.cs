using UnityEngine;

public class MenuController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameManager.instance.state = GameState.MainMenu;
	}
	public void StartGame(string _name)
	{
		GameManager.instance.LoadGameScene(_name);
		SoundManager.instance.PlayUIButtonClicks();
	}
}