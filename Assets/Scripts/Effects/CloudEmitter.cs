using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEmitter : MonoBehaviour
{
    [SerializeField] private float cloudSpawnTime = 10f;
    [SerializeField] private float cloudYPositionRange = 5f;
    [SerializeField] private List<GameObject> cloudPrefabs;
    [SerializeField] private Transform cloudSpawnLocation;

    private void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    private IEnumerator SpawnClouds()
    {
        while (true)
        {
            var spawnTime = cloudSpawnTime;
            spawnTime -= Random.Range(0, 2.5f);
            yield return new WaitForSeconds(spawnTime);
            SpawnCloud();
        }
    }

    private void SpawnCloud()
    {
        var cloudIndex = Random.Range(0, cloudPrefabs.Count);
        var prefab = cloudPrefabs[cloudIndex];
        var pos = cloudSpawnLocation.position;
        pos.y += Random.Range(-cloudYPositionRange / 2f, cloudYPositionRange / 2f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
