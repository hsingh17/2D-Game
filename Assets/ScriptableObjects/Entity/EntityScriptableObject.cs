using UnityEngine;

[CreateAssetMenu(fileName = "EntityScriptableObject", menuName = "Scriptable Objects/Entity")]
public class EntityScriptableObject : ScriptableObject
{
    public int health = 3;
    public float speed = 5f;
    public float jumpHeight = 10f;
    public float attackDamage = 1f;
}
