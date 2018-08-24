using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class CubeInteraction : MonoBehaviour {

    OVRInteractionEvent m_InteractiveEvent;
    VRInput m_VRInput;
    Renderer m_Renderer;
    
    [SerializeField] Color defaultColor;
    [SerializeField] Color hoverColor;
    [SerializeField] Color activeColor;

    private void OnEnable()
    {
        m_InteractiveEvent = GetComponent<OVRInteractionEvent>();
        m_InteractiveEvent.OnEnter += HandleEnter;
        m_InteractiveEvent.OnExit += HandleExit;
        m_InteractiveEvent.OnDown += HandleDown;
        m_InteractiveEvent.OnUp += HandleUp;
    }
        
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        defaultColor = m_Renderer.material.GetColor("_Color");
    }

    private void OnDisable()
    {
        m_InteractiveEvent.OnEnter -= HandleEnter;
        m_InteractiveEvent.OnExit -= HandleExit;
        m_InteractiveEvent.OnDown -= HandleDown;
        m_InteractiveEvent.OnUp -= HandleUp;
    }

    private void HandleEnter(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", hoverColor);
    }

    private void HandleExit(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", defaultColor);
    }

    private void HandleDown(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", activeColor);
    }

    private void HandleUp(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", hoverColor);
    }
}
