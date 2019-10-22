using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Ctrl ctrl;

    private AudioSource audioSource;

    public AudioClip dropClip;
    public AudioClip gameOverClip;
    public AudioClip lineClearClip;
    public AudioClip balloonClip;
    public AudioClip cursorClip;

    private void Awake()
    {
        ctrl = GetComponent<Ctrl>();
        audioSource = transform.GetComponentInChildren<AudioSource>();
    }
    private void PlayAudio(AudioClip clip)
    {
        if (ctrl.Model.GetMuteSet()) return;
        audioSource.PlayOneShot(clip);
    }

    public void PlayDrop()
    {
        PlayAudio(dropClip);
    }
    public void PlayGameOver()
    {
        PlayAudio(gameOverClip);
    }
    public void PlayLineClear()
    {
        PlayAudio(lineClearClip);
    }
    public void PlayBalloon()
    {
        PlayAudio(balloonClip);
    }
    public void PlayCursor()
    {
        PlayAudio(cursorClip);
    }
}
