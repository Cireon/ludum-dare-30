using UnityEngine;
using System.Collections;

public class RoadUpgradeGUI : MonoBehaviour
{
	public float width = 160;
	public float height = 100;
	public float padding = 10;
	public float margin = 5;
	
	private float x { get { return margin; } }
	private float y { get { return Screen.height - height - margin; } }
	private float insideWidth { get { return width - 2 * padding; } }

	public Road road;

	void OnGUI()
	{
		GUI.Box (new Rect(x, y, width, height), this.gameObject.name);
		GUI.Label (new Rect(x + padding, y + 16, insideWidth, 20), "Upgrade:");

		for (int i = 0; i < road.upgradePrefabs.Length; i++)
		{
			var p = road.upgradePrefabs[i];
			var c = road.length * (road.upgradePrefabs[i].GetComponent<Road>().costPerUnit - road.costPerUnit);

			if (GUI.Button (new Rect(x + padding, y + height - padding - 24 * road.upgradePrefabs.Length + i * 24, insideWidth, 20), string.Format ("{0} (${1:0}", road.upgradePrefabs[i].name, c)))
			{
				var newObj = (GameObject)Instantiate (p);
				var newRoad = newObj.GetComponent<Road>();
				newRoad.From = road.From;
				newRoad.To = road.To;
				newObj.transform.parent = Infrastructure.Instance.roads.transform;
				
				Destroy (road.gameObject);
			}
		}
	}
}
