using UnityEngine;
using System.Collections;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {

        [SerializeField]
        Texture2D walkCursor = null;
        [SerializeField]
        Texture2D targetCursor = null;
        [SerializeField]
        Texture2D unknownCursor = null;
        [SerializeField]
        Vector2 cursorHotspot = new Vector2(0, 0);

        // TODO solve fight between serialize and const 
        [SerializeField]
        const int walkableLayerNumber = 8;
        [SerializeField]
        const int enemyLayerNumber = 9;

        CameraRaycaster camRaycaster;

        // Use this for initialization
        void Start()
        {
            camRaycaster = GetComponent<CameraRaycaster>();
            camRaycaster.notifyLayerChangeObservers += OnLayerChanged; // registering
        }

        void OnLayerChanged(int newLayer)
        {

            switch (newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case enemyLayerNumber:
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }

        // TODO consider de-registering OnLayerChanged on leaving all game scenes
    }
}
