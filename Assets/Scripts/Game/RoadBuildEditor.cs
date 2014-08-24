using UnityEngine;
using System.Linq;

public class RoadBuildEditor : MonoBehaviour
{
	public Node from, to;
	float cost;
	int selectedType;

	// Use this for initialization
	void Start () {
		InputManager.Instance.DisableSelection();
	}

	void End()
	{
		InputManager.Instance.EnableSelection();
		Destroy(this);
	}

	void Reset()
	{
		cost = 0;
		this.from = null;
		this.to = null;
	}

	void Build()
	{
		var obj = (GameObject)Instantiate(RoadPrefabs.Instance.prefabs[selectedType]);

		obj.name = from.gameObject.name + " - " + to.gameObject.name;

		var road = obj.GetComponent<Road>();

		road.From = from;
		road.To = to;

		obj.transform.parent = Infrastructure.Instance.roads.transform;

		CurrencyManager.Instance.money -= cost;

		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((!from || !to) && Input.GetMouseButtonDown(0))
		{
			var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if (hit.collider != null)
			{
				var node = hit.collider.GetComponent<Node>();
				
				if (node != null)
				{
					if (!from)
						from = node;
					else if (node != from)
						to = node;
				}
			}
		}

		if (from && to)
			cost = Mathf.Round ((to.transform.position - from.transform.position).magnitude * RoadPrefabs.Instance.prefabs[selectedType].GetComponent<Road>().costPerUnit);
		
		if (Input.GetMouseButtonDown(1))
		{
			End();
		}
	}

	#region GUI
	public float guiWidth = 160;
	public float guiHeight = 230;
	public float guiPadding = 10;
	public float guiMargin = 5;
	
	private float guiX { get { return guiMargin; } }
	private float guiY { get { return Screen.height - guiHeight - guiMargin; } }
	private float guiInsideWidth { get { return guiWidth - 2 * guiPadding; } }
	
	public Vector2 scrollPosition = Vector2.zero;
	
	void OnGUI()
	{
		GUI.Box (new Rect(guiX, guiY, guiWidth, guiHeight), "Build roads");

		GUI.Label (new Rect(guiX + guiPadding, guiY + 24, guiInsideWidth, 20), "From: " + (from ? from.gameObject.name : ""));
		GUI.Label (new Rect(guiX + guiPadding, guiY + 44, guiInsideWidth, 20), "To: " + (to ? to.gameObject.name : ""));

		selectedType = GUI.SelectionGrid(new Rect(guiX + guiPadding, guiY + 72, guiInsideWidth, 24 * RoadPrefabs.Instance.prefabs.Length), selectedType, RoadPrefabs.Instance.prefabs.Select(obj => obj.name).ToArray(), 1);

		if (from && to)
			if (GUI.Button (new Rect(guiX + guiPadding, guiY + guiHeight - guiPadding - 60, .63f * guiInsideWidth - 2, 20), string.Format ("Build (${0:0})", cost)))
				Build();
		if (from)
			if (GUI.Button (new Rect(guiX + guiPadding + .63f * guiInsideWidth + 4, guiY + guiHeight - guiPadding - 60, .37f * guiInsideWidth - 2, 20), "Reset"))
				Reset();

		if (GUI.Button(new Rect(guiX + guiPadding, guiY + guiHeight - guiPadding - 20, guiInsideWidth, 20), "Done"))
			End();
	}
	#endregion
}
