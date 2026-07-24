using UnityEngine;

public class ItemObtainer : MonoBehaviour
{
    public Item itemToGive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemManager.instance.AddItem(itemToGive);
            GetCollected();
        }
    }

    private void GetCollected()
    {
        //neat animation or particles
        Destroy(gameObject);
    }
}
