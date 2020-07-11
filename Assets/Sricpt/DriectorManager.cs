using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
[RequireComponent(typeof(PlayableDirector))]
public class DriectorManager : IActorManagerInterface {

	public PlayableDirector pd;
	[Header("=== TimelineAssets ===")]
	public TimelineAsset frontStab;
	[Header("=== Assets Setting ===")]
	public ActorManager attacker;
	public ActorManager victim;
	// Use this for initialization
	void Start () {
		pd = GetComponent<PlayableDirector>();
		pd.playOnAwake = false;
		pd.playableAsset = Instantiate(frontStab);
		foreach (var track in pd.playableAsset.outputs)
		{	
			if(track.streamName == "Attack Sricpt")
			{
				pd.SetGenericBinding(track.sourceObject,attacker);
			}
			else if(track.streamName == "Victim Sricpt")
			{
				pd.SetGenericBinding(track.sourceObject,victim);
			}
			else if(track.streamName == "AttackerAnimation")
			{
				pd.SetGenericBinding(track.sourceObject,attacker.ac.anim);
			}
			else if(track.streamName == "VictimAnimation")
			{
				pd.SetGenericBinding(track.sourceObject,victim.ac.anim);
			}
		}   
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)&& gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			pd.Play();
		}
	}
}
