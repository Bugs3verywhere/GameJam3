using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FanController : MonoBehaviour
{
    public enum Mode { Off, On, Timer, PlayerJump }
    public Mode mode = Mode.Off;

    public Vector2 direction = Vector2.right;
    public float force = 600f;

    // Timer mode
    public float onTime = 2f;
    public float offTime = 2f;

    // Player detection
    public string playerTag = "Player";

    public ParticleSystem windParticles;

    HashSet<Rigidbody2D> bodies = new HashSet<Rigidbody2D>();
    Rigidbody2D playerRb;      // now used ONLY to detect player exists
    bool active = false;

    void Start()
    {
        direction.Normalize();

        // Locate the player anywhere in the scene
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();

        if (mode == Mode.On) SetActive(true);
        if (mode == Mode.Off) SetActive(false);

        if (mode == Mode.Timer)
            StartCoroutine(TimerRoutine());

        if (mode == Mode.PlayerJump)
            SetActive(false);
    }

    void Update()
    {
        // PlayerJump mode: jump toggles fan ON/OFF anywhere
        if (mode == Mode.PlayerJump && playerRb != null)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SetActive(!active); // toggle
            }
        }
    }

    void FixedUpdate()
    {
        if (!active) return;

        foreach (var rb in bodies)
        {
            if (rb != null)
                rb.AddForce(direction * force * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody != null)
            bodies.Add(col.attachedRigidbody);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.attachedRigidbody != null)
            bodies.Remove(col.attachedRigidbody);
    }

    IEnumerator TimerRoutine()
    {
        while (true)
        {
            SetActive(true);
            yield return new WaitForSeconds(onTime);

            SetActive(false);
            yield return new WaitForSeconds(offTime);
        }
    }

    void SetActive(bool state)
    {
        active = state;

        if (windParticles != null)
        {
            if (state && !windParticles.isPlaying) windParticles.Play();
            if (!state && windParticles.isPlaying) windParticles.Stop();
        }
    }
}
