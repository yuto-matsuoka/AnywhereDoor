using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SwitchMovie : MonoBehaviour
{
    [Header("Movies")]
    [SerializeField]
    public List<VideoClip> videoClipList;

    //　VideoPlayerコンポーネント
    private VideoPlayer videoPlayer;

    //　videoClip
    private int movieIdx = 0;

    // VR用360°動画の初期設定
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClipList[movieIdx];
        videoPlayer.isLooping = true;
    }

    //VR用360°動画の再生
    public void StartMovie()
    {
        videoPlayer.Play();
    }

    //VR用360°動画の停止
    public void StopMovie()
    {
        videoPlayer.Pause();
    }

    //VR用360°動画の切替
    public void changeMovie()
    {
        GameObject sciptGameObject = GameObject.Find("CameraManager");
        SwitchCamera switchCamera = sciptGameObject.GetComponent<SwitchCamera>();
        if (switchCamera.IsDoorFrontOnly()) {
            videoPlayer.Pause();
            movieIdx = (movieIdx == (videoClipList.Count - 1)) ? 0 : (movieIdx + 1);
            videoPlayer.clip = videoClipList[movieIdx];
            videoPlayer.Play();
        }
    }
}
