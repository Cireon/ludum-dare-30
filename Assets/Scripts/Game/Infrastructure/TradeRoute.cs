using UnityEngine;
using System.Collections.Generic;

public class TradeRoute : MonoBehaviourBase
{
	public TradeHub start;
	public List<TradeRouteNode> nodes;

	public Commodity commodity;

	public Supply GetStartSupply()
	{
		var supplies = this.start.GetComponents<Supply>();
		foreach (var s in supplies)
			if (s.commodity == this.commodity)
				return s;
		return null;
	}
}
