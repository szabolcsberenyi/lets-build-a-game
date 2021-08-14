using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Handles the music of the game
/// </summary>
/// <remarks>
/// Music Manager is responsible for playing songs in random order and setting specific music with smooth transition
/// </remarks>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Music Manager Audio Source")]
    AudioSource audioSource;

    [Range(0.0f, 6.0f)]
    [SerializeField]
    [Tooltip("Fade Duration in seconds")]
    float fadeDuration = 2.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("Maximum volume of the game")]
    float maxVolume;

    //[SerializeField] // Uncoment to check music name list on unity editor
    List<string> musicList;
    List<int> randomMusicOrder;
    int currentMusicIndex;
    bool isManagerCreated;
    bool isBusy;
    AudioClip loadedClip;
    static string musicFolderPath = "Audio/Music/";
    DirectoryInfo directory;

    /// <summary>
    /// On awake is responsible for loading all the available music files and creating the dictionary
    /// </summary>
    private void Awake()
    {
        InitialSetup();
        GetMusicResourcesList();
        ShuffleMusicOrder();
        StartMusic();
    }

    /// <summary>
    /// GetMusicResourcesList is responsible for reading all music resources in directory and storing their names
    /// </summary>
    void GetMusicResourcesList()
    {
        FileInfo[] fileNames = directory.GetFiles("*.*", SearchOption.AllDirectories);                  // Get all file names from folder path
        for (int i = 0; i < fileNames.Length; i++)                                                      // For every file in the resource path
        {
            if (fileNames[i].Extension != ".meta")
            {
                if (fileNames[i].Extension == ".wav" || fileNames[i].Extension == ".mp3")               // Check file extension (if audio)
                {
                    musicList.Add(Path.GetFileNameWithoutExtension(fileNames[i].Name));
                    randomMusicOrder.Add(randomMusicOrder.Count);
                }
            }
        }
    }

    /// <summary>
    /// Initial Setup is responsible for initializing the music list and setting one instance of this manager with DontDestroyOnLoad
    /// </summary>
    void InitialSetup()
    {
        /*var musicManagers = GameObject.FindGameObjectsWithTag("MusicManager");                          // Look for any other music managers
        if (musicManagers.Length > 1)                                                                   // if found any
        {
            if (!isManagerCreated)                                                                      // if not the original
            {
                Destroy(gameObject);                                                                    // destroy
            }
        }*/

        DontDestroyOnLoad(gameObject);                                                                  // past this point, it is the original
        isManagerCreated = true;

        musicList = new List<string>();
        randomMusicOrder = new List<int>();
        directory = new DirectoryInfo("Assets/Resources/" + musicFolderPath);
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource.volume != 0)                                                                    // if audio source starts with volume
        {
            maxVolume = audioSource.volume;                                                             // set it as maximum
        }
        else
        {
            maxVolume = 1f;                                                                             // else, set maximum as 1
        }

        audioSource.volume = 0;                                                                         // start audio on low, to fade in
    }


    /// <summary>
    /// ShuffleMusicOrder is responsible for creating a random order for the music player
    /// </summary>
    void ShuffleMusicOrder()
    {
        if (currentMusicIndex != 0)                                                                    // if playlist already playing
        {
            currentMusicIndex = -1;                                                                    // reset playlist, but don't stop playing
        }
        
        for (int i = 0; i < randomMusicOrder.Count; i++)
        {
            int temp = randomMusicOrder[i];
            int randomIndex = Random.Range(i, randomMusicOrder.Count);
            randomMusicOrder[i] = randomMusicOrder[randomIndex];
            randomMusicOrder[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Temporary function update to control music selection
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayNextSong();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySong(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySong(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaySong(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaySong(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShuffleMusicOrder();
        }
    }

    /// <summary>
    /// Play Next Song function calls play song with the next current music number
    /// </summary>
    void PlayNextSong()
    {
        PlaySong(currentMusicIndex + 1);                                                                            // play the new selected song
    }

    /// <summary>
    /// Play Song is responsible for receiving an string with the song name and calling the function that receives the number with its index.
    /// </summary>
    /// <param name="_name">Name of the song file</param>
    void PlaySong(string _name)
    {
        for (int i = 0; i < randomMusicOrder.Count; i++)                                                            // for every music
        {
            if (musicList[i] == _name)                                                                              // check if it is the right music
            {
                PlaySong(i);                                                                                        // play music and break loop
                break;
            }
        }
    }

    /// <summary>
    /// Play Song is responsible for receiving an int with the song index and playing it.
    /// </summary>
    /// <param name="_index">Index of the song file</param>
    void PlaySong(int _index)
    {
        if (!isBusy)
        {
            isBusy = true;

            currentMusicIndex = _index;                                                                             // increment music number
            if (currentMusicIndex >= randomMusicOrder.Count)                                                        // if over maximum, reset
            {
                currentMusicIndex = 0;
            }

            CancelInvoke("PlayNextSong");                                                                           // cancel any other invokes
            if (audioSource.isPlaying)
            {
                FadeOffMusic(true);                                                                                 // if we have a song playing, stop it first
            }
            else
            {
                StartMusic();
            }
        }
    }

    /// <summary>
    /// Fade is responsible for fading audio source volume to target volume within duration
    /// </summary>
    /// <param name="audioSource">Audio source to manipulate</param>
    /// <param name="duration">Duration of the fade</param>
    /// <param name="targetVolume">Target volume of the fade</param>
    public static IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        if (duration > 0)
        {
            float currentTime = 0;
            float currentVol = audioSource.volume;

            while (currentTime < duration)                                                                          // while duration not over
            {                                                                                                       
                currentTime += Time.deltaTime;                                                                      // increment duration
                float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / duration);                        // lerp volume value
                audioSource.volume = newVol;                                                                        // update audio source volume
                yield return null;                                                                                  // keep yield
            }
        }
        else                                                                                                        // if duration is 0 or lower, just jump to target volume
        {
            audioSource.volume = targetVolume;
        }

        yield break;                                                                                                // finish yield
    }

    /// <summary>
    /// Play Song is responsible for receiving an string with the song name and calling the function that receives the number with its index.
    /// </summary>
    /// <param name="_startNext">If true, starts the next song, if false, does nothing after fading off</param>
    void FadeOffMusic(bool _startNext)
    {   
        StartCoroutine(Fade(audioSource, fadeDuration, 0));                                                         // fade off
        if (_startNext)                                                                                             // if true, start next song
        {
            Invoke("StartMusic", fadeDuration);
        }
    }

    /// <summary>
    /// Start Music is reponsible for loading clip into memory and starting song. It also unloads previous clip from memory;
    /// </summary>
    void StartMusic()
    {
        if (loadedClip != null)                                                                                     // If had a clip previously, unload it
        {
            Resources.UnloadAsset(loadedClip);
        }
        int randomClipIndex = randomMusicOrder[currentMusicIndex];                                                  // get index from random list
        loadedClip = (AudioClip)Resources.Load(musicFolderPath + musicList[randomClipIndex], typeof(AudioClip));    // load next clip into memory
        audioSource.clip = loadedClip;                                                                              // set clip to audio source
        audioSource.Play();                                                                                         // play music (starts muted)
        StartCoroutine(Fade(audioSource, fadeDuration, maxVolume));                                                 // slowly fade in
        Invoke("PlayNextSong", loadedClip.length - fadeDuration);                                                   // Invoke next song when this one finishes
        isBusy = false;
    }
}
