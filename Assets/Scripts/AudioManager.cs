using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager instance;
	public AudioSource musicSource, sfxSource;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static void PlaySound(AudioClip clip)
	{
		if (instance == null) return;
		instance.sfxSource.PlayOneShot(clip);
	}
}
