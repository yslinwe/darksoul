using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
[RequireComponent(typeof(PlayableDirector))]
//todo attacker victim postion  camera postion 
public class DriectorManager : IActorManagerInterface {

	public PlayableDirector pd;
	[Header("=== TimelineAssets ===")]
	public TimelineAsset frontStab;
	public TimelineAsset openBox;
	public TimelineAsset LeverUp;
	//[Header("=== Assets Setting ===")]
	// public ActorManager attacker;
	// public ActorManager victim;
	// Use this for initialization
	void Start () {
		pd = GetComponent<PlayableDirector>();
		pd.playOnAwake = false;
	}
	
	// Update is called once per frame
	// void Update () {
	// 	if(Input.GetKeyDown(KeyCode.H)&& gameObject.layer == LayerMask.NameToLayer("Player"))
	// 	{
	// 		pd.Play();
	// 	}
	// }
	public bool isPlayTimeline()
	{
		if(pd.state == PlayState.Playing)
			return true;
		else
			return false;
	}
	public void playFrontStab(string timelineName,ActorManager attacker,ActorManager victim)
	{
		if(timelineName == "frontStab")
		{
			pd.playableAsset = Instantiate(frontStab); 
			TimelineAsset timeLines = (TimelineAsset)pd.playableAsset;
			foreach (var track in timeLines.GetOutputTracks())
			{
				if(track.name == "Attack Sricpt")
				{
					//设置演员
					pd.SetGenericBinding(track,attacker);
					foreach (var clip in track.GetClips())
					{
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,attacker);
					}					
				}
				else if(track.name == "Victim Sricpt")
				{
					//设置演员
					pd.SetGenericBinding(track,victim);
					foreach (var clip in track.GetClips())
					{
						//注意asset 只能使用资源 不能使用场景物件
						//不能使用 
						//MySuperPlayableBehaviour myBehav = myClip.template;
						//myBehav.am = victim;
						//需要借助ExposedReference类型将场景物件放入asset中
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,victim);
					}					
				}
				else if(track.name == "AttackerAnimation")
				{
					pd.SetGenericBinding(track,attacker.ac.anim);
				}
				else if(track.name == "VictimAnimation")
				{
					pd.SetGenericBinding(track,victim.ac.anim);
				}
			}
			pd.Evaluate();
			pd.Play();
		}
		else if(timelineName == "openBox")
		{
			pd.playableAsset = Instantiate(openBox); 
			TimelineAsset timeLines = (TimelineAsset)pd.playableAsset;
			foreach (var track in timeLines.GetOutputTracks())
			{
				if(track.name == "Player Sricpt")
				{
					pd.SetGenericBinding(track,attacker);
					foreach (var clip in track.GetClips())
					{
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,attacker);
					}					
				}
				else if(track.name == "Box Sricpt")
				{
					pd.SetGenericBinding(track,victim);
					foreach (var clip in track.GetClips())
					{
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,victim);
					}					
				}
				else if(track.name == "PlayerAnimation")
				{
					pd.SetGenericBinding(track,attacker.ac.anim);
				}
				else if(track.name == "BoxAnimation")
				{
					pd.SetGenericBinding(track,victim.ac.anim);
				}
			}
			pd.Evaluate();
			pd.Play();
		}
		else if(timelineName == "LeverUp")
		{
			pd.playableAsset = Instantiate(LeverUp); 
			TimelineAsset timeLines = (TimelineAsset)pd.playableAsset;
			foreach (var track in timeLines.GetOutputTracks())
			{
				if(track.name == "Player Sricpt")
				{
					pd.SetGenericBinding(track,attacker);
					foreach (var clip in track.GetClips())
					{
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,attacker);
					}					
				}
				else if(track.name == "Lever Sricpt")
				{
					pd.SetGenericBinding(track,victim);
					foreach (var clip in track.GetClips())
					{
						MySuperPlayableClip myClip =  clip.asset as MySuperPlayableClip;
						myClip.am.exposedName = System.Guid.NewGuid().ToString();
						pd.SetReferenceValue(myClip.am.exposedName,victim);
					}					
				}
				else if(track.name == "PlayerAnimation")
				{
					pd.SetGenericBinding(track,attacker.ac.anim);
				}
				else if(track.name == "LeverAnimation")
				{
					pd.SetGenericBinding(track,victim.ac.anim);
				}
			}
			pd.Evaluate();
			pd.Play();
		}
	}
}
