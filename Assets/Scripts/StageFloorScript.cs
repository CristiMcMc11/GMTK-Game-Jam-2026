using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageFloorScript : MonoBehaviour
{
    private GameObject player;

    private BoxCollider2D boxCollider;
    private Tilemap tilemap;
    private GameObject child;

    [SerializeField] bool isSolid = false;
    [SerializeField] bool startSolid = false;
    [SerializeField] Color nonSolidColor = new Color(255, 163, 100, 155);
    [SerializeField] Color solidColor = new Color(255, 163, 100, 255);

    private void Awake()
    {
        player = GameObject.Find("Player");
        boxCollider = GetComponent<BoxCollider2D>();
        child = transform.Find("Collider").gameObject;
        tilemap = GetComponent<Tilemap>();

        if (startSolid)
        {
            child.SetActive(true);
            isSolid = true;
            tilemap.color = solidColor;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            child.SetActive(false);
            tilemap.color = nonSolidColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !startSolid)
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
        tilemap.color = solidColor;

        yield return new WaitForSeconds(0.2f);

        child.SetActive(true);
    }
}
