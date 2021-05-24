using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAlignment : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This needs to be set to the mouse cursor object")]
    private GameObject pointer;
    private PlayerMovement cursor;

    [HideInInspector]
    public Renderer m_Renderer;

    [SerializeField]
    [Tooltip("To which faction is this territory aligned. Set to 1 for allied, 2 for contested, 3 for enemy")]
    [Range(1,2)]
    private int alignment; //1 for ally, 2 for enemy, 3 for contested

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        cursor = pointer.GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        if (!Selected())
        { 
            SetFactionColor();
        }
    }

    public void SetFactionColor()
    {
        if (alignment == 1) //if allied
        {
            m_Renderer.sharedMaterial.color = Color.cyan;
        }
        if (alignment == 2) //if enemy
        {
            m_Renderer.sharedMaterial.color = Color.magenta;
        }
        else if (alignment == 3) //if contested
        {
            m_Renderer.sharedMaterial.color = Color.red;
        }
    }

    private bool Selected()
    {
        if (gameObject == cursor.selected && alignment != 1)
        {
            m_Renderer.sharedMaterial.color = Color.green;
            return true;
        }
        return false;
    }
}
