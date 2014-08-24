using UnityEngine;
using System;
using System.Collections.Generic;

public class TradeHubGUI : MonoBehaviour
{
	public float width = 215;
	public float height = 150;
	public float padding = 10;
	public float margin = 5;
	
	private float x { get { return margin; } }
	private float y { get { return Screen.height - height - margin; } }
	private float insideWidth { get { return width - 2 * padding; } }

	public Dictionary<string, Texture2D> commodityTextures = new Dictionary<string, Texture2D>();

	void Start()
	{
		var names = Enum.GetNames(typeof(Commodity));
		foreach (var n in names)
		{
			commodityTextures.Add (n, (Texture2D)Resources.Load("Commodities/" + n.ToLower(), typeof(Texture2D)));
		}
	}

	void OnGUI()
	{
		if (!this.GetComponent<Supply>())
			return;

		GUI.Box (new Rect(x, y, width, height), gameObject.name + " trade hub");

		TradeHub hub;
		if ((hub = this.GetComponent<TradeHub>()) != null)
		{
			for (int i = 0; i < hub.routes.Length; i++)
			{
				GUI.Label (new Rect(x + padding, y + padding + 10 + i * 24, 60, 20), string.Format ("Route {0}: ", i + 1));

				TradeRoute route;
				if ((route = hub.routes[i]) != null)
				{
					GUI.Label (new Rect(x + padding + 60, y + padding + 10 + i * 24, 20, 20), commodityTextures[route.commodity.ToString()]);
					GUI.Label (new Rect(x + padding + 80, y + padding + 10 + i * 24, 50, 20), "Carts: " + route.vehicles.Count);
					if (GUI.Button (new Rect(x + padding + 130, y + padding + 10 + i * 24, 20, 20), "+") && CurrencyManager.Instance.carts > 0)
					{
						CurrencyManager.Instance.carts--;
						var obj = (GameObject)Instantiate (CurrencyManager.Instance.vehiclePrefab);
						obj.transform.position = route.start.transform.position;
						var vehicle = obj.GetComponent<Vehicle>();
						vehicle.route = route;
						route.vehicles.Add (vehicle);

						obj.transform.parent = Infrastructure.Instance.vehicles.transform;
					}
					if (GUI.Button (new Rect(x + padding + 150, y + padding + 10 + i * 24, 20, 20), "-") && route.vehicles.Count > 0)
					{
						Destroy (route.vehicles[route.vehicles.Count - 1].gameObject);
						route.vehicles.RemoveAt(route.vehicles.Count - 1);
						CurrencyManager.Instance.carts++;
					}
					if (GUI.Button (new Rect(x + padding + 175, y + padding + 10 + i * 24, 20, 20), "x"))
					{
						foreach (var v in route.vehicles)
							Destroy(v.gameObject);
						Destroy(route.gameObject);
					}
                }
                else
				{
					GUI.Label (new Rect(x + padding + 60, y + padding + 10 + i * 24, 50, 20), "[none]");
					if (GUI.Button(new Rect(x + padding + 110, y + padding + 10 + i * 24, insideWidth - 110, 20), "Create"))
					{
						var editor = this.gameObject.AddComponent<TradeRouteEditor>();
						var obj = new GameObject(string.Format ("{0} route {1}", hub.gameObject.name, i + 1));
                        route = obj.AddComponent<TradeRoute>();
						route.start = hub;
						route.nodes = new System.Collections.Generic.List<TradeRouteNode>();
						//route.transform.parent = Infrastructure.Instance.routes.transform;

						hub.routes[i] = route;
						editor.route = route;
					}
				}
			}
		}
		else
		{
			if (GUI.Button(new Rect(x + padding, y + padding + 16, insideWidth, 20), "Purchase trade hub [$100]"))
			{
				CurrencyManager.Instance.money -= 100;
				this.gameObject.AddComponent<TradeHub>();
			}
		}
	}
}
