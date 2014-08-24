using UnityEngine;
using System;

public class Village : MonoBehaviourBase {
	public int size;
	public Storage[] storage;

	public GameObject loading;

	public int maxStorage
	{
		get { return 3 * this.size; }
	}

	// Use this for initialization
	void Start () {
		var commodities = Enum.GetValues(typeof(Commodity));
		this.storage = new Storage[commodities.Length];
		for (int i = 0; i < this.storage.Length; i++)
			this.storage[i] = new Storage
			{
				commodity = (Commodity)commodities.GetValue(i),
				amount = 0
			};

		var supplies = gameObject.GetComponents<Supply>();
		if (supplies == null || supplies.Length == 0)
			return;

		for (int i = 0; i < supplies.Length; i++)
		{
			var obj = new GameObject();

			var sprite = obj.AddComponent<SpriteRenderer>();
			sprite.sprite = (Sprite)Resources.Load("Commodities/" + supplies[i].commodity.ToString().ToLower(), typeof(Sprite));

			obj.transform.position = transform.position - new Vector3(sprite.bounds.size.x * (i - (supplies.Length - 1) * .5f), -.5f, .1f);
			obj.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (var s in this.storage)
		{
			s.amount = Math.Max(0, s.amount - this.size * Time.deltaTime * 0.01f);
		}
	}

	public bool RequestLoadingSpot(GameObject vehicle)
	{
		if (loading) return false;

		this.loading = vehicle;
		return true;
	}

	public void ReleaseLoadingSpot(GameObject vehicle)
	{
		if (loading != vehicle) throw new Exception();

		this.loading = null;
	}

	public void PushCommodity(Commodity commodity, float amount)
	{
		var s = this.FindStorage(commodity);

		s.amount = Math.Min (this.maxStorage, s.amount + amount);
	}

	public float GetPriceFor(Commodity commodity, float amount)
	{
		var s = this.FindStorage(commodity);
		return amount * .5f * (pricePerUnitAt(s.amount) + pricePerUnitAt(s.amount + amount));
	}

	private float pricePerUnitAt(float amount)
	{
		float ratio = amount / maxStorage;

		return CurrencyManager.Instance.fullCommodityPrice * (ratio < .5
			? 1 - 6 * ratio * ratio * ratio * ratio
			: 2 * (ratio - 1) * (ratio - 1));
	}

	public Storage FindStorage(Commodity commodity)
	{
		// this.storage[(int)commodity]

		for (int i = 0; i < this.storage.Length; i++)
			if (this.storage[i].commodity == commodity)
				return this.storage[i];
		throw new Exception();
	}

	[Serializable]
	public class Storage
	{
		public Commodity commodity;
		public float amount;
	}
}
