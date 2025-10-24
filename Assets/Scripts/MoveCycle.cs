using UnityEngine;


public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;
    public float speed = 1f;
    public int size = 1;

    private Vector3 leftedge;
    private Vector3 rightedge; 

    private void Start()
    {
        leftedge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightedge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        // Check if the object is past the right edge of the screen
        if (direction.x > 0 && (transform.position.x - size) > rightedge.x) 
        {

            Vector3 position = transform.position;
            position.x = leftedge.x - size;
            transform.position = position;
        
       }
        // Check if the object is past the left edge of the screen
        else if (direction.x < 0 && (transform.position.x + size) < leftedge.x)
        {

            Vector3 position = transform.position;
            position.x = rightedge.x + size;
            transform.position = position;

        }
        else
        {
            // Based on the frame rate of our game, it doesn't vary
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
