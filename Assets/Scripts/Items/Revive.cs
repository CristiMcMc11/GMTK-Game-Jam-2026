using System.Collections;
using UnityEngine;

public class Revive : Item
{
    [SerializeField] bool reviveActive = false;

    Revive()
    {
        passive = false;
        cooldown = 15;
        isOnCooldown = false;

        maxStageTimer = 10;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnABK3Pressed += UseItem;
    }

    protected override void UseItem()
    {
        if (isOnCooldown || !isEnabled || reviveActive) return;

        foreach (Transform zoneParent in GameManager.instance.correctParent)
        {
            if (zoneParent.gameObject.CompareTag("Stage Floor")) continue;

            foreach(Transform platform in zoneParent)
            {
                if (platform.gameObject.CompareTag("Platform"))
                {
                    DecayingPlatform platScript = platform.GetComponent<DecayingPlatform>();
                    if (platScript.state != DecayingPlatform.PlatformState.Broken) continue;

                    platScript.NextTouchRevive(true);
                    platScript.PlayerTouchedForRevive += OnRevive;
                }
            }
        }
        StartCoroutine(Cooldown());
    }

    private void OnRevive()
    {
        foreach (Transform zoneParent in GameManager.instance.correctParent)
        {
            if (zoneParent.gameObject.CompareTag("Stage Floor")) continue;

            foreach (Transform platform in zoneParent)
            {
                if (platform.gameObject.CompareTag("Platform"))
                {
                    DecayingPlatform platScript = platform.GetComponent<DecayingPlatform>();
                    if (platScript.state != DecayingPlatform.PlatformState.Broken) continue;

                    platScript.NextTouchRevive(false);
                    platScript.PlayerTouchedForRevive -= OnRevive;
                }
            }
        }
    }

}
