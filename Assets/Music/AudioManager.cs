using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musiceScore;

    public AudioClip background;

    public void Start()
    {
        musiceScore.clip = background;
        musiceScore.Play();
    }
}
