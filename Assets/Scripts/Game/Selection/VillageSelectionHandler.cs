using UnityEngine;
using System.Collections;

public class VillageSelectionHandler : MonoBehaviourBase, ISelectionHandler, IHoverHandler {
	public void OnSelect()
	{
		if (!this) return;

		this.gameObject.AddComponent<TradeHubGUI>();
	}

	public void OnDeselect()
	{
		if (!this) return;

		Component gui;

		gui = this.gameObject.GetComponent<TradeHubGUI>();
		if (gui) Destroy(gui);
	}

	public void OnHover()
	{
		if (!this) return;

		this.gameObject.AddComponent<VillageGUI>();
	}

	public void OnHoverEnd()
	{
		if (!this) return;

		Component gui;
		
		gui = this.gameObject.GetComponent<VillageGUI>();
		if (gui) Destroy(gui);
	}
}
