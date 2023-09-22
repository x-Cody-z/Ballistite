using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField][Tooltip("Name of the file to be player, do not include file extension" )]string videoName;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
    }

    private void Awake()
    {
        videoPlayer.Play();
    }
}
