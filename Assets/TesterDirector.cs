using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TesterDirector : MonoBehaviour {

	public PlayableDirector pb;
	public Animator attacker;
	public Animator victim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H))
		{
			foreach(var track in pb.playableAsset.outputs)
			{
				if(track.streamName == "AttackerAnimation")
				{
					pb.SetGenericBinding(track.sourceObject,attacker);
				}
				else if(track.streamName == "VictimAnimation")
				{
					pb.SetGenericBinding(track.sourceObject,victim);
				}
			}
			restart();
		}
		
	}
	private void restart()
	{
		pb.time = 0;
		pb.Stop();
		pb.Evaluate();
		pb.Play();
	}
}
