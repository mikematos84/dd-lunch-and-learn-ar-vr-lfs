using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(OVRInteractionEvent))]
public class OVRInteractionEventListener : MonoBehaviour
{
    protected OVRInteractionEvent m_Interaction;
	protected Material m_Material;
	protected Image m_Image;

	[SerializeField] bool visualize = true;
    [SerializeField] Color normalColor = Color.white;
	[SerializeField] Color hoverColor = Color.gray;
	[SerializeField] Color selectedColor = Color.cyan;
        
	public virtual void Awake ()
	{
        m_Interaction = GetComponent<OVRInteractionEvent> ();

		// 3d Element
		if (GetComponent<Renderer> () != null) {
			m_Material = GetComponent<Renderer> ().material;
			normalColor = m_Material.color;
		}

		// UI Element
		if (GetComponent<Image> () != null) {
			m_Image = GetComponent<Image> ();
			normalColor = m_Image.color;
		}
    }

    public virtual void OnEnable ()
	{
		m_Interaction.OnEnter += HandleEnter;
		m_Interaction.OnExit +=	HandleExit;
		m_Interaction.OnClick += HandleClick;
		m_Interaction.OnDown += HandleDown;
		m_Interaction.OnUp += HandleUp;
	}

	public virtual void OnDisable ()
	{
		m_Interaction.OnEnter -= HandleEnter;
		m_Interaction.OnExit -=	HandleExit;
		m_Interaction.OnClick -= HandleClick;
		m_Interaction.OnDown -= HandleDown;
		m_Interaction.OnUp -= HandleUp;
	}

	public virtual void HandleEnter (OVRInteractionEvent interaction)
	{
		if (!visualize)
			return;

		if (m_Material != null)
			m_Material.color = hoverColor;

		if (m_Image != null)
			m_Image.color = hoverColor;
	}

	public virtual void HandleExit (OVRInteractionEvent interaction)
	{
		if (!visualize)
			return;

		if (m_Material != null)
			m_Material.color = normalColor;

		if (m_Image != null)
			m_Image.color = normalColor;
	}

	public virtual void HandleClick (OVRInteractionEvent interaction)
	{
		if (!visualize)
			return;

		if (m_Material != null)
			m_Material.color = selectedColor;

		if (m_Image != null)
			m_Image.color = selectedColor;
	}

	public virtual void HandleDown (OVRInteractionEvent interaction)
	{
		if (!visualize)
			return;

		if (m_Material != null)
			m_Material.color = selectedColor;

		if (m_Image != null)
			m_Image.color = selectedColor;
	}

	public virtual void HandleUp (OVRInteractionEvent interaction)
	{
		if (!visualize)
			return;

		if (m_Material != null)
			m_Material.color = normalColor;

		if (m_Image != null)
			m_Image.color = normalColor;
	}
}