using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophoneClip : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private AudioLoudnessDetection detector;
    
    [SerializeField] private float loudnessSensitivity = 100;
    [SerializeField] private float threshold = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone();

        if (loudness < threshold)
            loudness = 0;
        
        //transform.localScale = transform.Lerp(minScale, maxScale, loudness);

    }
}
