using UnityEngine;
using System;

public class Village : MonoBehaviourBase {
	public int size;
	public Storage[] storage;

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
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (var s in this.storage)
		{
			s.amount = Math.Max(0, s.amount - this.size * Time.deltaTime * 0.01f);
		}
	}

	public void PushCommodity(Commodity commodity, float amount)
	{
		var s = this.FindStorage(commodity);

		s.amount = Math.Min (this.maxStorage, s.amount + amount);
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
