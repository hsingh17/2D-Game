using UnityEngine;
using UnityEngine.Pool;

public class CustomObjectPool
{
    private readonly GameObject objectPrefab;
    private readonly ObjectPool<GameObject> pool;

    public CustomObjectPool(GameObject objectPrefab, int defaultCapacity = 10, int maxSize = 25)
    {
        this.objectPrefab = objectPrefab;
        pool = new(
            createFunc: CreateGameObject,
            actionOnGet: OnGetGameObject,
            actionOnRelease: OnReleaseGameObject,
            actionOnDestroy: OnDestroyGameObject,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public GameObject GetGameObject()
    {
        return pool.Get();
    }

    public void ReleaseGameObject(GameObject gameObject)
    {
        pool.Release(gameObject);
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
