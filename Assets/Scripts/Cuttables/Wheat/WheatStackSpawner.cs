using System;
using System.Collections;
using UnityEngine;

namespace MidlandFarm.Scripts.Cuttables.Wheat
{
    public class WheatStackSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject blockPrefab;

        public void Spawn()
        {
            var block = Instantiate(blockPrefab, spawnPoint.position, blockPrefab.transform.rotation, spawnPoint);
        }
    }
}
