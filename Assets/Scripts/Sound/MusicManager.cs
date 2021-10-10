using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using Chrio.Entities;

public class MusicManager : SharksBehaviour
{
    public List<AudioClip> WarMusic = new List<AudioClip>();
    public List<AudioClip> PeaceMusic = new List<AudioClip>();

    private List<int> WarPlays = new List<int>();
    private List<int> PeacePlays = new List<int>();

    private AudioSource AudioSource;
    private Tension TensionLevel;

    private enum Tension
    {
         Peace,
         War
     }

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        WarMusic = Resources.LoadAllAsync<AudioClip>("Music/War/").ToList();
        PeaceMusic = Resources.LoadAllAsync<AudioClip>("Music/Peace/").ToList();

        AudioSource = GetComponent<AudioSource>();
        StartCoroutine(ChooseMusic());
        base.OnLoad(_gameState, _callback);
    }

    public IEnumerator ChooseMusic()
    {
        int index = Mathf.RoundToInt(Random.Range(0, WarMusic.Count() - 1));
        int peaceIndex = Mathf.RoundToInt(Random.Range(0, PeaceMusic.Count() - 1);

        AudioSource.PlayOneShot(WarMusic[index]);

        while (true)
        {
            yield return null;
            Tension _currentTensionLevel;

            _currentTensionLevel = GlobalState.Game.Entities.Selected > 20;
            
           if (_currentTensionLevel != TensionLevel)
           {
               // Fade out old song fade in new song
               AudioSource.volume.DOFade(1.0f, 0.0f, 15.0f);
               yield return new WaitForSeconds(15.0f);
               PlayNewSong();
               AudioSource.volume.DOFade(0.0f, 1.0f, 15.0f);
               yield return new WaitForSeconds(15.0f);

               TensionLevel = _currentTensionLevel;
               // Randomize index
           }
           // Handle if Time.time + songs<Tension>[index].length < Time.time aka song ended randomize index and play new song
        }
    }  

    private void PlayNewSong()
    {
       if (TensionLevel == Tension.War)
          AudioSource.PlayOneShot(WarMusic[index]);
       else
          AudioSource.PlayOneShot(PeaceMusic[peaceIndex]);
    }
}
