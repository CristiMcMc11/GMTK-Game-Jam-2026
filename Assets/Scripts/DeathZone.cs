using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public static DeathZone instance;

    private Transform player;

    [Header("Settables")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float distanceFromPlayerForStart = 15;

    [Header("Runtime")]
    [SerializeField] private float zoneSizeY;
    [SerializeField] private bool moving = false;
    [SerializeField] private bool catchingUp;

    public float endStageYPos;
    public float newEndStageYPos;
    public float endStageYPosOffset = 13.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        player = GameObject.Find("Player").transform;

        GameManager.NextStage += StartCatchUp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            GameManager.instance.Die();
            moving = false;
        }
    }

    private void Update()
    {
        if (player.position.y - (transform.position.y + zoneSizeY/2) > distanceFromPlayerForStart && !moving)
        {
            moving = true;
        }
        else if (transform.position.y + zoneSizeY / 2 >= endStageYPos && moving)
        {
            moving = false;
        }

        if (moving) transform.Translate(0, moveSpeed * Time.deltaTime, 0);
    }

    private void StartCatchUp()
    {
        StartCoroutine(CatchUp());
    }

    public IEnumerator CatchUp()
    {
        endStageYPos = GameManager.instance.totalYOffset - endStageYPosOffset;
        catchingUp = true;

        while (transform.position.y + zoneSizeY / 2 < endStageYPos) 
        {
            transform.Translate(0, 60 * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.02f);
        }

        catchingUp = false;
        endStageYPos = newEndStageYPos;
    }
}
