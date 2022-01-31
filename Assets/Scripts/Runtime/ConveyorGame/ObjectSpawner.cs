using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnablePrefab;
    [SerializeField] private List<ConveyorItemData> spawnableItems;

    [SerializeField] private float timeBetweenSpawns;
    

    public void BeginSpawning()
    {
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (TimerManager.InPlay)
        {
            GameObject item = Instantiate(spawnablePrefab, transform.position, Quaternion.identity);

            item.GetComponent<SpriteRenderer>().sprite = spawnableItems[0].sprite;
            
            switch (spawnableItems[0].collision)
            {
                case CollisionType.Circle:
                    item.AddComponent<BoxCollider2D>(); 
                    break;
                case CollisionType.Square:
                    item.AddComponent<CircleCollider2D>();
                    break;
            }
            
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1.5f, 3.75f), ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
