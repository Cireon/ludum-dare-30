using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviourBase
{
	public TradeRoute route;

	public float capacity;
	public float speed;
	public float loadingSpeed;

	public enum State
	{
		Idle,
		Loading,
		Moving
	}

	public State currentState = State.Idle;
	private int currentNode = -1;
	public float currentCapacity;

	// Moving
	public float distanceTravelled;
	public float distanceToTravel;
	private Transform from, to;

	void Update()
	{
		switch (this.currentState)
		{
			case State.Loading:
			if (this.currentNode == -1)	// Loading at trade hub
			{
				var supply = this.route.GetStartSupply();
				var loadable = Mathf.Min (this.capacity - this.currentCapacity, this.loadingSpeed * Time.deltaTime, supply.storage);
				if (loadable > 0)
				{
					this.currentCapacity += loadable;
					supply.storage -= loadable;
            	}

				if (loadable <= 0 || supply.storage <= 0 || this.currentCapacity >= this.capacity)
				{
					this.currentState = State.Moving;
					this.ProceedNode();
				}
			}
			else // Unloading at village
			{
				Village village = this.route.nodes[this.currentNode].village;
				var storage = village.FindStorage(this.route.commodity);
				var unloadable = Mathf.Min (this.currentCapacity, this.loadingSpeed * Time.deltaTime, village.maxStorage - storage.amount);

				if (unloadable > 0)
				{
					this.currentCapacity -= unloadable;
					storage.amount += unloadable;
            	}

				if (unloadable <= 0 || storage.amount >= village.maxStorage || this.currentCapacity <= 0)
				{
					this.currentState = State.Moving;
					this.ProceedNode();
				}
			}
			break;
				
			case State.Moving:
			this.distanceTravelled += speed * Time.deltaTime;
			if (distanceTravelled >= distanceToTravel)
			{
				this.distanceTravelled = distanceToTravel;
				this.currentState = State.Loading;
			}
			this.transform.position = this.from.position + (this.to.position - this.from.position) * (this.distanceTravelled / this.distanceToTravel);
			break;
		}
	}

	void ProceedNode()
	{
		var oldNode = this.currentNode;

		currentNode++;
		if (currentNode == this.route.nodes.Count)
			currentNode = -1;

		from = TransformFromIndex(oldNode);
		to = TransformFromIndex(currentNode);

		distanceToTravel = (from.position - to.position).magnitude;
		distanceTravelled = 0;
	}

	Transform TransformFromIndex(int index)
	{
		if (index == -1)
			return route.start.transform;
		else
			return route.nodes[index].node.transform;
	}
}
