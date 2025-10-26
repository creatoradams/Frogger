using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.Rendering.DebugUI;

// Handles grid-based player movement for Frogger, allowing the player to hop 
// in four directions using keyboard input with smooth interpolation between positions.

public class FroggerPlayerMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {
        // transform.position += direction;
        Vector3 destination = transform.position + direction;

        // Returns a collider if it exists (if this barrier exists, we should prevent movement)
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        // Returns a collider if it exists (if this platform exists, we should adapt movement)
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        // Returns a collider if it exisits (if this obstacle exists, we should adapt movement (frogger dies)
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null)
        {
            return;
        }

        // If there is a platform, once frogger makes it to the platform, it needs to move with it
        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            // Detach frogger from platform-aligned movement
            transform.SetParent(null);
        }

        // If we move on to an obstacle, frogger dies
        if (obstacle != null && platform == null)
        {
            // even if the position leads to death, it should be an allowable transition
            transform.position = destination;
            Death();
        }
        else
        {
            StartCoroutine(Leap(destination));
        }

    }
    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
    }

    private void Death()
    {
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;

        // When you die, you shouldn't be able to contorl frogger anymore
        enabled = false;
    }

    // This function gets called when some other collider enters our zone (collision detection)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();
        }
    }
}
    