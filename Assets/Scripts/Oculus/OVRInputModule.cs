using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DD
{
    public class OVRInputModule : UnityEngine.EventSystems.OVRInputModule
    {
        [Header("Gear VR Controller")]
        public Transform trackingSpace;
        public LineRenderer lineRenderer;

        override protected MouseState GetGazePointerData()
        {
            // Get the OVRRayPointerEventData reference
            OVRPointerEventData leftData;
            GetPointerData(kMouseLeftId, out leftData, true);
            leftData.Reset();

            //Now set the world space ray. This ray is what the user uses to point at UI elements
            OVRInput.Controller controller = OVRInput.GetConnectedControllers() & (OVRInput.Controller.LTrackedRemote | OVRInput.Controller.RTrackedRemote);
            if (lineRenderer != null)
            {
                lineRenderer.enabled = trackingSpace != null && controller != OVRInput.Controller.None;
            }
            if (trackingSpace != null && controller != OVRInput.Controller.None)
            {
                controller = ((controller & OVRInput.Controller.LTrackedRemote) != OVRInput.Controller.None) ? OVRInput.Controller.LTrackedRemote : OVRInput.Controller.RTrackedRemote;

                Quaternion orientation = OVRInput.GetLocalControllerRotation(controller);
                Vector3 localStartPoint = OVRInput.GetLocalControllerPosition(controller);

                Matrix4x4 localToWorld = trackingSpace.localToWorldMatrix;
                Vector3 worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
                Vector3 worldOrientation = localToWorld.MultiplyVector(orientation * Vector3.forward);
                leftData.worldSpaceRay = new Ray(worldStartPoint, worldOrientation);
                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(0, worldStartPoint);
                    lineRenderer.SetPosition(1, worldStartPoint + worldOrientation * 500.0f);
                }
            }
            else
            {
                leftData.worldSpaceRay = new Ray(rayTransform.position, rayTransform.forward);
            }

            leftData.scrollDelta = GetExtraScrollDelta();

            //Populate some default values
            leftData.button = PointerEventData.InputButton.Left;
            leftData.useDragThreshold = true;
            // Perform raycast to find intersections with world
            eventSystem.RaycastAll(leftData, m_RaycastResultCache);
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            OVRRaycaster ovrRaycaster = raycast.module as OVRRaycaster;
            // We're only interested in intersections from OVRRaycasters
            if (ovrRaycaster)
            {
                // The Unity UI system expects event data to have a screen position
                // so even though this raycast came from a world space ray we must get a screen
                // space position for the camera attached to this raycaster for compatability
                leftData.position = ovrRaycaster.GetScreenPosition(raycast);


                // Find the world position and normal the Graphic the ray intersected
                RectTransform graphicRect = raycast.gameObject.GetComponent<RectTransform>();
                if (graphicRect != null)
                {
                    // Set are gaze indicator with this world position and normal
                    Vector3 worldPos = raycast.worldPosition;
                    Vector3 normal = GetRectTransformNormal(graphicRect);
                    OVRGazePointer.instance.SetPosition(worldPos, normal);
                    // Make sure it's being shown
                    OVRGazePointer.instance.RequestShow();
                    if (lineRenderer != null)
                    {
                        lineRenderer.SetPosition(1, raycast.worldPosition);
                    }
                }
            }

            // Now process physical raycast intersections
            OVRPhysicsRaycaster physicsRaycaster = raycast.module as OVRPhysicsRaycaster;
            if (physicsRaycaster)
            {
                Vector3 position = raycast.worldPosition;

                if (performSphereCastForGazepointer)
                {
                    // Here we cast a sphere into the scene rather than a ray. This gives a more accurate depth
                    // for positioning a circular gaze pointer
                    List<RaycastResult> results = new List<RaycastResult>();
                    physicsRaycaster.Spherecast(leftData, results, OVRGazePointer.instance.GetCurrentRadius());
                    if (results.Count > 0 && results[0].distance < raycast.distance)
                    {
                        position = results[0].worldPosition;
                    }
                }

                leftData.position = physicsRaycaster.GetScreenPos(raycast.worldPosition);

                // Show the cursor while pointing at an interactable object
                OVRGazePointer.instance.RequestShow();
                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(1, raycast.worldPosition);
                }

                if (matchNormalOnPhysicsColliders)
                {
                    OVRGazePointer.instance.SetPosition(position, raycast.worldNormal);
                }
                else
                {
                    OVRGazePointer.instance.SetPosition(position);
                }
            }




            // Stick default data values in right and middle slots for compatability

            // copy the apropriate data into right and middle slots
            OVRPointerEventData rightData;
            GetPointerData(kMouseRightId, out rightData, true);
            CopyFromTo(leftData, rightData);
            rightData.button = PointerEventData.InputButton.Right;

            OVRPointerEventData middleData;
            GetPointerData(kMouseMiddleId, out middleData, true);
            CopyFromTo(leftData, middleData);
            middleData.button = PointerEventData.InputButton.Middle;


            m_MouseState.SetButtonState(PointerEventData.InputButton.Left, GetGazeButtonState(), leftData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Right, PointerEventData.FramePressState.NotChanged, rightData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, PointerEventData.FramePressState.NotChanged, middleData);
            return m_MouseState;
        }

        static Vector3 GetRectTransformNormal(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 BottomEdge = corners[3] - corners[0];
            Vector3 LeftEdge = corners[1] - corners[0];
            rectTransform.GetWorldCorners(corners);
            return Vector3.Cross(BottomEdge, LeftEdge).normalized;
        }

        private readonly MouseState m_MouseState = new MouseState();
    }
}
