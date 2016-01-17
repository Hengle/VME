using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;

namespace VME {

    /// <summary>
    /// Global functions used on multiple locations thereby easier
    /// to reference as a global function.
    /// </summary>
    public class VMEGlobal {

        /// <summary>
        /// Hides the default transform controls.
        /// </summary>
        public static bool Hidden {
            get {
                Type type = typeof(Tools);
                FieldInfo field = type.GetField("s_Hidden", BindingFlags.NonPublic | BindingFlags.Static);
                return ((bool)field.GetValue(null));
            }
            set {
                Type type = typeof(Tools);
                FieldInfo field = type.GetField("s_Hidden", BindingFlags.NonPublic | BindingFlags.Static);
                field.SetValue(null, value);
            }
        }

        /// <summary>
        /// Returns the Game object on mouse position.
        /// </summary>
        public static GameObject GetTileAtMousePosition () {

            Event e = Event.current;

            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(
                e.mousePosition.x, Screen.height - e.mousePosition.y - 36, 0)); //Upside-down and offset a little because of menus

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 1000.0f)) {

                if (hit.collider.gameObject.GetComponent<ChunkObjectData>()) {

                    return hit.collider.gameObject;

                } else {

                    Debug.LogWarning("The location you want to paint at is not on the same chunk as selected.");

                }

            } else {

                return null;

            }

            return null;

        }

        /// <summary>
        /// Returns the position next to the side you pressed on.
        /// </summary>
        public static Vector3 GetPositionNextToHoveredTile () {

            Event e = Event.current;

            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(
                e.mousePosition.x, Screen.height - e.mousePosition.y - 36, 0)); //Upside-down and offset a little because of menus

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 1000.0f)) {

                if (hit.collider.gameObject.GetComponent<ChunkObjectData>()) {

                    return hit.collider.gameObject.transform.position + hit.normal;

                }
                else {

                    Debug.LogWarning("The location you want to paint at is not on the same chunk as selected.");

                }

            }
            else {

                return new Vector3(0, 9000, 0);

            }

            return new Vector3(0, 9000, 0);

        }

    }

}