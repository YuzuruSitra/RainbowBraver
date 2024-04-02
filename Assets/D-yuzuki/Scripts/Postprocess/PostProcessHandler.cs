using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessHandler : MonoBehaviour
{
    [System.Serializable]
    private struct VolumeState
    {
        public CamState state;
        public VolumeProfile profile;
    }

    private Volume _volume;
    [SerializeField]
    private VolumeState[] _volumeStates;

    private void Start()
    {
#if UNITY_EDITOR
        CheckCameraStatePair();
#endif
        _volume = GetComponent<Volume>();
    }

    public void ChangeVolume(CamState camState)
    {
        VolumeProfile targetProfile = GetProfileForState(camState);
        if (targetProfile != null)
            _volume.sharedProfile = targetProfile;
        else
            Debug.LogWarning("No volume profile found for state: " + camState);
    }

    private VolumeProfile GetProfileForState(CamState camState)
    {
        foreach (VolumeState state in _volumeStates)
            if (state.state == camState) return state.profile;
        return null;
    }

    // 設定ミスをチェック
    private void CheckCameraStatePair()
    {
        HashSet<CamState> uniqueStates = new HashSet<CamState>();
        foreach (VolumeState pair in _volumeStates)
            if (!uniqueStates.Add(pair.state))
                Debug.LogError("Duplicate CamState detected: " + pair.state.ToString());
    }

}
