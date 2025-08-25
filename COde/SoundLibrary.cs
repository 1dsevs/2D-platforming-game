using UnityEngine;
using System;

[Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        if (soundEffects == null) return null;
        
        foreach (SoundEffect soundEffect in soundEffects)
        {
            if (soundEffect.groupID == name)
            {
                if (soundEffect.clips != null && soundEffect.clips.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, soundEffect.clips.Length);
                    return soundEffect.clips[randomIndex];
                }
                return null;
            }
        }
        return null;
    }

    // Optional: Add this for initialization
    private void Start()
    {
        Debug.Log("SoundLibrary initialized successfully!");
    }
}