using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Declaracion de Singleton
    public static AudioManager Instance;

    [Header("Audio Source References")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips Arrays")]

    public AudioClip[] musicList;
    public AudioClip[] sfxlist;
    private void Awake()
    {
        //Singleton que no se destruye entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int musicIndex)
    {
        musicSource.clip = musicList[musicIndex];
        musicSource.Play();
    }

    public void PlaySFX(int sfxIndex)
    {
        sfxSource.PlayOneShot(sfxlist[sfxIndex]);
        
    }
}
