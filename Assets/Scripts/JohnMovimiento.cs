using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnMovimiento : MonoBehaviour
{
    public float Speed;
    public float JumpForce;

    public GameObject bullet;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    

    void Start()
    {
         Rigidbody2D = GetComponent<Rigidbody2D>();
         Animator = GetComponent<Animator>();

    }

    void Update()
    {
         Horizontal = Input.GetAxisRaw("Horizontal");

         if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
         else if(Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);


        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f)) 
        {
            Grounded = true;
        }
        else Grounded = false;



        if (Input.GetKeyDown(KeyCode.W) && Grounded)
         {
            Jump();
         }

         if (Input.GetKey(KeyCode.Space))
         {
            Shoot();
         }

    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector2.right;
        else direction = Vector2.left;

        GameObject newBullet = Instantiate(bullet, transform.position + direction * 0.1f, Quaternion.identity);
        newBullet.GetComponent<bulletScript>().SetDirection(direction);
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }
    
    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
    }
}   