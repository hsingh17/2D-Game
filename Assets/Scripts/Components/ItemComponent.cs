using System.Collections;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject onCollectEffect;

    private CustomObjectPool objectPool;

    private void Awake()
    {
        objectPool = new(onCollectEffect);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO:  Need to make item invisible since destroying means obj never released
        // also pool should be shared rn tied to one item
        GameObject effect = objectPool.GetGameObject();
        effect.transform.position = transform.position;
        StartCoroutine(ReturnAfter(gameObject, 1f));
    }

    private IEnumerator ReturnAfter(GameObject gameObject, float seconds)
    {
        Logger.Log("courutine");
        yield return new WaitForSeconds(seconds);
        Logger.Log("releasing");
        objectPool.ReleaseGameObject(gameObject);
    }
}
