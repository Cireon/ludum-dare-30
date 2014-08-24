using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour
{
	public Node From, To;
	public float SpeedFactor;

	// Use this for initialization
	void Awake ()
	{
		var fromPosition = From.transform.position;
		var toPosition = To.transform.position;

		var diff = toPosition - fromPosition;

		var sprite = this.GetComponent<SpriteRenderer>();

		transform.position = fromPosition + .5f * diff;
		var angle = Mathf.Atan2(diff.y, diff.x);
		transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, new Vector3(0,0,1));
		transform.localScale = new Vector3(diff.magnitude * 6.4f, 1, 1);

		var childPrefab = new GameObject();
		var childSprite = childPrefab.AddComponent<SpriteRenderer>();
		childPrefab.transform.position = transform.position;
		childPrefab.transform.rotation = transform.rotation;
		childSprite.sprite = sprite.sprite;
		
		GameObject child;
		int m = Mathf.RoundToInt(transform.localScale.x);
		for (int i = 1; i < m; i++)
		{
			child = (GameObject)Instantiate(childPrefab);
			child.transform.position = fromPosition + (i + .5f) * diff / m;
			child.transform.parent = transform;
		}

		childPrefab.transform.position = fromPosition + .5f * diff / m;
		childPrefab.transform.parent = transform;
		
		sprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
