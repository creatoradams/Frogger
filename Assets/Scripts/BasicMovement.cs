using UnityEngine;

public class FroggerPlayerMovement : MonoBehaviour
{
    // [SerializeField] is a special tag that tells Unity to show a private variable in the inspector.
    // keeps variables protected and prevents other scripts from accidentally changing values they shouldnt.
    // ofr moveDistance and moveSpeed it allows them to be tweaked inside the Inspector but protected in code.
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 1f; // How far the frog moves per hop
    [SerializeField] private float moveSpeed = 5f; // How fast the frog moves

    // Vector3 is a Struct that stores three numbers (X, Y, Z coordinates).
    private bool isMoving = false; // Prevents input while moving
    private Vector3 targetPosition; // Where the frog is moving to

    void Start()
    {
        // Set initial target position to current position
        targetPosition = transform.position;
    }

    void Update()
    {
        // Only accept input if not currently moving
        if (!isMoving)
        {
            HandleInput();
        }

        // Move towards target position
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    void HandleInput()
    {
        // Check for arrow key or WASD input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartMove(Vector3.forward); // determines which direction to hop
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartMove(Vector3.back); // determines which direction to hop
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartMove(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartMove(Vector3.right);
        }
    }

    void StartMove(Vector3 direction)
    {
        // Calculate new target position
        targetPosition = transform.position + (direction * moveDistance);
        isMoving = true;
    }

    void MoveToTarget()
    {
        // Smoothly move towards target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            transform.position = targetPosition; // Snap to exact position
            isMoving = false; // Allow new input
        }
    }
}