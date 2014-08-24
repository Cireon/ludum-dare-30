using UnityEngine;
using System;
using System.Collections.Generic;

public class VillageGUI : MonoBehaviourBase
{
	public float width = 120;
	public float height = 120;
	public float padding = 10;
	public float margin = 5;

	private float x { get { return Screen.width - width - margin; } }
	private float y { get { return Screen.height - height - margin; } }
	private float insideWidth { get { return width - 2 * padding; } }

	Dictionary<string, Texture2D> commodityTextures = new Dictionary<string, Texture2D>();
	Texture2D supply, demand;

	void Start()
	{
		var names = Enum.GetNames(typeof(Commodity));
		foreach (var n in names)
		{
			commodityTextures.Add (n, (Texture2D)Resources.Load("Commodities/" + n.ToLower(), typeof(Texture2D)));
		}

		supply = (Texture2D)Resources.Load ("Icons/supply", typeof(Texture2D));
		demand = (Texture2D)Resources.Load ("Icons/demand", typeof(Texture2D));
	}

	void OnGUI()
	{
		var village = this.gameObject.GetSafeComponent<Village>();

		GUI.Box (new Rect(x, y, width, height), this.gameObject.name + " (" + village.size + ")");

		var enumValues = Enum.GetValues(typeof(Commodity));
		var supplies = this.GetComponents<Supply>();

		for (int i = 0; i < enumValues.Length; i++)
		{
			var c = (Commodity)enumValues.GetValue (i);
			Supply s;

			GUI.Label(new Rect(x + padding, y + padding + 10 + 24 * i, 20, 20), this.commodityTextures[c.ToString()]);

			if ((s = Array.Find (supplies, supp => supp.commodity == c)) != null)
			{
				GUI.Label(new Rect(x + padding + 30, y + padding + 15 + 24 * i, 20, 20), supply);
				GUI.Label(new Rect(x + padding + 42, y + padding + 10 + 24 * i, 100, 20), string.Format ("{0:0.0} / {1:0.0}", s.storage, s.maxStorage));
			}
			else
			{
				var storage = village.FindStorage(c);

				GUI.Label(new Rect(x + padding + 30, y + padding + 15 + 24 * i, 20, 20), demand);
				GUI.Label(new Rect(x + padding + 42, y + padding + 10 + 24 * i, 100, 20), string.Format ("{0:0.0} / {1:0.0}", storage.amount, village.maxStorage));
			}
		}
	}
}
