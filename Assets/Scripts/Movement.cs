using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Reference to our rigid body
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float mainThrust = 1000.0f;
    [SerializeField] float rotationThrust = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            // somente vamos tocar se nao estiver tocando.
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else  // se nao estiver sendo pressionado, devemos parar de tocar
        {
            audioSource.Stop();
        } 
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(-1 * rotationThrust);
        }
    }

    private void ApplyRotation(float rotationThrust)
    {
        rb.freezeRotation = true;  // freeze rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
