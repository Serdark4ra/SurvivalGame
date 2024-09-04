using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{

    public static AudioListenerManager Instance { get; set; }

    public AudioSource dropItemSound;
    public AudioSource chopTreeSound;
    public AudioSource craftItemSound;
    public AudioSource swingAxeSound;
    void Start()
    {
        // Find all audio listeners in the scene
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // If there is more than one audio listener, disable the extra ones
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                audioListeners[i].enabled = false;
                Debug.LogWarning("Disabled extra AudioListener on " + audioListeners[i].gameObject.name);
            }
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    


}