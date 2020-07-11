using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySuperPlayableBehaviour : PlayableBehaviour
{
    public ActorManager newExposedReference;
    public float newBehaviourVariable;
    PlayableDirector pd;
    public override void OnPlayableCreate (Playable playable)
    {
        
    }
    public override void OnGraphStart (Playable playable)
    {
        Debug.Log("Start");
        pd = (PlayableDirector)playable.GetGraph().GetResolver();
        foreach (var track in pd.playableAsset.outputs)
        {
            if(track.streamName == "Attack Sricpt" || track.streamName == "Victim Sricpt")
            {
               ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
               am.lockActorController();
            }
        }
    }
    public override void OnGraphStop(Playable playable)
    {
        Debug.Log("Stop");
        foreach (var track in pd.playableAsset.outputs)
        {
            if(track.streamName == "Attack Sricpt" || track.streamName == "Victim Sricpt")
            {
               ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
               am.UnlockActorController();
            }
        }
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Debug.Log("Play");
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //Debug.Log("Pause");
    }
    public override void PrepareFrame(Playable playable, FrameData info) 
    {
        //Debug.Log("In Frame");
    }
}
