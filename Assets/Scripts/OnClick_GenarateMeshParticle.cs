using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������A�^�b�`�����I�u�W�F�N�g���N���b�N����ƁA
/// �G�t�F�N�g�̃v���n�u���W�F�l���[�g���Ă����B
/// </summary>
public class OnClick_GenarateMeshParticle : MonoBehaviourMyExtention
{
    /// <summary>
    /// MeshParticle ���A�^�b�`�����v���n�u�B
    /// ��������̃X�N���v�g���� Instantiate() ���Ă���B
    /// Instantiate() ����ƁA�X�ɂ���(MeshParticle��)���Ŕ��菈���ɓ���A
    /// �ʂ�����A���ۂ̃G�t�F�N�g�� Instantiate() �����B
    /// </summary>
    MeshParticle meshParticlePrefab;

    [SerializeField] float interval = 1;


    float lastClickTime;

    private void Start()
    {
        LoadResources();
    }

    void LoadResources()
    {
        GameObject a = (GameObject)Resources.Load("MeshParticleEffect");
        meshParticlePrefab = a.GetComponent<MeshParticle>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastClickTime > interval)
        {
            lastClickTime = Time.time;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject != gameObject) return;

                var other = hit.collider;
                var mp = Instantiate(meshParticlePrefab);

                mp.model = other.gameObject;
                //mp.startEffect = true;

                //StartCoroutine(DestroyDelay(mp.gameObject, mp.effectDiableDelay + 1f));
            }
        }
    }
}
