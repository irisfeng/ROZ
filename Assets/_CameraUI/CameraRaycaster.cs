using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;				// for enemy detection

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        
        // inspector properties rendered by custom editor script
		[SerializeField] Texture2D walkCursor = null;
		[SerializeField] Texture2D enemyCursor = null;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

		const int POTENTIALLY_WALKABLE_LAYER = 8;
        float maxRaycastDepth = 100f;

		// TODO remove once working
        int topPriorityLayerLastFrame = -1;

		public delegate void OnMouseOverEnemy(Zerg zerg);  // FMI use zerg for enemy
		public event OnMouseOverEnemy onMouseOverEnemy;

		public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        void Update()
        {
            // Check if pointer is over an interactable UI element
			if (EventSystem.current.IsPointerOverGameObject ()) 
			{
				// Implement UI Interaction
			} else 
			{
				PerformRaycast ();
			}
        }

		void PerformRaycast() {
			// Specify layer priorities here
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (RaycastForEnemy(ray)) {
				return;
			}
			if (RaycastForPotentiallyWalkable(ray)) {
				return;
			}
		
		}

		private bool RaycastForEnemy(Ray ray){
			RaycastHit hitInfo;
			Physics.Raycast (ray, out hitInfo, maxRaycastDepth);
			var gameObjectHit = hitInfo.collider.gameObject;
			var enemyHit = gameObjectHit.GetComponent<Zerg> ();
			if (enemyHit) {
				Cursor.SetCursor (enemyCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverEnemy (enemyHit);
				return true;
			}
			return false;
		}

		private bool RaycastForPotentiallyWalkable(Ray ray){

			RaycastHit hitInfo;
			LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
			bool potentiallyWalkableHit = Physics.Raycast (ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
			if (potentiallyWalkableHit) {
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);     // TODO   set up virtual btn action instead of mouse click action
				onMouseOverPotentiallyWalkable(hitInfo.point);
				return true;
			}

			return false;
		}


    }
}