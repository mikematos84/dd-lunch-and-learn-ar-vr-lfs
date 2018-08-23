using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OVRInteractionEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Action<OVRInteractionEvent> OnEnter;
    public Action<OVRInteractionEvent> OnExit;
    public Action<OVRInteractionEvent> OnClick;
    public Action<OVRInteractionEvent> OnDown;
    public Action<OVRInteractionEvent> OnUp;

    PointerEventData pointerEvent;

    private OVRInputModule m_InputModule;

    private void Awake()
    {
        m_InputModule = FindObjectOfType<OVRInputModule>();
    }

    public virtual void OnPointerEnter(PointerEventData pointerEvent)
    {
        this.pointerEvent = pointerEvent;

        if (OnEnter != null)
            OnEnter(this);
    }

    public virtual void OnPointerExit(PointerEventData pointerEvent)
    {
        this.pointerEvent = pointerEvent;

        if (OnExit != null)
            OnExit(this);
    }

    public virtual void OnPointerClick(PointerEventData pointerEvent)
    {
        this.pointerEvent = pointerEvent;

        if (OnClick != null)
            OnClick(this);
    }

    public virtual void OnPointerDown(PointerEventData pointerEvent)
    {
        this.pointerEvent = pointerEvent;

        if (OnDown != null)
            OnDown(this);
    }

    public virtual void OnPointerUp(PointerEventData pointerEvent)
    {
        this.pointerEvent = pointerEvent;

        if (OnUp != null)
            OnUp(this);
    }
}
