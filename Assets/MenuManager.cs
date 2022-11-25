using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
	[SerializeField] Button _onePlayerButton;
	[SerializeField] Button _twoPlayerButton;
	[SerializeField] Button _mainMenuButton;
	[SerializeField] Button _replayButton;
	[SerializeField] Slider _mainMenuAiDifficultySlider;
	[SerializeField] TextMeshProUGUI startMenuAIDifficulty;

	public bool ButtonPressed = false;
	public int difficulty = 1;
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
		_onePlayerButton.onClick.AddListener(OnClickOnePlayer);
		_twoPlayerButton.onClick.AddListener(OnClickTwoPlayer);
		_mainMenuButton.onClick.AddListener(OnClickMainMenu);
		_replayButton.onClick.AddListener(OnClickReplay);
		_mainMenuAiDifficultySlider.onValueChanged.AddListener(OnSliderChange);

	}

	void OnSliderChange(float value)
    {
		startMenuAIDifficulty.text = value.ToString();
    }

    void OnClickOnePlayer()
	{
		ButtonState = ButtonStates.OnePlayer;
		difficulty = (int)_mainMenuAiDifficultySlider.value;

	}
	void OnClickTwoPlayer()
    {
		ButtonState = ButtonStates.TwoPlayer;
    }

	void OnClickReplay()
    {
		ButtonPressed = true;
    }

	void OnClickMainMenu()
    {
		ButtonState = ButtonStates.Menu;
		ButtonPressed = true;
    }
}
