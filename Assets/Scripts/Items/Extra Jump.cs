using System.Collections;
using UnityEngine;

public class ExtraJump : Item
{

    private PlayerMovement pmScript;

    ExtraJump()
    {
        cooldown = 3;
        isOnCooldown = false;

        maxStageTimer = 10;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnJumpPressed += UseItem;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    protected override void UseItem()
    {
        if (!isEnabled) return;

        if (pmScript.PlayerState == PlayerMovement.PlayerStates.InAir && !isOnCooldown)
        {
            pmScript.Jump();
            StartCoroutine(Cooldown());
        }
    }

}
