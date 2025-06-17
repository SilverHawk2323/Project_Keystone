using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    public UnitBase[] units;

    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnLocations.Count; i++)
        {

            var unitSpawned = Instantiate(units[Random.Range(0, units.Length)], spawnLocations[Random.Range(0, spawnLocations.Count)]);
            unitSpawned.SetTeamNumber(2);
        }
    }
}
