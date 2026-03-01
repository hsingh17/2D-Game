using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private int defaultCapacity = 10;

    [SerializeField]
    private int maxSize = 25;

    private ObjectPool<GameObject> pool;

    void Awake()
    {
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
        GameObject gameObject = Instantiate(objectPrefab);
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
        Destroy(gameObject);
    }
}
