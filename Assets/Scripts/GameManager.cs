using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class GameManager : MonoBehaviour
{
    public GameObject[] zones;
    public GameObject stageFloor;

    public Transform stageParent1;
    public Transform stageParent2;

    public static event Action OnTimersStart;
    private bool _timersRunning;
    public bool TimersRunning
    {
        get => _timersRunning;
        set
        {
            if (_timersRunning == value) return;
            _timersRunning = value;
            if (_timersRunning == true) OnTimersStart?.Invoke();
        }
    }

    [Header("Generation")]
    [SerializeField] private float zoneSpaceY = 1;
    [SerializeField] private float zoneHeight = 6;
    [SerializeField] private float stagePlatformOffsetAdjustment = -2;
    [SerializeField] private float zonesToSpawn;

    [SerializeField] private float minTimer = 3;
    [SerializeField] private float timerInterval = 1.5f;

    [SerializeField] private float totalYOffset = 0;

    [Header("Deletion")]
    //[SerializeField] private float skewPercentage = 70;
    [SerializeField] private float percentForRangeIncrease = 10;
    [SerializeField] private float rangeIncreaseNumber = 1;

    [Header("Stage")]
    [SerializeField] private int stage = 1;

    [SerializeField] private int extraZonesPerStage = 2;

    [SerializeField] private int stagesForTimerReduction = 2;
    [SerializeField] private float intervalReduction = 0.1f;
    [SerializeField] private float minTimerReduction = 0.2f;

    [SerializeField] private float smallestMinTimer = 0.75f;
    [SerializeField] private float smallestInterval = 0.125f;

    private bool spawnedLayout = false;

    private void GenerateLayout()
    {
        List<GameObject> platforms = new List<GameObject>();
        Transform correctParent = stage % 2 == 0 ? stageParent1 : stageParent2;

        //spawning
        for (int i = 0; i < zonesToSpawn; i++)
        {
            //spawn the zone
            int zoneIndex = UnityEngine.Random.Range(0, zones.Length);
            Vector2 spawnPosition = new Vector2(0, totalYOffset);
            GameObject zoneParent = Instantiate(zones[zoneIndex], spawnPosition, Quaternion.identity, correctParent);
            totalYOffset += zoneHeight + zoneSpaceY;

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

        Instantiate(stageFloor, new Vector2(0, totalYOffset + stagePlatformOffsetAdjustment), Quaternion.identity, correctParent);
        totalYOffset += 6 + zoneSpaceY + stagePlatformOffsetAdjustment;

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
            return UnityEngine.Random.Range(0, currPlatforms);
        }

        int numToAdd = (int)((maxNumPlatforms - currPlatforms) / numForRangeIncrease);

        int maxNum = maxNumPlatforms / 2 + numToAdd;
        maxNum = Mathf.Min(maxNum, currPlatforms);
        return UnityEngine.Random.Range(0, maxNum);
    }

    public void StartStage()
    {
        TimersRunning = true;
    }

    public void EndStage()
    {
        TimersRunning = false;

        //scaling
        zonesToSpawn += extraZonesPerStage;
        if (stage-1 % stagesForTimerReduction == 0)
        {
            minTimer = Mathf.Max(minTimer - minTimerReduction, smallestMinTimer);
            timerInterval = Mathf.Max(timerInterval - intervalReduction, smallestInterval);
        }

        GenerateLayout();
    }
}
