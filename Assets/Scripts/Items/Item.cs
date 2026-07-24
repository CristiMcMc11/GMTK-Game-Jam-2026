using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected float cooldown = 10;
    [SerializeField]  protected bool isOnCooldown = false;

    [SerializeField] protected float maxStageTimer = 5;
    [SerializeField] protected float currStageTimer = 0;

    public bool passive { get; protected set; } = true;
    public bool isEnabled = false;

    protected void Awake()
    {
        
    }

    protected abstract void UseItem();

    protected void OnStageIncrease()
    {
        currStageTimer--;

        if (currStageTimer <= 0) ItemManager.instance.RemoveItem(this);
    }

    protected IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    public void EnableItem()
    {
        currStageTimer = maxStageTimer;
        GameManager.NextStage += OnStageIncrease;
    }
}
