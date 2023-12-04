using System;
using System.Collections;
using DG.Tweening;
using Mediapipe.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static event Action RedLightGreenLigth = delegate { };
    public PoseWorldLandmarkListAnnotationController Pose;
    public AudioSource AudioSource;
    public AudioClip[] AudioClips; // 0 : redlightgreenlight, 1:shoot
    public GameObject Robot;
    public ParticleSystem BloodParticle;
    public GameObject Camera;
    public TextMeshProUGUI timeText;
    private bool gameEnd = false;

    void Awake()
    {
        UnityEngine.Device.Screen.SetResolution(1080, 1920, true); 
    }
    void Start()
    {
        RedLightGreenLigth += Pose.StartRedLight;
        Pose.EndByMove += PlayEnd;
        Pose.GreenLightRedLightFinsh += StartWaitingGreenLight;
        BloodParticle.Stop();
        BloodParticle.Clear();

        StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
    {
        float time = 4f;
        while(time > 1f)
        {
            timeText.text = (int)time + "";
            time -= Time.deltaTime;
            yield return null;
        }
        timeText.gameObject.SetActive(false);
        StartWaitingGreenLight();
    }

    public void StartWaitingGreenLight()
    {
        AudioSource.pitch = 1;
        Robot.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutQuad).Play()
        .OnComplete( () => {
            StartCoroutine(WaitingGreenLightRedLight());
        });
    }
    public IEnumerator WaitingGreenLightRedLight()
    {
        if(!gameEnd)
        {
            float randomTime = UnityEngine.Random.Range(3, 6);
            AudioSource.clip = AudioClips[0];
            float currentPitch = 1f;
            AudioSource.pitch = currentPitch * (6/randomTime);
            AudioSource.Play();
            var wait = new WaitForSeconds(randomTime);
            yield return wait;
            GreenLightRedLight();
        }
        else
        {
            yield return null;
        }
    }
    void GreenLightRedLight()
    {
        Robot.transform.DORotate(new Vector3(0, 180, 0), 0.5f).SetEase(Ease.OutQuad).Play()
        .OnComplete(static () => {
            RedLightGreenLigth();
        });
    }



    void PlayEnd()
    {
        gameEnd = true;
        AudioSource.pitch = 1;
        AudioSource.Stop();
        AudioSource.clip = AudioClips[1];
        AudioSource.Play();
        BloodParticle.Play();

        DOTween.Sequence()
        .Append(Camera.transform.DOMoveY(-30, 1.5f).SetEase(Ease.OutBounce))
        .Append(Camera.transform.DORotate(new Vector3(0, 0, -78), 1.5f).SetEase(Ease.OutBounce))
        .Play()
        .OnComplete( () => {
            StartCoroutine(ReturnLobby());
        });
    }


    public IEnumerator ReturnLobby()
    {
        var time = new WaitForSeconds(4f);
        yield return time;
        SceneManager.LoadScene("StartScene");
    }
}
