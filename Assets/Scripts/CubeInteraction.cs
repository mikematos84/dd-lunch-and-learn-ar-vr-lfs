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

    private void OnEnable()
    {
        m_InteractiveEvent = GetComponent<OVRInteractionEvent>();
        m_InteractiveEvent.OnEnter += HandleEnter;
        m_InteractiveEvent.OnExit += HandleExit;
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
    }

    private void HandleEnter(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", hoverColor);
    }

    private void HandleExit(OVRInteractionEvent evt)
    {
        m_Renderer.material.SetColor("_Color", defaultColor);
    }
}
