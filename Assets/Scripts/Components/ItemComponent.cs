using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Logger.Log($"{other.tag}");
        Destroy(gameObject);
        // Item component needs some kind of associated "Fx" variable for knowing what to play when collected
        // https://www.youtube.com/watch?v=U08ScgT3RVM
    }
}
