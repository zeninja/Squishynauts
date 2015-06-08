using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	AudioManager instance;

	public AudioClip arthurGrunt;



	private static AudioManager _instance;
	
	public static AudioManager GetInstance ()
	{
		if(_instance == null)
		{
			_instance = GameObject.FindObjectOfType<AudioManager>();
		}
		
		return _instance;
	}

	void Awake() {
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(AudioClip clip) {
		GetComponent<AudioSource>().PlayOneShot(clip);
	}
}
