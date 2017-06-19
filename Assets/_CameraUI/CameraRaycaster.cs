using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        //   public Layer[] layerPriorities = {
        //       Layer.Enemy,
        //       Layer.Walkable
        //   };

        //[SerializeField]
        //float distanceToBackground = 100f;
        //   Camera viewCamera;

        //RaycastHit raycastHit;
        //   public RaycastHit hit
        //   {
        //       get { return raycastHit; }
        //   }

        //Layer layerHit;
        //   public Layer currentLayerHit
        //   {
        //	get { return layerHit; }
        //   }

        // inspector properties rendered by custom editor script
        [SerializeField] int[] layerPriorities;

        float maxRaycastDepth = 100f;
        int topPriorityLayerLastFrame = -1;

        //public delegate void OnLayerChange(Layer newLayer); 		// declare new delegate type
        //public event OnLayerChange onLayerChange; 	// instantiate an observer set

        // Setup delegates for broadcasting layer changes to other classes
        public delegate void OnCursorLayerChange(int newLayer); // declare new delegate type
        public event OnCursorLayerChange notifyLayerChangeObservers; // instantiate a observer set

        public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit); // declare new delegate type
        public event OnClickPriorityLayer notifyMouseClickObservers; // instantiate a observer set


        //void Start()
        //{
        //    viewCamera = Camera.main;

        //}

        void Update()
        {
            //    // Look for and return priority layer hit
            //    foreach (Layer layer in layerPriorities)
            //    {
            //        var hit = RaycastForLayer(layer);
            //        if (hit.HasValue)
            //        {
            //            raycastHit = hit.Value;
            //if (layerHit != layer) {	// if layer has changed
            //	layerHit = layer;
            //	onLayerChange (layer);	// call the delegates
            //}
            //            return;
            //        }
            //    }

            //    // Otherwise return background hit
            //    raycastHit.distance = distanceToBackground;
            //    layerHit = Layer.RaycastEndStop;
            //    onLayerChange(layerHit);

            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                NotifyObserversIfLayerChanged(5);
                return;         // Stop looking for other objects
            }

            // Raycast to max depth, every frame as things can move under mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

            RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
            if (!priorityHit.HasValue)  // if hit no priority object
            {
                NotifyObserversIfLayerChanged(0);       // broadcast default layer
                return;
            }

            // Notify delegate of layer change
            var layerHit = priorityHit.Value.collider.gameObject.layer;
            NotifyObserversIfLayerChanged(layerHit);

            // Notify delegates of highest priority game object under mouse when clicked
            if (Input.GetMouseButton(0))
            {
                notifyMouseClickObservers(priorityHit.Value, layerHit);
            }
        }

        void NotifyObserversIfLayerChanged(int newLayer)
        {
            if (newLayer != topPriorityLayerLastFrame)
            {
                topPriorityLayerLastFrame = newLayer;
                notifyLayerChangeObservers(newLayer);
            }
        }

        //RaycastHit? RaycastForLayer(Layer layer)
        //{
        //    int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        //    Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hit; // used as an out parameter
        //    bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        //    if (hasHit)
        //    {
        //        return hit;
        //    }
        //    return null;
        //}

        RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits)
        {
            // Form list of layer numbers hit
            List<int> layersOfHitColliders = new List<int>();
            foreach (RaycastHit hit in raycastHits)
            {
                layersOfHitColliders.Add(hit.collider.gameObject.layer);
            }

            // Step through layers in order of priority looking for a gameobject with that layer
            foreach (int layer in layerPriorities)
            {
                foreach (RaycastHit hit in raycastHits)
                {
                    if (hit.collider.gameObject.layer == layer)
                    {
                        return hit; // stop looking
                    }
                }
            }
            return null; // because cannot use GameObject? nullable
        }
    }
}