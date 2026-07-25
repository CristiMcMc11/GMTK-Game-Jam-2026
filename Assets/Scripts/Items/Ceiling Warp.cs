using System.Collections;
using UnityEngine;

public class CeilingWarp : Item
{
    private Rigidbody2D playerRb;
    private PlayerMovement pmScript;

    [Header("Ceiling Warp Settings")]
    [SerializeField] private float maxWarpDistance = 10;
    [SerializeField] private float travelSpeed = 15;
    [SerializeField] private float pointYOffset = 0.5f;

    CeilingWarp()
    {
        cooldown = 10;
        isOnCooldown = false;

        maxStageTimer = 10;
        currStageTimer = 0;
    }

    private void Awake()
    {
        ItemManager.OnABK2Pressed += UseItem;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        pmScript = playerRb.GetComponent<PlayerMovement>();
    }

    protected override void UseItem()
    {
        if (!isEnabled) return;

        RaycastHit2D[] hits = playerRb.RaycastAll(new Vector2(0, maxWarpDistance), Vector2.down, maxWarpDistance, pmScript.playerRaycastLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.point.y > pmScript.transform.position.y && hit.point.y != pmScript.transform.position.y + maxWarpDistance)
            {
                StartCoroutine(HitSuccess(hit.point));
                return;
            }
        }

        HitFailed();
    }

    private void HitFailed()
    {
        //nothing yet
    }

    private IEnumerator HitSuccess(Vector2 hitPoint)
    {
        float yPos = hitPoint.y + pointYOffset;
        pmScript.PlayerState = PlayerMovement.PlayerStates.NoMovement;
        pmScript.GetComponent<BoxCollider2D>().enabled = false;

        while (pmScript.transform.position.y < yPos)
        {
            pmScript.transform.Translate(0, travelSpeed * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        pmScript.transform.position = new Vector2(pmScript.transform.position.x, yPos);

        pmScript.PlayerState = PlayerMovement.PlayerStates.InAir;
        pmScript.GetComponent<BoxCollider2D>().enabled = true;

        StartCoroutine(Cooldown());
    }
}
