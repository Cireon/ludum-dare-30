using UnityEngine;
using System.Collections;

public class VillageSelectionHandler : MonoBehaviourBase, ISelectionHandler {
	public void OnSelect()
	{
		if (!this) return;

		this.gameObject.AddComponent<VillageGUI>();
		this.gameObject.AddComponent<TradeHubGUI>();
	}

	public void OnDeselect()
	{
		if (!this) return;

		Component gui;

		gui = this.gameObject.GetComponent<VillageGUI>();
		if (gui) Destroy(gui);
		gui = this.gameObject.GetComponent<TradeHubGUI>();
		if (gui) Destroy(gui);
	}
}
