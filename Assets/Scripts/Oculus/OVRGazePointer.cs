using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD
{
    public class OVRGazePointer : global::OVRGazePointer
    {
        // Change "private" to "protected" in base class
        // public bool hidden { get; private set; } => public bool hidden { get; protected set; }

        void Update()
        {
            
            if (rayTransform == null && Camera.main != null)
                rayTransform = Camera.main.transform;

            // Should we show or hide the gaze cursor?
            if (visibilityStrength == 0 && !hidden)
            {
                Hide();
            }
            else if (visibilityStrength > 0 && hidden)
            {
                Show();
            }
        }

        void Hide()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().enabled = false;
            hidden = true;
        }

        void Show()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().enabled = true;
            hidden = false;
        }
    }
}
