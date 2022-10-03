using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    public TileBase[] sideTiles;
    public Sprite[] sideSprites;
    public Image dieImage;
    public Animator animator;

    public int value = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll()
    {
        animator.SetBool("isRolling", true);
    }

    public void StopRoll()
    {
        animator.SetBool("isRolling", false);
    }

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int roll)
    {
        value = roll;
        dieImage.overrideSprite = roll > 0 ? GetSideSprite(roll) : null;
    }

    public TileBase GetValueTile()
    {
        return GetSideTile(value);
    }

    public TileBase GetSideTile(int index)
    {
        return sideTiles[index];
    }

    public Sprite GetSideSprite(int index)
    {
        return sideSprites[index];
    }
}
