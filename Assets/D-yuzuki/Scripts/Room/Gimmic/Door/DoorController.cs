using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ƒhƒA‚Ì§Œä
public class DoorController : MonoBehaviour
{
    [Header("‰ñ“]‚É‚©‚¯‚éŽžŠÔ")]
    [SerializeField]
    private float _rotationTime;

    [Header("“à‘¤‚ÉŠJ‚¯‚½Žž‚Ì‰ñ“]")]
    [SerializeField]
    private Vector3 _inOpenRot = new Vector3(-90, 0, 0);
    [Header("•Â‚ß‚½Žž‚Ì‰ñ“]")]
    [SerializeField]
    private Vector3 _closeRot = new Vector3(-90, 90, 0);
    [Header("ŠO‘¤‚ÉŠJ‚¯‚½‚Æ‚«‚Ì‰ñ“]")]
    [SerializeField]
    private Vector3 _outOpenRot = new Vector3(-90, 180, 0);

    [SerializeField]
    private GameObject _doorObj;
    private bool _isOpen = false;
    private int _inObjCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomBraver") || other.CompareTag("RoomMaid")  || other.CompareTag("Player"))
        {
            _inObjCount++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RoomBraver") || other.CompareTag("RoomMaid") || other.CompareTag("Player"))
        {
            if (_isOpen) return;
            _isOpen = true;
            Vector3 direction = (_doorObj.transform.position - other.transform.position).normalized;
            // forward •ûŒü‚©‚ç“–‚½‚Á‚½
            if (direction.z >= 0)
                StartCoroutine(RotateCoroutine(Quaternion.Euler(_inOpenRot)));
            else
                StartCoroutine(RotateCoroutine(Quaternion.Euler(_outOpenRot)));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RoomBraver") || other.CompareTag("RoomMaid") ||other.CompareTag("Player"))
        {
            _inObjCount--;

            if (_inObjCount >= 1) return;
            _isOpen = false;
            StartCoroutine(RotateCoroutine(Quaternion.Euler(_closeRot)));
        }
    }

    private IEnumerator RotateCoroutine(Quaternion targetRotation)
    {
        Quaternion startRotation = _doorObj.transform.rotation;

        float elapsedTime = 0f;
        while (elapsedTime < _rotationTime)
        {
            float t = elapsedTime / _rotationTime;
            _doorObj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _doorObj.transform.rotation = targetRotation;
    }

}