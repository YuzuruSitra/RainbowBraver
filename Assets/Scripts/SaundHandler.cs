using UnityEngine;

// SEとBGMの再生停止
public class SaundHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _seAudioSource;

    void Start()
    {
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SaundHandler");
    }

    // 再生
    public void PlayBgm(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        if(clip == null) return;
        _bgmAudioSource.Play();
    }

    public void PlaySe(AudioClip clip)
    {
        if(clip == null) return;
        _seAudioSource.PlayOneShot(clip);
    }

    // 停止
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    public void StopSE()
    {
        _seAudioSource.Stop();
    }

    // 音量変更
    public void SetnewValueBGM(float newValueBGM)
    {
        _bgmAudioSource.volume = Mathf.Clamp01(newValueBGM);
    }

    public void SetnewValueSE(float newValueSE)
    {
        _seAudioSource.volume = Mathf.Clamp01(newValueSE);
    }
}