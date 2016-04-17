using UnityEngine;

namespace Mirage
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] Camera[] _cameras;

        void Start()
        {
            if (Application.isEditor)
            {
                _cameras[0].targetDisplay = 0;
                _cameras[1].targetDisplay = 1;
                _cameras[2].targetDisplay = 2;
            }
            else
            {
                // Hide mouse cursor.
                Cursor.visible = false;

                #if MIRAGE_SINGLE

                // Remap displays for the single display mode.
                _cameras[0].enabled = false;
                _cameras[2].enabled = false;
                _cameras[1].targetDisplay = 0;

                #elif MIRAGE_DUAL

                // Remap displays for the dual display mode.
                _cameras[0].enabled = false;
                _cameras[2].targetDisplay = 1;

                // Try activating multiple displays.
                TryActivateDisplay(0);
                TryActivateDisplay(1);

                #else

                // Remap displays for the triple display mode.
                _cameras[0].targetDisplay = 0;
                _cameras[1].targetDisplay = 1;
                _cameras[2].targetDisplay = 2;

                // Try activating multiple displays.
                TryActivateDisplay(0);
                TryActivateDisplay(1);
                TryActivateDisplay(2);

                #endif
            }
        }

        void TryActivateDisplay(int index)
        {
            if (index < Display.displays.Length)
            {
                Display.displays[index].Activate();
                CreateClearOnlyCamera(index);
            }
        }

        void CreateClearOnlyCamera(int index)
        {
            var go = new GameObject("Clear Camera " + index);
            var cam = go.AddComponent<Camera>();
            cam.depth = -1000;
            cam.cullingMask = 0;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.targetDisplay = index;
        }
    }
}
