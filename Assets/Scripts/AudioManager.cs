using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDatabaseSO audioDB;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string soundName, AudioSource sfxSource)
    {
        var data = audioDB.Get(soundName);
        if (data == null)
            return;

        var clip = data.GetRandomClip();
        if (clip == null)
            return;

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(.95f, 1.1f);
        sfxSource.volume = data.volume;
        sfxSource.PlayOneShot(clip);
    }
}
