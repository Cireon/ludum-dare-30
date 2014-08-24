using UnityEngine;
using System;
using System.Collections;

public enum Commodity
{
	Wine,
	Mead,
	Meat,
	Books
}

public class Supply : MonoBehaviourBase {
	public Commodity commodity;
	public int amount;
	public float storage;

	private int maxStorage { get { return this.amount * 3; } }

	// Use this for initialization
	void Start () {
		this.storage = .5f * this.maxStorage;
	}
	
	// Update is called once per frame
	void Update () {
		this.storage = Math.Min(this.maxStorage, this.storage + this.amount * .01f * Time.deltaTime);

		var village = this.gameObject.GetSafeComponent<Village>();
		village.PushCommodity(this.commodity, village.maxStorage);
	}
}
