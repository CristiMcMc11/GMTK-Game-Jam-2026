using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected float cooldown = 10;
    protected bool isOnCooldown = false;

    protected float maxStageTimer = 5;
    protected float currStageTimer = 0;

    public bool passive { get; private set; } = true;
    public bool isEnabled = false;

    protected abstract void UseItem();

    protected void OnEndOfTime()
    {
        ItemManager.instance.RemoveItem(this);
    }


    protected IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
