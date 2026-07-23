using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DecayingPlatform : MonoBehaviour
{
    private enum PlatformState
    {
        Solid,
        Warning1,
        Warning2,
        Broken
    }
    private PlatformState state;

    public bool decay = true;
    public float timeUntilDecay = 0;
    [SerializeField] private float timer = 0;

    [SerializeField] float warning1Time = 5;
    [SerializeField] float warning2Time = 1;

    private void Awake()
    {
        if (timeUntilDecay <= warning2Time)
        {
            state = PlatformState.Warning2;
        }
        else if (timeUntilDecay <= warning1Time)
        {
            state = PlatformState.Warning1;
        }
        else
        {
            state = PlatformState.Solid;
        }

        transform.GetComponent<SpriteRenderer>().color = new Color(141, 141, 141);
    }

    private void OnEnable()
    {
        if (!decay) return;
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(DecayTimer());
    }

    private IEnumerator DecayTimer()
    {
        timer = timeUntilDecay;

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

    private void ShowWarning1()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void ShowWarning2()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void PlatformBreak()
    {
        gameObject.SetActive(false);
    }
}
