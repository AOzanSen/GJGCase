using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();

        this.transform.DOMoveY(this.transform.position.y, 1).From(3);
    }
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float _dragSpeed = 10;

    private bool _isSelected = false;
    AudioSource _audioSource;

    private void OnMouseDown()
    {
        _isSelected = true;
    }

    private void OnMouseUp()
    {
        _isSelected = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wax"))
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.UnPause();
            }
            else
            {
                _audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wax"))
        {
            _audioSource.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _isSelected)
        {
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                Camera.main.ScreenToWorldPoint(Input.mousePosition + offset), 
                Time.deltaTime * _dragSpeed);
        }
    }
}
