using UnityEngine;
using System.Collections.Generic;

public class Vehicle : MonoBehaviourBase
{
	public TradeRoute route;

	public float capacity;
	public float speed;
	public float costPerMinute;
	public float loadingSpeed;

	public enum State
	{
		Waiting,
		Loading,
		Moving
	}

	public State currentState = State.Waiting;
	private int currentNode = -1;
	public float currentCapacity;

	// Moving
	public Queue<Node> path;
	public float distanceTravelled;
	public float distanceToTravel;
	public Transform from, to;
	public float speedFactor;

	void Update()
	{
		switch (this.currentState)
		{
			case State.Waiting:
			if (this.NodeFromIndex(currentNode).GetComponent<Village>().RequestLoadingSpot(this.gameObject))
				currentState = State.Loading;
			break;

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
					CurrencyManager.Instance.money += village.GetPriceFor(this.route.commodity, unloadable);
					storage.amount += unloadable;
            	}

				if (unloadable <= 0 || storage.amount >= village.maxStorage || this.currentCapacity <= 0 || (storage.amount >= village.maxStorage && storage.amount >= 1.5 * route.averageStorage))
				{
					this.currentState = State.Moving;
					this.ProceedNode();
				}
			}
			break;
				
			case State.Moving:
			this.distanceTravelled += speed * Time.deltaTime * speedFactor;
			if (distanceTravelled >= distanceToTravel)
			{
				if (path.Count == 0)
				{
					path.Clear ();
					this.distanceTravelled = distanceToTravel;
					this.currentState = State.Waiting;
				}
				else
				{
					from = to;
					to = path.Dequeue().transform;
					speedFactor = from.GetComponent<Node>().SpeedFactorTo(to.GetComponent<Node>());
					distanceTravelled -= distanceToTravel;
					distanceToTravel = (from.position - to.position).magnitude;
				}
			}
			this.transform.position = this.from.position + (this.to.position - this.from.position) * (this.distanceTravelled / this.distanceToTravel);
			break;
		}
	}

	void ProceedNode()
	{
		NodeFromIndex(currentNode).GetComponent<Village>().ReleaseLoadingSpot(this.gameObject);

		var oldNode = this.currentNode;

		currentNode++;
		if (currentNode == this.route.nodes.Count)
			currentNode = -1;

		CalculatePath(NodeFromIndex(oldNode), NodeFromIndex(currentNode));

		distanceToTravel = 0;
		distanceTravelled = 0;
		speedFactor = .5f;
		to = NodeFromIndex (oldNode).transform;
	}

	Node NodeFromIndex(int index)
	{
		if (index == -1)
			return route.start.GetComponent<Node>();
		else
			return route.nodes[index].node;
	}

	void CalculatePath(Node from, Node to)
	{
		var queue = new PriorityQueue<Node>();
		var dict = new Dictionary<Node, DijkstraState>();
		
		queue.Add(from, 0);

		var fromState = new DijkstraState();
		fromState.foundPathLength = 0;

		dict.Add(from, fromState);
		float foundPath = float.PositiveInfinity;
		
		while (queue.Count > 0 && queue.GetPriorityAt(0) < foundPath)
		{
			var node = queue.Pop();
			var nodeState = dict[node];
			
			foreach (var arc in node.arcs)
			{
				if (!dict.ContainsKey(arc.node))
				{
					queue.Add(arc.node, float.PositiveInfinity);
					dict.Add (arc.node, new DijkstraState());
				}

				var pathViaLength = nodeState.foundPathLength + arc.road.length / arc.road.SpeedFactor;
				var state = dict[arc.node];

				if (state.foundPathLength > pathViaLength)
				{
					state.foundPathLength = pathViaLength;
					state.parent = node;
					queue.SetPriority(arc.node, pathViaLength);
					queue.Sort ();
					
					if (arc.node == to)
						foundPath = pathViaLength;
				}
			}
		}

		// Walk over grass
		if (foundPath * foundPath > (to.transform.position - from.transform.position).sqrMagnitude * 4)
		{
			this.path = new Queue<Node>();
			this.path.Enqueue(to);
			return;
		}
		
		var pathList = new LinkedList<Node>();
		var current = to;
		Node parent;
		while ((parent = dict[current].parent) != null)
		{
			pathList.AddFirst(current);
			current = parent;
		}

		this.path = new Queue<Node>(pathList);
	}
	
	private class DijkstraState
	{
		public Node parent;
		public float foundPathLength = float.PositiveInfinity;
	}
}
