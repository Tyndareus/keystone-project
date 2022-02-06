using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnablePrefab;
    [SerializeField] private List<Sprite> spawnableItems;
    [SerializeField] private float timeBetweenSpawns;
    
    public void BeginSpawning() => StartCoroutine(SpawnItems());

    private IEnumerator SpawnItems()
    {
        while (TimerManager.InPlay)
        {
            GameObject item = Instantiate(spawnablePrefab, transform.position, Quaternion.identity);
            item.GetComponent<SpriteRenderer>().sprite = spawnableItems[Random.Range(0, spawnableItems.Count)];;
            item.AddComponent<BoxCollider2D>();
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3.5f, 4.75f), ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void OnScore() => timeBetweenSpawns = Mathf.Clamp(timeBetweenSpawns - 0.15f, 0.0f, 4f);
}
