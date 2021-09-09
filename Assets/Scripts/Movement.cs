using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float mainThrust = 1000.0f;
    [SerializeField] float rotationThrust = 100.0f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem boosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    // CACHE
    // Reference to our rigid body
    Rigidbody rb;
    AudioSource audioSource;

    // STATE

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
            StartThrusting();
        }
        else  // se nao estiver sendo pressionado, devemos parar de tocar
        {
            StopThrusting();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        boosterParticles.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        // somente vamos tocar se nao estiver tocando.
        if (!audioSource.isPlaying) audioSource.PlayOneShot(mainEngine);
        if (!boosterParticles.isPlaying) boosterParticles.Play();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotatingLeft();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotatingRight();
        }
        else
        {
            NotRotating();
        }
    }

    void NotRotating()
    {
        if (leftBoosterParticles.isPlaying) leftBoosterParticles.Stop();
        if (rightBoosterParticles.isPlaying) rightBoosterParticles.Stop();
    }

    void RotatingRight()
    {
        ApplyRotation(-1 * rotationThrust);
        if (!leftBoosterParticles.isPlaying) leftBoosterParticles.Play();
    }

    void RotatingLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightBoosterParticles.isPlaying) rightBoosterParticles.Play();
    }

    private void ApplyRotation(float rotationThrust)
    {
        rb.freezeRotation = true;  // freeze rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
