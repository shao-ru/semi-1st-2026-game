using UnityEngine;
using System.Collections;

public class CrowAttack : MonoBehaviour
{
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float minTime = 2f;
    [SerializeField] private float maxTime = 4f;

    private void Start()
    {
        StartCoroutine(DropPoop());
    }

    IEnumerator DropPoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            Instantiate(
                poopPrefab,
                firePoint.position,
                Quaternion.identity);
        }
    }
}
///やりますねぇ～