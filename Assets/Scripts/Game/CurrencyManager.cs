using UnityEngine;
using System.Collections;

public class CurrencyManager : Singleton<CurrencyManager>
{
	public float fullCommodityPrice;
	public float money;
	public float carts;
	public float nextPeriodicPayments;

	public GameObject vehiclePrefab;

	private Component gameOver;

	void Start () {
		nextPeriodicPayments = 60;
	}

	void Update () {
		if (gameOver) return;

		this.nextPeriodicPayments -= Time.deltaTime;
		if (nextPeriodicPayments < 0)
		{
			this.processPeriodicPayments();
			nextPeriodicPayments += 60;
		}

		if (money < 0)
		{
			InputManager.Instance.enabled = false;
			gameOver = gameObject.AddComponent<GameOverGUI>();
		}
	}

	void processPeriodicPayments()
	{
		var vehicles = Infrastructure.Instance.vehicles.GetComponentsInChildren<Vehicle>();
		foreach (var v in vehicles)
			this.money -= v.costPerMinute;
	}

	#region GUI
	void OnGUI() {
		GUI.Box (new Rect(10,10,100,54), "");
		GUI.Label (new Rect(16, 16, 92, 24), string.Format ("$ {0:0}", money));
		GUI.Label (new Rect(16, 40, 92, 24), string.Format ("Carts: {0:0}", carts));

		if (!gameOver && !gameObject.GetComponent<RoadBuildEditor>())
		{
			if (GUI.Button (new Rect(10, 72, 100, 24), "Buy Cart ($50)"))
			{
				this.carts++;
				this.money -= 50;
			}

			if (GUI.Button (new Rect(10, 100, 100, 24), "Build roads"))
			{
				gameObject.AddComponent<RoadBuildEditor>();
			}
		}

		var audio = Camera.main.GetComponent<AudioSource>();
		if (GUI.Button (new Rect(Screen.width - 100, 10, 90, 20), audio.mute ? "Play sound" : "Mute sound"))
			audio.mute = !audio.mute;
	}
	#endregion
}
