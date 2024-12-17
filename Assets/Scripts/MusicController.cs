using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Classe responsável por controlar qualquer tipo de áudio
    private AudioSource audioSource;

    // AudioClip é o arquivo de audio que será executado
    public AudioClip levelMusic;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ao iniciar o MusicController, inicia a música da fase
        PlayMusic(levelMusic);
    }

    public void PlayMusic(AudioClip music)
    {
        // Define o som que irá ser reproduzido

        audioSource.clip = music;

        audioSource.Play();
    }
}
