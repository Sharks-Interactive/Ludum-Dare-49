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

    public List<int> WarPlays = new List<int>();
    public List<int> PeacePlays = new List<int>();

    private AudioSource AudioSource;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        WarMusic = Resources.LoadAll<AudioClip>("Music/War/").ToList();
        PeaceMusic = Resources.LoadAll<AudioClip>("Music/Peace/").ToList();

        AudioSource = GetComponent<AudioSource>();
        StartCoroutine(ChooseMusic());
        base.OnLoad(_gameState, _callback);
    }

    public IEnumerator ChooseMusic()
    {
        int index = Mathf.RoundToInt(Random.Range(0, WarMusic.Count() - 1));

        AudioSource.PlayOneShot(WarMusic[index]);
        yield return new WaitForSeconds(WarMusic[index].length);

        StartCoroutine(ChooseMusic());
    }
}
