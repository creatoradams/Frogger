using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject frog;

    // Thinking of the homebase as on on/off object, therefore if Frogger jumps to a home that's enabled, frog is active
    // Enabled meaning occupied
    private void OnEnable()
    {
        frog.SetActive(true);
    }

    private void OnDisable()
    {
        frog.SetActive(false);
    }

    // This function tells us that something has collided with this object (entered the trigger zone (home))
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If Frogger collided with the homebase
        if (other.tag == "Player")
        {
            enabled = true;
            FindObjectOfType<GameManager>().HomeOccupied();
        }
    }
}
