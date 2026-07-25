using System.Collections;
using UnityEngine;

public class Sprint : Item
{
    private PlayerMovement pmScript;

    [SerializeField] private float defaultSpeed;
    [SerializeField] private float boostedSpeed = 12;

    Sprint()
    {
        cooldown = 0;
        isOnCooldown = false;

        maxStageTimer = 8;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnSprintStart += StartSprint;
        ItemManager.OnSprintStop += StopSprint;

        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        defaultSpeed = pmScript.walkSpeed;
    }

    protected override void UseItem()
    {
        return;
    }

    private void StartSprint()
    {
        pmScript.walkSpeed = boostedSpeed;
    }

    private void StopSprint()
    {
        pmScript.walkSpeed = defaultSpeed;
    }


}
