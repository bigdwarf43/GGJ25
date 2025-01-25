using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform target;

    public KeyCode inputKey;

    public float speed = 2f;
    private float angle = 1f;
    private float radius;

    private float dir = 1;

    //Temp logic 
   /* public float raycastDuration = 0.1f; // Time in seconds for which the raycast is active
    public float intervalDuration = 0.5f; // Time in seconds for which the raycast is inactive*/
 /*   bool isRaycasting = true;*/
    private Vector2 boxSize = new Vector2(2f, 2f);
    public float circleRadius = 0.5f; // Width of the CircleCast

    //Bubble force
    private float bubblePushForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*StartCoroutine(RaycastCycle());*/
        bubblePushForce = GameManager.Instance.bubblePushForce;
    
    }

    void changeDir()
    {
        dir = -dir;
    }

    // Update is called once per frame
    void Update()
    {
        radius = target.GetComponent<CircleCenterTarget>().circleRadius;

        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector2(x, y);

        if (dir == 1)
        {
            angle += speed * Time.deltaTime;
        }
        else
        {
            angle -= speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(inputKey))
        {
            changeDir();
        }

        /*checkBubble();*/
     /*   if (isRaycasting)*/
        checkBubble();


    }
   /* private IEnumerator RaycastCycle()
    {
        while (true)
        {
            // Enable raycasting
            isRaycasting = true;
            yield return new WaitForSeconds(raycastDuration);

            // Disable raycasting
            isRaycasting = false;
            yield return new WaitForSeconds(intervalDuration);
        }
    }*/
    void checkBubble()
    {

        Vector2 targetDir = (target.position - transform.position).normalized;

        // Perform the CircleCast
        RaycastHit2D hit = Physics2D.CircleCast(
            origin: transform.position,
            radius: circleRadius,
            direction: targetDir,
            distance: Mathf.Infinity
        );

        if (hit.collider)
        {
            if (hit.collider.transform.CompareTag("Bubble"))
            {
                Debug.Log("Bubble hit");
                Rigidbody2D rb = hit.transform.GetComponent<Rigidbody2D>();

                // Apply force to the bubble
                rb.AddForce(targetDir * bubblePushForce, ForceMode2D.Force);

                float maxVelocity = 3f; // Set the maximum velocity you want
                if (rb.linearVelocity.magnitude > maxVelocity)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
                }
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            changeDir();
        }
    }


    /// FOR DRAWING GIZMOS
    void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.blue;

        // Calculate the direction to the target
        Vector2 targetDir = (target.position - transform.position).normalized;

        // Draw the starting box
        DrawBox(transform.position, boxSize, 0f);

        // Draw the box along the path at intervals for better visualization
        int segments = 10; // Number of segments along the cast path
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments; // Interpolate along the path
            Vector2 interpolatedPosition = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y) + targetDir * radius, t);
            DrawBox(interpolatedPosition, boxSize, 0f); // Adjust the angle if needed
        }


        // Draw the connecting line
        Vector2 endPosition = transform.position + (Vector3)(targetDir * radius);
        Gizmos.DrawLine(transform.position, endPosition);
    }

    private void DrawBox(Vector2 position, Vector2 size, float angle)
    {
        Vector3[] corners = new Vector3[4];
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 halfSize = new Vector3(size.x / 2, size.y / 2, 0);

        // Calculate the corners of the box
        corners[0] = rotation * new Vector3(-halfSize.x, -halfSize.y) + (Vector3)position;
        corners[1] = rotation * new Vector3(halfSize.x, -halfSize.y) + (Vector3)position;
        corners[2] = rotation * new Vector3(halfSize.x, halfSize.y) + (Vector3)position;
        corners[3] = rotation * new Vector3(-halfSize.x, halfSize.y) + (Vector3)position;

        // Draw lines between the corners
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
    }
}

