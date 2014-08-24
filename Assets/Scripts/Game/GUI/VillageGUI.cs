using UnityEngine;
using System;

public class VillageGUI : MonoBehaviourBase
{
	public float width = 200;
	public float height = 200;
	public float padding = 10;
	public float margin = 5;

	private float x { get { return Screen.width - width - margin; } }
	private float y { get { return Screen.height - height - margin; } }
	private float insideWidth { get { return width - 2 * padding; } }

	void OnGUI()
	{
		var village = this.gameObject.GetSafeComponent<Village>();

		GUI.Box (new Rect(x, y, width, height), this.gameObject.name + " (" + village.size + ")");
		GUI.Label (new Rect(x + padding, y + padding + 10, insideWidth, 20), "Supply:");

		var supplies = this.GetComponents<Supply>();
		for (int i = 0; i < supplies.Length; i++)
			GUI.Label(new Rect(x + padding, y + padding + 30 + i * 20, insideWidth, 20), string.Format("- {0} {1}", supplies[i].amount, supplies[i].commodity.ToString()));

		GUI.Label (new Rect(x + padding, y + padding + 40 + supplies.Length * 20, insideWidth, 20), "Storage");
		for (int i = 0; i < village.storage.Length; i++)
			GUI.Label (new Rect(x + padding, y + padding + 60 + (i + supplies.Length) * 20, insideWidth, 20), string.Format ("{0}: {1:0.0}/{2:0.0}", village.storage[i].commodity, village.storage[i].amount, village.maxStorage));
	}
}
