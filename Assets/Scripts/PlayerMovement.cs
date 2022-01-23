using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* This script sets an object to mouse position, 
 * then casts a ray to select an object in 3d space.*/
public class PlayerMovement : MonoBehaviour
{
    public GameObject selected;

    [SerializeField] private GameObject listener;
    private Arduino arduino;
    public FakeArduino fakeArduino;

    public int buttonPressed;

    float collisionTime = 0.0f;

    public int upDistance;
    public int downDistance;
    public int leftDistance;
    public int rightDistance;

    private bool dead = false;
    private bool Dead
    {
        set
        {
            if (!dead && value)
            {
                Debug.Log("YOU DIED");
                dead = true;

                //TODO: restart scene, deduct life etc
                deadEvent.Invoke();
            }
        }
    }
    public UnityEvent deadEvent;
    public UnityEvent winEvent;

    private bool isColliding = false;

    void Start()
    {
        arduino = listener.GetComponent<Arduino>();
        
    }

    void Update()
    {

        Vector3 nextPosition = new Vector3(-17.0f + (arduino.Position.x * 0.05f), -17.0f + (arduino.Position.y * 0.05f), 0);
        transform.position = Vector3.Lerp(transform.position, nextPosition, 0.5f);

        //transform.position = fakeArduino.Position;

        if (isColliding)
        {
            collisionTime += Time.deltaTime;
        }

        if (collisionTime >= 1.0f)
        {
            Dead = true;
        }

        //buttonPressed = 1;

        //Debug.Log(buttonPressed);
        if (!isColliding)
        {
            upDistance = GetDistanceToWall(Vector3.up);
            downDistance = GetDistanceToWall(Vector3.down);
            leftDistance = GetDistanceToWall(Vector3.left);
            rightDistance = GetDistanceToWall(Vector3.right);
        }
        else
        {
            upDistance = 255;
            downDistance = 255;
            leftDistance = 255;
            rightDistance = 255;
        }
        Debug.Log("up: " + upDistance + "; down: " + downDistance);
        
    }

    int GetDistanceToWall(Vector3 _direction)
    {
        //Debug.Log("yes");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 2.0f, LayerMask.GetMask("Obstacle"));

        if (hit.collider == null)
        {
            return 0;
        }

        float distance = Vector3.Distance(transform.position, hit.point) * 128.0f;
        distance = Mathf.Clamp(distance, 0.0f, 255.0f);
        
        return 255 - (int)distance;

        //if (distance <= 255) return 255 - distance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("hit wall");
            isColliding = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hello");
        if (collision.gameObject.CompareTag("Goal"))
        {
            winEvent.Invoke();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("no more hit wall");
            collisionTime = 0.0f;
            isColliding = false;
        }
    }
    
}
