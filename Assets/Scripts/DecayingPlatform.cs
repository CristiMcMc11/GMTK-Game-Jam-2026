using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class DecayingPlatform : MonoBehaviour
{
    public enum PlatformState
    {
        Solid,
        Warning1,
        Warning2,
        Broken
    }
    public PlatformState state { get; private set; }

    public event UnityAction PlayerTouchedForRevive; 

    private TilemapCollider2D tilemapCollider;
    private TilemapRenderer tilemapRenderer;

    public bool decay = true;
    public float timeUntilDecay = 0;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool playerInBrokenPlatform;

    [SerializeField] float warning1Time = 5;
    [SerializeField] float warning2Time = 1;

    [SerializeField] bool nextTouchRevive = false;

    private void Awake()
    {
        FindPlatformState();
        transform.GetComponent<Tilemap>().color = new Color(141, 141, 141);

        tilemapCollider = GetComponent<TilemapCollider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnEnable()
    {
        GameManager.OnTimersStart += StartTimer;
    }

    private void OnDisable()
    {
        GameManager.OnTimersStart -= StartTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBrokenPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBrokenPlatform = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (nextTouchRevive)
        {
            PlayerTouchedForRevive?.Invoke();
            Respawn();
        }
    }


    public void StartTimer()
    {
        StartCoroutine(DecayTimer());
    }

    public void Respawn(float respawnTime = -1)
    {
        if (playerInBrokenPlatform) return;

        tilemapCollider.isTrigger = false;
        tilemapRenderer.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Terrain");

        if (respawnTime != -1)
        {
            timeUntilDecay = respawnTime;
            StartTimer();
        }
        else
        {
            state = PlatformState.Solid;
        }
    }

    public void NextTouchRevive(bool turnOn)
    {
        if (turnOn)
        {
            nextTouchRevive = true;
            tilemapCollider.isTrigger = false;
            tilemapRenderer.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Terrain");
        }
        else
        {
            nextTouchRevive = false;
            tilemapCollider.isTrigger = true;
            tilemapRenderer.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    private IEnumerator DecayTimer()
    {
        timer = timeUntilDecay;
        FindPlatformState();

        if (state == PlatformState.Solid)
        {
            yield return new WaitForSeconds(Mathf.Max(0, timer - warning1Time));
            state = PlatformState.Warning1;
            timer = warning1Time;
            ShowWarning1();
        }
        
        if (state == PlatformState.Warning1)
        {
            yield return new WaitForSeconds(Mathf.Max(0, timer - warning2Time));
            state = PlatformState.Warning2;
            timer = warning2Time;
            ShowWarning2();
        }

        if (state == PlatformState.Warning2)
        {
            yield return new WaitForSeconds(timer);
            PlatformBreak();
        }

        yield return new WaitForSeconds(Mathf.Max(0, timeUntilDecay - warning2Time));
    }

    private void FindPlatformState()
    {
        if (timeUntilDecay <= warning2Time)
        {
            state = PlatformState.Warning2;
            ShowWarning2();
        }
        else if (timeUntilDecay <= warning1Time)
        {
            state = PlatformState.Warning1;
            ShowWarning1();
        }
        else
        {
            state = PlatformState.Solid;
        }
    }

    private void ShowWarning1()
    {
        transform.GetComponent<Tilemap>().color = Color.yellow;
    }

    private void ShowWarning2()
    {
        transform.GetComponent<Tilemap>().color = Color.red;
    }

    private void PlatformBreak()
    {
        tilemapRenderer.enabled = false;
        tilemapCollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        state = PlatformState.Broken;
    }
}
