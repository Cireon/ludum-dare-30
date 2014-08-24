using UnityEngine;
using System.Collections;

public class TradeHubGUI : MonoBehaviour
{
	public float width = 200;
	public float height = 200;
	public float padding = 10;
	public float margin = 5;
	
	private float x { get { return margin; } }
	private float y { get { return Screen.height - height - margin; } }
	private float insideWidth { get { return width - 2 * padding; } }
	
	void OnGUI()
	{
		GUI.Box (new Rect(x, y, width, height), "Trade hub");

		TradeHub hub;
		if ((hub = this.GetComponent<TradeHub>()) != null)
		{
			for (int i = 0; i < hub.routes.Length; i++)
			{
				GUI.Label (new Rect(x + padding, y + padding + 10 + i * 20, 60, 20), string.Format ("Route {0}: ", i + 1));

				TradeRoute route;
				if ((route = hub.routes[i]) != null)
				{
					GUI.Label (new Rect(x + padding + 60, y + padding + 10 + i * 20, 50, 20), route.commodity.ToString());

					if (GUI.Button(new Rect(x + padding + 110, y + padding + 10 + i * 20, insideWidth - 110, 20), "Edit"))
					{
						var editor = this.gameObject.AddComponent<TradeRouteEditor>();
                        editor.route = route;
                    }
                }
                else
				{
					GUI.Label (new Rect(x + padding + 60, y + padding + 10 + i * 20, 50, 20), "[none]");
					if (GUI.Button(new Rect(x + padding + 110, y + padding + 10 + i * 20, insideWidth - 110, 20), "Create"))
					{
						var editor = this.gameObject.AddComponent<TradeRouteEditor>();
						var obj = new GameObject(string.Format ("{0} route {1}", hub.gameObject.name, i + 1));
                        route = obj.AddComponent<TradeRoute>();
						route.start = hub;
						route.nodes = new System.Collections.Generic.List<TradeRouteNode>();

						hub.routes[i] = route;
						editor.route = route;
					}
				}
			}
		}
		else
		{
			if (GUI.Button(new Rect(x + padding, y + padding + 10, insideWidth, 20), "Purchase trade hub"))
				this.gameObject.AddComponent<TradeHub>();
		}
	}
}
