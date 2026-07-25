using System.Collections;
using UnityEngine;

public class Recount : Item
{
    [Header("Recount Settings")]
    [SerializeField] private float platformRespawnTimer;

    Recount()
    {
        passive = false;
        cooldown = 15;
        isOnCooldown = false;

        maxStageTimer = 10;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnABK1Pressed += UseItem;
    }

    protected override void UseItem()
    {
        if (isOnCooldown || !isEnabled) return;

        foreach (Transform zoneParent in GameManager.instance.correctParent)
        {
            if (zoneParent.gameObject.CompareTag("Stage Floor")) continue;

            foreach(Transform platform in zoneParent)
            {
                if (platform.gameObject.CompareTag("Platform"))
                {
                    DecayingPlatform platScript = platform.GetComponent<DecayingPlatform>();
                    if (platScript.state != DecayingPlatform.PlatformState.Broken) continue;

                    platScript.Respawn(platformRespawnTimer);
                }
            }
        }

        StartCoroutine(Cooldown());
    }

}
