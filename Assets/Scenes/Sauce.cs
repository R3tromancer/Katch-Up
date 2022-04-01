using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sauce : MonoBehaviour
{
    bool isTransitioning = false;

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool IsCollisionOff = false;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 200f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip succesSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip mainEngineSound;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem engineParticle;
    [SerializeField] ParticleSystem succesParticle;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            ResponseToThrustInput();
            ResponseToRotateInput();
        }
         if(Debug.isDebugBuild)
        {
            ResponseToDebugInput();
        }
    }

    private void ResponseToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            IsCollisionOff = !IsCollisionOff; // Switcher
        }
    }

    private void ResponseToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }

    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        engineParticle.Stop();
    }
    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSound);
            engineParticle.Play();
        }

    }

    private void ResponseToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.angularVelocity = Vector3.zero; //  remove rotation due to physics

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

    }
   
    void OnCollisionEnter (Collision collision)
    {
        if (isTransitioning || IsCollisionOff) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccesSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(succesSound);
        succesParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay); // parametrise this time
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticle.Play();
        Invoke("Respawn", levelLoadDelay); // parametrise this time
    }

    private void Respawn()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    





}
