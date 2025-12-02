using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    
    public AudioSource musicSource;
    public AudioSource sfxSource; 

    [System.Serializable]
    public class SoundData
    {
        public string name;     
        public AudioClip clip;   
        [Range(0f, 1f)] public float volume = 1f; 
    }

    [Header("Sound Library")]
    public SoundData[] sounds;

    void Awake()
    {
        
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
    


    public void PlaySFX(string soundName)
    {
        SoundData s = System.Array.Find(sounds, item => item.name == soundName);
        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning("หาเสียงชื่อ " + soundName + " ไม่เจอ!");
        }
    }

   
    public void PlayBGM(string soundName)
    {
        SoundData s = System.Array.Find(sounds, item => item.name == soundName);
        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.Play();
        }
    }

    
    public void StopMusic()
    {
        musicSource.Stop();
    }
}