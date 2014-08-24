using UnityEngine;
using System.Collections;

public class GameOverGUI : MonoBehaviour {

	private const float width = 200;
	private const float height = 100;

	void OnGUI()
	{
		GUI.Box (new Rect(.5f * Screen.width - .5f * width, .5f * Screen.height - .5f * height, width, height), "Game over");
		GUI.Label (new Rect(.5f * Screen.width - .5f * width + 43, .5f * Screen.height - 18, width - 60, 24), "You went bankrupt");

		if (GUI.Button (new Rect(.5f * Screen.width - .5f * width + 8, .5f * Screen.height + .5f * height - 32, width - 16, 24), "Restart game"))
			Application.LoadLevel(0);
	}
}
