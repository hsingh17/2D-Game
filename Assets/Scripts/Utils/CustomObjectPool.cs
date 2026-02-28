using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CustomObjectPool
{
    private readonly GameObject objectPrefab;
    public readonly ObjectPool<GameObject> Pool;

    public CustomObjectPool(GameObject objectPrefab, int defaultCapacity = 10, int maxSize = 25)
    {
        this.objectPrefab = objectPrefab;
        Pool = new(
            createFunc: CreateGameObject,
            actionOnGet: OnGetGameObject,
            actionOnRelease: OnReleaseGameObject,
            actionOnDestroy: OnDestroyGameObject,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private GameObject CreateGameObject()
    {
        GameObject gameObject = Object.Instantiate(objectPrefab);
        gameObject.SetActive(false);
        return gameObject;
    }

    private void OnGetGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnReleaseGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroyGameObject(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }
}
