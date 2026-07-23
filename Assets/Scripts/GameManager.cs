using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] zones;

    [Header("Generation Settings")]
    [SerializeField] private float zoneSpaceY = 1;
    [SerializeField] private float zoneHeight = 6;
    [SerializeField] private float zonesToSpawn;

    [SerializeField] private float minTimer = 3;
    [SerializeField] private float timerInterval = 1.5f;

    [Header("Deletion Settings")]
    [SerializeField] private float skewPercentage = 70;
    [SerializeField] private float percentForRangeIncrease = 10;
    [SerializeField] private float rangeIncreaseNumber = 1;

    private bool spawnedLayout = false;

    private void GenerateLayout()
    {
        List<GameObject> platforms = new List<GameObject>();

        //spawning
        for (int i = 0; i < zonesToSpawn; i++)
        {
            //spawn the zone
            int zoneIndex = Random.Range(0, zones.Length);
            Vector2 spawnPosition = new Vector2(0, (zoneSpaceY + zoneHeight) * i);
            GameObject zoneParent = Instantiate(zones[zoneIndex], spawnPosition, Quaternion.identity);

            //add the zone's platform to the list
            List<Transform> zoneChildren = new List<Transform>();
            
            for (int j = 0; j < zoneParent.transform.childCount; j++)
            {
                zoneChildren.Add(zoneParent.transform.GetChild(j));
            }

            for (int j = 0; j < zoneChildren.Count; j++) 
            {
                platforms.Add(zoneChildren[j].gameObject);
            }
        }

        int numPlatforms = platforms.Count;
        int numForRangeIncrease = Mathf.FloorToInt(numPlatforms * (percentForRangeIncrease / 100));

        //setting timers
        for (int i = 0; i < numPlatforms; i++)
        {
            //get random platform and give it timer
            int randomIndex = SelectPlatformToDelete(numPlatforms, platforms.Count, numForRangeIncrease);
            GameObject platform = platforms[randomIndex];
            platform.GetComponent<DecayingPlatform>().timeUntilDecay = minTimer + timerInterval * i;

            //enable platform and delete from list
            platform.SetActive(true);
            platforms.RemoveAt(randomIndex);
        }
    }

    private void Start()
    {
        GenerateLayout();
    }

    private int SelectPlatformToDelete(int maxNumPlatforms, int currPlatforms, float numForRangeIncrease)
    {
        //float percentPresent = currPlatforms / maxNumPlatforms;
        //float percentMissing = 1 - percentPresent;

        if (currPlatforms <= maxNumPlatforms / 2)
        {
            return Random.Range(0, currPlatforms);
        }

        int numToAdd = (int)((maxNumPlatforms - currPlatforms) / numForRangeIncrease);

        int maxNum = maxNumPlatforms / 2 + numToAdd;
        maxNum = Mathf.Min(maxNum, currPlatforms);
        print(maxNum);
        return Random.Range(0, maxNum);
    }
}
