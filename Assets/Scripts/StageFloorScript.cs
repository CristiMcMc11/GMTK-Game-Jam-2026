using System.Collections;
using UnityEngine;

public class StageFloorScript : MonoBehaviour
{
    private GameObject player;

    private BoxCollider2D boxCollider;
    private GameObject child;

    [SerializeField] bool isSolid = false;
    [SerializeField] bool startSolid = false;

    private void Awake()
    {
        player = GameObject.Find("Player");
        boxCollider = GetComponent<BoxCollider2D>();
        child = transform.Find("Collider").gameObject;

        if (startSolid)
        {
            child.SetActive(true);
            isSolid = true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            child.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.StartStage();
        }
    }

    private void Update()
    {
        if (player.transform.position.y > transform.position.y && boxCollider.isTrigger == true && !isSolid && !startSolid)
        {
            StartCoroutine(SetPlatformSolid());
        }
    }

    private IEnumerator SetPlatformSolid()
    {
        isSolid = true;
        player.GetComponent<PlayerMovement>().AddForce(new Vector2(0, 10), true);
        GameManager.instance.EndStage();

        yield return new WaitForSeconds(0.2f);

        child.SetActive(true);
    }
}
