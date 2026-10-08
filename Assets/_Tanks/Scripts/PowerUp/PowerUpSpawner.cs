using System.Collections;
using UnityEngine;

namespace Tanks.Complete
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [Tooltip("Array that holds different power-up prefabs that can be spawned.")]
        public PowerUp[] m_PowerUps;
        [Tooltip("Time in seconds that will wait this spawner to instantiate a new power up when collected the new one.")]
        public float m_RespawnCooldown = 20f;


        private void Start()
        {
            // Spawn a random power up when the game starts.
            SpawnRandomPowerUp();
        }

        private void SpawnRandomPowerUp()
        {
            // Ensure there are power ups available to spawn.
            if (m_PowerUps != null && m_PowerUps.Length > 0)
            {
                int first = Random.Range(0, m_PowerUps.Length);
                for (int offset = 0; offset < m_PowerUps.Length; offset++)
                {
                    int randomNumber = (first + offset) % m_PowerUps.Length;
                    if (m_PowerUps[randomNumber] == null) continue;
                    Vector3 positionToSpawn = transform.position;
                    positionToSpawn.y = 1.09f;
                    PowerUp m_SpawnedPowerup = Instantiate(m_PowerUps[randomNumber], positionToSpawn, Quaternion.identity);
                    if (m_SpawnedPowerup != null)
                        m_SpawnedPowerup.SetSpawner(this);
                    break;
                }
            }
        }

        // Called when a power up is collected, starting a respawn timer.
        public void CollectPowerUp()
        {
            StartCoroutine(RespawnPowerUp());
        }

        private IEnumerator RespawnPowerUp()
        {
            // Wait for the cooldown time then spawns a power up.
            yield return new WaitForSeconds(m_RespawnCooldown);
            SpawnRandomPowerUp();
        }
    }
}
