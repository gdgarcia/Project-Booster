using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2.0f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        // if isTransitioning, just return, do nothing
        if (isTransitioning) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence(levelLoadDelay);
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            default:
                // We need to ReloadLevel after some delay 
                // ReloadLevel();
                StartCrashSequence(levelLoadDelay);
                break;
        }
    }


    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);

    }

    void StartSuccessSequence(float levelLoadDelay)
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        // Stop all other sound effects linked to the ship
        GetComponent<AudioSource>().Stop();
        audioSource.PlayOneShot(success);
        // TODO: add particle effect upon success
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence(float reloadDelay)
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        // Stop all other sound effects linked to the ship
        GetComponent<AudioSource>().Stop();
        // plays crash audio
        audioSource.PlayOneShot(crash);
        // TODO: add particle effect upon crash
        
        Invoke("ReloadLevel", reloadDelay);
    }
}
