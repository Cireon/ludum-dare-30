using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager> {
	const float arrowMoveFactor = 6;
	const float dragMoveFactor = 1;
	const float borderMoveSize = 16;

	ISelectionHandler[] selectedObjects;
	public bool selectionEnabled = true;

	public Vector3 lastMousePos;

	// Use this for initialization
	void Start() { }
	
	// Update is called once per frame
	void Update () {
		#region Camera movement
		if (Input.GetKey(KeyCode.Alpha0))
			Camera.main.transform.position = new Vector3(0, 0, -10);

		if (Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y >= 0 && Input.mousePosition.y <= borderMoveSize))
			Camera.main.transform.SetY(Camera.main.transform.position.y - arrowMoveFactor * Time.deltaTime);
		if (Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y <= Screen.height && Input.mousePosition.y >= Screen.height - borderMoveSize))
			Camera.main.transform.SetY(Camera.main.transform.position.y + arrowMoveFactor * Time.deltaTime);
		if (Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x >= 0 && Input.mousePosition.x <= borderMoveSize))
			Camera.main.transform.SetX(Camera.main.transform.position.x - arrowMoveFactor * Time.deltaTime);
		if (Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x <= Screen.width && Input.mousePosition.x >= Screen.width - borderMoveSize))
			Camera.main.transform.SetX(Camera.main.transform.position.x + arrowMoveFactor * Time.deltaTime);

		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			lastMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			var mousePos = Input.mousePosition;
			var diff = Camera.main.ScreenToWorldPoint(lastMousePos) - Camera.main.ScreenToWorldPoint(mousePos);
			Camera.main.transform.position += dragMoveFactor * diff;
			lastMousePos = mousePos;
		}
		#endregion

		#region Scrolling
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel"), 1f, 12f);
		#endregion

		if (selectionEnabled)
			if (Input.GetMouseButtonDown(0))
			{
				var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null)
				{
					var handlers = hit.collider.gameObject.GetInterfaceComponents<ISelectionHandler>();
					if (handlers != null)
					{
						if (this.selectedObjects != null)
							foreach (var obj in this.selectedObjects)
								obj.OnDeselect();

						this.selectedObjects = handlers;

						if (this.selectedObjects != null)
							foreach (var obj in this.selectedObjects)
								obj.OnSelect();
					}
				}
			}
	}

	public void EnableSelection()
	{
		this.selectionEnabled = true;
		
		if (this.selectedObjects != null)
			foreach (var obj in this.selectedObjects)
				obj.OnSelect();
	}
    
    public void DisableSelection()
	{
		this.selectionEnabled = false;

		if (this.selectedObjects != null)
			foreach (var obj in this.selectedObjects)
				obj.OnDeselect();
    }
}
