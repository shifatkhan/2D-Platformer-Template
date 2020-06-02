using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineCameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f; // Time the Camera Shake effect will last
    public float shakeAmplitude = 1.0f; // Cinemachine Noise Profile Parameter
    public float shakeFrequency = 1.0f; // Cinemachine Noise Profile Parameter

    // Cinemachine Shake
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Start()
    {
        if(virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        // Get Virtual Camera Noise Profile
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCo());
    }

    public IEnumerator ShakeCameraCo()
    {
        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;

        yield return new WaitForSeconds(shakeDuration);

        virtualCameraNoise.m_AmplitudeGain = 0f;
    }
}
