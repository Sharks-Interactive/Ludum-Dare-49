using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using Chrio.Entities;
using DG.Tweening;

public class MusicManager : SharksBehaviour
{
    public List<AudioClip> WarMusic = new List<AudioClip>();
    public List<AudioClip> PeaceMusic = new List<AudioClip>();

    private List<int> WarPlays = new List<int>();
    private List<int> PeacePlays = new List<int>();

    private AudioSource AudioSource;
    private Tension TensionLevel;

    private int index;
    private int peaceIndex;

    private float timeAtStart;

    private enum Tension
    {
        Peace,
        War
    }

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        base.OnLoad(_gameState, _callback);
        WarMusic = Resources.LoadAll<AudioClip>("Music/War/").ToList();
        PeaceMusic = Resources.LoadAll<AudioClip>("Music/Peace/").ToList();

        AudioSource = GetComponent<AudioSource>();
        StartCoroutine(ChooseMusic());
    }

    public IEnumerator ChooseMusic()
    {
        timeAtStart = Time.time;
        index = Mathf.RoundToInt(Random.Range(0, WarMusic.Count() - 1));
        peaceIndex = Mathf.RoundToInt(Random.Range(0, PeaceMusic.Count() - 1));

        PlayNewSong();

        while (true)
        {
            yield return null;
            Tension _currentTensionLevel;

            int _playerShips = 0;
            foreach (IBaseEntity ent in GlobalState.Game.Entities.WorldEntities.Values)
                if (ent.GetOwnerID() == 0) _playerShips++;
            _currentTensionLevel = (_playerShips > 10 ? Tension.War : Tension.Peace);

            if (_currentTensionLevel != TensionLevel)
            {
                // Fade out old song fade in new song
                AudioSource.DOFade(0.0f, 15.0f);
                yield return new WaitForSeconds(15.0f);
                TensionLevel = _currentTensionLevel;
                PlayNewSong();
                AudioSource.DOFade(0.125f, 15.0f);
                yield return new WaitForSeconds(15.0f);

                index = Mathf.RoundToInt(Random.Range(0, WarMusic.Count() - 1));
                peaceIndex = Mathf.RoundToInt(Random.Range(0, PeaceMusic.Count() - 1));
            }
            if (timeAtStart + (_currentTensionLevel == Tension.War ? WarMusic[index].length : PeaceMusic[peaceIndex].length) < Time.time)
            {
                index = Mathf.RoundToInt(Random.Range(0, WarMusic.Count() - 1));
                peaceIndex = Mathf.RoundToInt(Random.Range(0, PeaceMusic.Count() - 1));
                PlayNewSong();
            }
        }
    }

    private void PlayNewSong()
    {
        timeAtStart = Time.time;
        AudioSource.Stop();
        if (TensionLevel == Tension.War)
        {
            AudioSource.PlayOneShot(WarMusic[index]);
            AudioSource.clip = WarMusic[index];
        }
        else
        {
            AudioSource.PlayOneShot(PeaceMusic[peaceIndex]);
            AudioSource.clip = PeaceMusic[peaceIndex];
        }
    }
}
