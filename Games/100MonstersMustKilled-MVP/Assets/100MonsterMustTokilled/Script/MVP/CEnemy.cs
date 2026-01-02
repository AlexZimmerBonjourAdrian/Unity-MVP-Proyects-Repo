using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
public class CEnemy : MonoBehaviour
{
    public float life;
    public bool isDead;

  

    [SerializeField] private Sprite[] enemySprites;  // Array to hold enemy sprites
    [SerializeField] private UnityEngine.UI.Image image; // The SpriteRenderer component

   void Start()
    {
        SetRandomSprite(); // Set a random sprite on start
    }

    public void DiscountLife(float lifeSubstract)
    {
        life -= lifeSubstract;
    }

    public void SetRandomSprite()
    {
        if (enemySprites != null && enemySprites.Length > 0 && image != null)
        {
            int randomIndex = Random.Range(0, enemySprites.Length);
            image.sprite = enemySprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("Enemy Sprites array is empty or Image component is not assigned!");
        }
    }


    // Example: A function to set a specific sprite by index (if needed)
    public void SetSpriteByIndex(int index)
    {
        if (enemySprites != null && index >= 0 && index < enemySprites.Length && image != null)
        {
            image.sprite = enemySprites[index];
        }
        else
        {
            Debug.LogWarning("Invalid sprite index,  Enemy Sprites array is empty, or Image component is not assigned!");
        }
    }

   


}
