using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TradeRouteEditor : MonoBehaviour
{
	public TradeRoute route;
	public Dictionary<string, Texture2D> commodityTextures = new Dictionary<string, Texture2D>();

	private Supply[] supplies;
	private int selectedSupply;

	// Use this for initialization
	void Start () {
		InputManager.Instance.DisableSelection();
		LoadTextures();

		this.supplies = this.GetComponents<Supply>();
		for (int i = 0; i < supplies.Length; i++)
			if (supplies[i].commodity == route.commodity)
			{
				selectedSupply = i;
				break;
			}
	}

	void LoadTextures()
	{
		var names = Enum.GetNames(typeof(Commodity));
		foreach (var n in names)
		{
			commodityTextures.Add (n, (Texture2D)Resources.Load("Commodities/" + n.ToLower(), typeof(Texture2D)));
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.route.commodity = this.supplies[selectedSupply].commodity;

		if (Input.GetMouseButtonDown(0))
		{
			var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if (hit.collider != null)
			{
				var node = hit.collider.GetComponent<Node>();

				if (node != null)
					if (node.GetComponent<TradeHub>() != route.start && route.nodes.All(tn => tn.node != node))
						route.nodes.Add (new TradeRouteNode(node));
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			End();
		}
	}
	
	void End()
	{
		InputManager.Instance.EnableSelection();
		Destroy(this);
	}

	#region GUI
	public float guiWidth = 160;
	public float guiHeight = 240;
	public float guiPadding = 10;
	public float guiMargin = 5;
	
	private float guiX { get { return guiMargin; } }
	private float guiY { get { return Screen.height - guiHeight - guiMargin; } }
	private float guiInsideWidth { get { return guiWidth - 2 * guiPadding; } }

	public Vector2 scrollPosition = Vector2.zero;

	void OnGUI()
	{
		GUI.Box (new Rect(guiX, guiY, guiWidth, guiHeight), "Edit trade route");
		if (GUI.Button(new Rect(guiX + guiPadding, guiY + guiHeight - guiPadding - 20, guiInsideWidth, 20), "Done"))
			End();

		this.selectedSupply = GUI.SelectionGrid(new Rect(guiX + guiPadding, guiY + 24, guiInsideWidth, 32), this.selectedSupply, this.supplies.Select(supply => this.commodityTextures[supply.commodity.ToString()]).ToArray(), 4);

		scrollPosition = GUI.BeginScrollView(new Rect(guiX + guiPadding, guiY + 64 + guiPadding, guiInsideWidth, guiHeight - 2 * guiPadding - 64 - 32),
		                                     scrollPosition, new Rect(0, 0, guiInsideWidth - 8, 20 * (route.nodes.Count + 1)));
		GUI.Label(new Rect(0, 0, guiInsideWidth - 8, 20), route.start.gameObject.name);
		for (int i = 0; i < route.nodes.Count; i++)
		{
			GUI.Label (new Rect(0, (i + 1) * 20, guiInsideWidth - 8, 20), route.nodes[i].node.gameObject.name);
			if (GUI.Button (new Rect(guiInsideWidth - 28, (i + 1) * 20, 20, 20), "x"))
            {
				route.nodes.RemoveAt(i);
			}
		}
		GUI.EndScrollView();
	}
	#endregion
}
