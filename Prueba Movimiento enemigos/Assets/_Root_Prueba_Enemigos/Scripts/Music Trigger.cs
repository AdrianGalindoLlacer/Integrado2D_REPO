using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] int musicToPlay;

    
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }
}
