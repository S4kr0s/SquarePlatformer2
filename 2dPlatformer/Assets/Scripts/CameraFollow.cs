using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool activateCameraFollow = true;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 threshold;
    [SerializeField] private float speed;
    private Rigidbody2D rb2d;
    private Vector3 newPosition;
    private float moveSpeed;

    private void Start()
    {
        threshold = calculateThreshold();
        rb2d = target.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (activateCameraFollow)
        {
            Vector2 follow = target.transform.position;
            float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
            float yDifference = Vector2.Distance(Vector2.right * transform.position.y, Vector2.up * follow.x);

            newPosition = transform.position;

            if (Mathf.Abs(xDifference) >= threshold.x)
            {
                newPosition.x = follow.x;
            }

            /*
             * Dont want Y in this game
             * 
            if (Mathf.Abs(yDifference) >= threshold.y)
            {
                newPosition.y = follow.y;
            }
            */

            moveSpeed = Mathf.Abs(rb2d.velocity.x) > speed ? Mathf.Abs(rb2d.velocity.x) : speed;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }

    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= offset.x;
        t.y -= offset.y;

        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
