using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform target;

    public KeyCode inputKey;

    public float speed = 2f;
    public float angle = 1f;
    private float radius;

    private float dir = 1;

    //Temp logic 
    public float raycastDuration = 0.1f; // Time in seconds for which the raycast is active
    public float intervalDuration = 0.5f; // Time in seconds for which the raycast is inactive
    bool isRaycasting = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RaycastCycle());
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


        if (isRaycasting)
            checkBubble();


    }
    private IEnumerator RaycastCycle()
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
    }
    void checkBubble()
    {

 
        Vector2 targetDir = (target.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin:transform.position, direction:targetDir, distance:radius);

        if (hit.collider)
        {
            if (hit.collider.transform.CompareTag("Bubble")){
                Debug.Log("Bubble hit");
                hit.transform.GetComponent<Rigidbody2D>().AddForce(targetDir * 0.7f, ForceMode2D.Force);
            }
        }

        Debug.Log(targetDir);
        Debug.DrawRay(transform.position, targetDir * radius, Color.red);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            changeDir();
        }
    }
}
