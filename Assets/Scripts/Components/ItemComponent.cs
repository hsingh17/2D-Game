using System.Collections;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        LevelItemManager levelItemManager = GetComponentInParent<LevelItemManager>();
        if (levelItemManager)
        {
            levelItemManager.HandleItemGet(gameObject);
        }
    }
}
