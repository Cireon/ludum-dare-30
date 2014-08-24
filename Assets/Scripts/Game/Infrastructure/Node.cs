using System;
using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviourBase
{
	public List<OutgoingArc> arcs;

	public void AddOutgoingArc(Node node, Road via)
	{
		this.arcs.Add (new OutgoingArc {
			road = via, node = node
		});
	}

	public float SpeedFactorTo(Node n)
	{
		foreach (var a in arcs)
			if (a.node == n)
				return a.road.SpeedFactor;
		return 0.5f;
	}
}

[Serializable]
public class OutgoingArc
{
	public Road road;
	public Node node;
}