using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 0.0f;
    public float move_input;

    public float jumpForce = 5.0f;
    private int jumpCount = 2; // Menghitung jumlah lompatan
    private int maxJumps = 2; // Batas maksimal lompatan

    // Batas pergerakan pada sumbu X
    private float minX = -8.5f;
    private float maxX = 8.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gerakan horizontal
        move_input = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * Mathf.Abs(move_input));

        // Rotasi sederhana
        if (move_input > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0); // Menghadap ke depan
        }
        else if (move_input < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0); // Membalikkan badan
        }

        // Membatasi pergerakan dalam boundary pada sumbu X
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Lompat dan double jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < maxJumps)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z); // Reset kecepatan vertikal
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // Reset lompatan saat menyentuh tanah atau pilar
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Pillar"))
        {
            jumpCount = 0;
        }

        // Menangani tabrakan dengan musuh
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Menghancurkan player ketika bertabrakan dengan musuh
            Destroy(gameObject);
        }

        // Menangani tabrakan dengan obstacle
        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Menghancurkan player ketika bertabrakan dengan obstacle
            Destroy(gameObject);
        }
    }
}