using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("VALUABLES")]
    bool oyun_aksinmi;
    bool panelacikmi;
    int ballindex;
    int ballFxindex;
    [SerializeField] float ballpower;
    int baslangictopsayisi;
    int mevcutTopSayisi;
    [SerializeField] int girenTopSayisi;
    [SerializeField] int hedefTopSayisi;
    float kovabaslangicdeger;
    float kova_adimdeger;
    [SerializeField] Animator topataranim;

    [Header("OBJECTS")]
    [SerializeField] GameObject[] ballpooling;
    [SerializeField] GameObject firepoint;
    [SerializeField] MeshRenderer[] kovaseffaf;

    [Header("CANVAS SETTINGS")]
    [SerializeField] TextMeshProUGUI lvltxt;
    [SerializeField] TextMeshProUGUI mevcutTopSayisi_txt;
    [SerializeField] TextMeshProUGUI hedeftopsayisitxt;
    [SerializeField] GameObject pausepnl;
    [SerializeField] GameObject winpnl;
    [SerializeField] GameObject gopnl;
    [SerializeField] TextMeshProUGUI winlvltxt;
    [SerializeField] TextMeshProUGUI golvltxt;
    [SerializeField] Button[] buttons;
    [SerializeField] Sprite[] sprites;

    [Header("SOUNDS & EFFECTS")]
    [SerializeField] AudioSource[] sounds; //bg, kova, topatar, win, go;
    [SerializeField] ParticleSystem topatarFx;
    [SerializeField] ParticleSystem[] ballFx;

    private void Awake()
    {
        SahneIlkIslemler();
    }
    void Start()
    {
        baslangictopsayisi = ballpooling.Length;
        mevcutTopSayisi = ballpooling.Length;
        mevcutTopSayisi_txt.text = mevcutTopSayisi.ToString();

        kovabaslangicdeger = 0.5f;
        kova_adimdeger = 0.25f / hedefTopSayisi;

        string Levelad = SceneManager.GetActiveScene().name;
        lvltxt.text = Levelad;
        hedeftopsayisitxt.text = hedefTopSayisi.ToString();
    }
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (oyun_aksinmi)
            {
                Shoot();
            }
        }
    }
    void SahneIlkIslemler()
    {
        if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 1)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].mute = false;
            }
            buttons[0].image.sprite = sprites[0];
            buttons[1].image.sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 0)
        {
            sounds[0].mute = true;
            sounds[1].mute = false;
            sounds[2].mute = false;
            sounds[3].mute = false;
            sounds[4].mute = false;

            buttons[0].image.sprite = sprites[0];
            buttons[1].image.sprite = sprites[3];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 1)
        {
            sounds[0].mute = false;
            sounds[1].mute = true;
            sounds[2].mute = true;
            sounds[3].mute = true;
            sounds[4].mute = true;

            buttons[0].image.sprite = sprites[1];
            buttons[1].image.sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 0)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].mute = true;
            }
            buttons[0].image.sprite = sprites[1];
            buttons[1].image.sprite = sprites[3];
        }
    }
    public void Shoot()
    {
        if (panelacikmi)
        {
            oyun_aksinmi = true;
        }
        oyun_aksinmi = true;
        mevcutTopSayisi--;

        mevcutTopSayisi_txt.text = mevcutTopSayisi.ToString();

        ballpooling[ballindex].transform.SetLocalPositionAndRotation(firepoint.transform.position, firepoint.transform.rotation);
        ballpooling[ballindex].SetActive(true);
        ballpooling[ballindex].GetComponent<Rigidbody>().AddForce(ballpooling[ballindex].transform.TransformDirection(90, 90, 0) * ballpower, ForceMode.Force);

        topataranim.Play("topatar");
        sounds[1].Play();
        topatarFx.gameObject.SetActive(true);
        topatarFx.Play();
        oyun_aksinmi = false;

        if (ballindex != ballpooling.Length - 1)
        {
            ballindex++;
        }
        else
        {
            ballindex = 0;
        }
        if (girenTopSayisi == 0 & mevcutTopSayisi < hedefTopSayisi)
        {
            Go();
        }
        if (mevcutTopSayisi == 0 & girenTopSayisi < hedefTopSayisi)
        {
            Go();
        }
    }
    public void BallEffect(Vector3 pzsn, Color clr)
    {
        ballFx[ballFxindex].transform.position = pzsn;
        ballFx[ballFxindex].gameObject.SetActive(true);
        ballFx[ballFxindex].Play();

        var main = clr;
        ballFx[ballFxindex].startColor = clr;

        if (ballFxindex != ballpooling.Length - 1)
        {
            ballFxindex++;
        }
        else
        {
            ballFxindex = 0;
        }
    }
    public void TopGirdi()
    {
        girenTopSayisi++;

        kovabaslangicdeger -= kova_adimdeger;
        for (int i = 0; i < kovaseffaf.Length; i++)
        {
            kovaseffaf[i].material.SetTextureScale("_MainTex", new Vector2(0, kovabaslangicdeger));
        }
        sounds[2].Play();

        if (girenTopSayisi == hedefTopSayisi)
        {
            Win();
        }

        int sayi = 0;
        foreach (var item in ballpooling)
        {
            if (item.activeInHierarchy)
            {
                sayi++;
            }
            if (girenTopSayisi == 0 & mevcutTopSayisi < hedefTopSayisi)
            {
                Go();
            }
            if (mevcutTopSayisi == 0 & girenTopSayisi < hedefTopSayisi)
            {
                Go();
            }
        }
    }
    public void TopCikti()
    {
        int sayi = 0;
        foreach (var item in ballpooling)
        {
            if (item.activeInHierarchy)
            {
                sayi++;
            }
            if (girenTopSayisi == 0 & mevcutTopSayisi < hedefTopSayisi)
            {
                Go();
            }
            if (mevcutTopSayisi == 0 & girenTopSayisi < hedefTopSayisi)
            {
                Go();
            }
        }
    }
    public void ButtonIslemleri(string button)
    {
        switch (button)
        {
            case "paused":
                oyun_aksinmi = false;
                pausepnl.SetActive(true);
                break;
            case "resumed":
                pausepnl.SetActive(false);
                panelacikmi = true;
                break;
            case "tryagain":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "exit":
                Application.Quit();
                break;
            case "nextlevel":
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
                break;
            case "music":
                if (PlayerPrefs.GetInt("Music") == 1)
                {
                    PlayerPrefs.SetInt("Music", 0);

                    sounds[0].mute = true;
                    buttons[1].image.sprite = sprites[3];
                }

                else if (PlayerPrefs.GetInt("Music") == 0)
                {
                    PlayerPrefs.SetInt("Music", 1);

                    sounds[0].mute = false;
                    buttons[1].image.sprite = sprites[2];
                }
                break;

            case "sound":
                if (PlayerPrefs.GetInt("Sound") == 1)
                {
                    PlayerPrefs.SetInt("Sound", 0);

                    sounds[1].mute = true;
                    sounds[2].mute = true;
                    sounds[3].mute = true;
                    sounds[4].mute = true;

                    buttons[0].image.sprite = sprites[1];
                }

                else if (PlayerPrefs.GetInt("Sound") == 0)
                {
                    PlayerPrefs.SetInt("Sound", 1);

                    sounds[1].mute = false;
                    sounds[2].mute = false;
                    sounds[3].mute = false;
                    sounds[4].mute = false;

                    buttons[0].image.sprite = sprites[0];
                }
                break;
        }
    }
    void Win()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        winpnl.SetActive(true);
        sounds[3].Play();
        winlvltxt.text = SceneManager.GetActiveScene().name;

    }
    void Go()
    {
        gopnl.SetActive(true);
        sounds[4].Play();
        golvltxt.text = SceneManager.GetActiveScene().name;
    }
}
