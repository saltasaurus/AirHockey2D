using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] Button _onePlayerButton;
	[SerializeField] Button _twoPlayerButton;
	public ButtonStates ButtonState { get; private set; }
	public enum ButtonStates 
    {
		Menu,
		OnePlayer,
		TwoPlayer
    }
	void Start()
	{
		ButtonState = ButtonStates.Menu;
		Button btn1player = _onePlayerButton.GetComponent<Button>();
		Button btn2player = _twoPlayerButton.GetComponent<Button>();
		btn1player.onClick.AddListener(OnClickOnePlayer);
		btn2player.onClick.AddListener(OnClickTwoPlayer);
	}

	void OnClickOnePlayer()
	{
		ButtonState = ButtonStates.OnePlayer;
		Debug.Log("You have chosen 1 player mode");

	}
	void OnClickTwoPlayer()
    {
		ButtonState = ButtonStates.TwoPlayer;
		Debug.Log("You have chosen 2 player mode");
    }
}
