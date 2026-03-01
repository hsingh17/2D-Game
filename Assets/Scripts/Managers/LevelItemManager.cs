using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelItemManager : MonoBehaviour
{
    [SerializeField]
    private float itemCollectFeedbackDuration = 0.5f;

    [SerializeField]
    private int itemCollectFeedbackPoolDefaultCapacity = 10;

    [SerializeField]
    private int itemCollectFeedbackPoolMaxSize = 25;

    [SerializeField]
    private GameObject itemCollectFeedbackPrefab;

    private ObjectPool<GameObject> itemCollectFeedbackPool;

    private void Awake()
    {
        itemCollectFeedbackPool = new(
            createFunc: CreateGameObject,
            actionOnGet: OnGetGameObject,
            actionOnRelease: OnReleaseGameObject,
            actionOnDestroy: OnDestroyGameObject,
            collectionCheck: true,
            defaultCapacity: itemCollectFeedbackPoolDefaultCapacity,
            maxSize: itemCollectFeedbackPoolMaxSize
        );
    }

    private void Start()
    {
        AddItemComponentToItems();
    }

    private void AddItemComponentToItems()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            // Add ItemComponent if not already attached
            if (!child.gameObject.TryGetComponent(out ItemComponent _))
            {
                child.gameObject.AddComponent<ItemComponent>();
            }
        }
    }

    private GameObject CreateGameObject()
    {
        GameObject gameObject = Instantiate(itemCollectFeedbackPrefab);
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

    public void HandleItemGet(GameObject item)
    {
        GameObject itemCollectFeedback = itemCollectFeedbackPool.Get();
        itemCollectFeedback.transform.position = item.transform.position;
        Destroy(item);
        StartCoroutine(ReturnAfter(itemCollectFeedback, itemCollectFeedbackDuration));
    }

    private IEnumerator ReturnAfter(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        itemCollectFeedbackPool.Release(gameObject);
    }
}
