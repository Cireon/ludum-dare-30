using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TradeRoute : MonoBehaviourBase
{
	public TradeHub start;
	public List<TradeRouteNode> nodes;
	public List<Vehicle> vehicles = new List<Vehicle>();

	public Commodity commodity;

	public Supply GetStartSupply()
	{
		var supplies = this.start.GetComponents<Supply>();
		foreach (var s in supplies)
			if (s.commodity == this.commodity)
				return s;
		return null;
	}

	public float averageStorage
	{
		get
		{
			float total = nodes.Aggregate(0f, (f, n) => f + n.village.FindStorage(commodity).amount);
			return total / nodes.Count;
		}
	}
}
