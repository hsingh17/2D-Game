using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        LevelItemManager levelItemManager = GetComponentInParent<LevelItemManager>();
        if (levelItemManager)
        {
            levelItemManager.HandleItemGet(gameObject);
        }
    }
}
