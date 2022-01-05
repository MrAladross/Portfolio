using UnityEngine;

public class PlaySoundOnMove : MonoBehaviour
{
    public AudioSource source;
    public static PlaySoundOnMove psom;

    void Start()
    {
        source = GetComponent<AudioSource>();
        psom = this;
    }


    public void PlayMoveSound()
    {
        source.Play();
    }
}
