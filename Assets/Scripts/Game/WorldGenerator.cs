using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldGenerator : Singleton<WorldGenerator>
{
	public int numVillages;
	public float radius;

	public GameObject villagePrefab;
	public GameObject housePrefab;

	// Use this for initialization
	void Start ()
	{
		var namesAsset = (TextAsset)Resources.Load ("townnames", typeof(TextAsset));
		var names = namesAsset.text.Split (new [] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		var used = new bool[names.Length];

		var villages = new GameObject[numVillages];

		int ni = -1;

		for (int i = 0; i < numVillages; i++)
		{
			Vector3 newPosition = Vector3.zero;

			int triesLeft = 10;

			while (true)
			{
				var pos2d = i == 0 ? Vector2.zero : radius * Random.insideUnitCircle;
				newPosition = new Vector3(pos2d.x, pos2d.y, villagePrefab.transform.position.z);

				if (villages.Take(i).All (v => (v.transform.position - newPosition).sqrMagnitude > 25) || triesLeft-- <= 0)
					break;
			}

			villages[i] = (GameObject)Instantiate(villagePrefab);
			villages[i].transform.position = newPosition;

			while (ni == -1 || used[ni])
				ni = Random.Range (0, names.Length);
			villages[i].name = names[ni];
			used[ni] = true;
			villages[i].transform.parent = Infrastructure.Instance.villages.transform;

			var size = Random.Range (5, 15);
			villages[i].GetComponent<Village>().size = size;
			for (int j = 0; j < size; j++)
			{
				var house = (GameObject)Instantiate(housePrefab);

				house.transform.localScale = house.transform.localScale * Random.Range (.9f, 1.1f);
				house.transform.rotation = Quaternion.AngleAxis(Random.Range (0, 360f), new Vector3(0, 0, 1));
				var xy = Random.insideUnitCircle;
				house.transform.position = villages[i].transform.position + .4f * new Vector3(xy.x, xy.y, 0);

				house.transform.parent = villages[i].transform;
			}
		}

		var enumValues = System.Enum.GetValues(typeof(Commodity));
		foreach (var v in enumValues)
		{
			var c = (Commodity)v;

			var hs = new HashSet<int>();
			hs.Add (0);

			for (int i = 0; i < villages.Length / 7; i++)
			{
				int j = 0;
				while (hs.Contains (j))
					j = Random.Range (0, villages.Length);
				hs.Add (j);

				var supply = (Supply)villages[j].AddComponent<Supply>();
				supply.amount = Random.Range (5,25);
				supply.commodity = c;
			}
		}

		villages[0].transform.parent = Infrastructure.Instance.villages.transform;
	}
}
