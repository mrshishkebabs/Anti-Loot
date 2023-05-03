using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRen;
    [SerializeField] private GameObject highlight;
    public static GameObject currentTile;

    public void Init(bool isOffset)
    {
        spriteRen.color = isOffset ? offsetColor : baseColor;
    }

    private void Update()
    {
        bool mouseOver = false;
        Collider2D[] mouseColliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        foreach (Collider2D mouseCollider in mouseColliders)
        {
            if (mouseCollider.gameObject == gameObject) 
            {
                mouseOver = true;
                break;
            }
        }
        
        if (mouseOver) 
        {
            currentTile = gameObject;
            highlight.SetActive(true);
        }
        else 
        {
            highlight.SetActive(false);
        }            
    }
}
