using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private float _maxOffset;


    [SerializeField]
    private List<Vector3> _obstacleSpawnPos;

    private Vector3 moveDirection;
    private bool hasGameFinished;


    private void Start()
    {
        hasGameFinished = false;



        Vector3 spawnPos;
        int posIndex;
        posIndex = Random.Range(0, _obstacleSpawnPos.Count);
        spawnPos = _obstacleSpawnPos[posIndex];
        transform.position = spawnPos;
        moveDirection = -1 * spawnPos.normalized;
    }

    private void OnEnable()
    {
        GameManager.Instance.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameEnded -= OnGameEnded;
    }


    private void FixedUpdate()
    {
        if (hasGameFinished) return;

        transform.position += _moveSpeed * Time.fixedDeltaTime * moveDirection;

        transform.Rotate(_rotateSpeed * Time.fixedDeltaTime * Vector3.forward);

        if (transform.position.x > _maxOffset || transform.position.x < -_maxOffset)
        {
            Destroy(gameObject);
        }
    }

    public void OnGameEnded()
    {
        GetComponent<Collider2D>().enabled = false;
        hasGameFinished = true;
        StartCoroutine(Rescale());
    }

    [SerializeField]
    private float _destroyTime;

    private IEnumerator Rescale()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        Vector3 scaleOffset = endScale - startScale;
        float timeElapsed = 0f;
        float speed = 1 / _destroyTime;
        var updateTime = new WaitForFixedUpdate();
        while (timeElapsed < 1f)
        {
            timeElapsed += speed * Time.fixedDeltaTime;
            transform.localScale = startScale + timeElapsed * scaleOffset;
            yield return updateTime;
        }

        Destroy(gameObject);
    }
}
