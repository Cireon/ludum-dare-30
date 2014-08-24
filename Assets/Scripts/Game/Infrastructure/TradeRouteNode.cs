using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class TradeRouteNode
{
	public Node node;

	public Village village
	{
		get { return node.gameObject.GetSafeComponent<Village>(); }
	}

	public TradeRouteNode(Node n)
	{
		this.node = n;
	}
}
