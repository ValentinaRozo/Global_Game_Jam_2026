using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //[SerializeField] private AudioSource clip;
    [SerializeField] AudioClip[] clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource startGame;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        startGame = GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {       
        Debug.Log("prueba");
        PlayIndex();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("prueba 2");
        PlayIndex();
    }

    public void PlaySound()
    {
        startGame.Play();
        Debug.Log("Funciona");
    }

    void PlayIndex()
    {
        int randomindex = Random.Range(0, clip.Length);
        source.clip= clip[randomindex];
        source.Play();
    }
}
