using UnityEngine;

public class SphereDrag : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;
    public bool effacer = true;


    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        MoveSphereWithKeys();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            effacer = !effacer;
        }
    }

    void MoveSphereWithKeys()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move += Vector3.down;
        }
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
