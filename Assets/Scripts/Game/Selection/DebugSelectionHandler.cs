using UnityEngine;
using System.Collections;

public class DebugSelectionHandler : MonoBehaviourBase, ISelectionHandler {
	public void OnSelect()
	{
		Debug.Log (this.gameObject.name + " selected");
	}

	public void OnDeselect()
	{
		Debug.Log (this.gameObject.name + " deselected");
	}
}
