using System.Collections;
using UnityEngine;

public class Recount : Item
{
    [SerializeField] private float platformRespawnTimer;

    Recount()
    {
        cooldown = 15;
        isOnCooldown = false;

        maxStageTimer = 5;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnABK1Pressed += UseItem;
    }

    protected override void UseItem()
    {
        if (isOnCooldown) return;

        foreach (Transform zoneParent in GameManager.instance.correctParent)
        {
            if (zoneParent.gameObject.CompareTag("Stage Floor")) continue;

            foreach(Transform platform in zoneParent)
            {
                if (!platform.gameObject.activeSelf)
                {
                    platform.gameObject.SetActive(true);

                    DecayingPlatform platScript = platform.GetComponent<DecayingPlatform>();
                    platScript.timeUntilDecay = platformRespawnTimer;
                    platScript.StartTimer();
                }
            }
        }

        StartCoroutine(Cooldown());
    }

}
