using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public CharacterController controller;

    public float moveSpeed = 10;
    public float jumpForce = 20;
    public float gravity = -9.8f;
	
	void Update () {
        Vector3 input = new Vector2(0, 0);
        input.x = Input.GetAxis("Horizontal") * moveSpeed;
        input.z = Input.GetAxis("Vertical") * moveSpeed;

        input = transform.localRotation * input;

        if (!controller.isGrounded) input.y = gravity;

        controller.Move(input * Time.deltaTime);

        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, mouse.x, Space.Self);
	}
}
