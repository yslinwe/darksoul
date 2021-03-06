using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySuperPlayableBehaviour : PlayableBehaviour
{
    public ActorManager am;
    public float newBehaviourVariable;
    PlayableDirector pd;
    public override void OnPlayableCreate (Playable playable)
    {
        
    }
    public override void OnGraphStart (Playable playable)
    {
        
    }
    public override void OnGraphStop(Playable playable)
    {
     
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Debug.Log("Play");
        am.lockActorController();
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
       am.UnlockActorController();
    }
    public override void PrepareFrame(Playable playable, FrameData info) 
    {
        am.lockActorController();
        //Debug.Log("In Frame");
    }
}
