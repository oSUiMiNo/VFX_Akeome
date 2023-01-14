using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// ���b�V������������N���X
/// </Summary>
public class MeshCombiner : MonoBehaviourMyExtentionSingleton<MeshCombiner>
{
    /// <summary>
    /// ���b�V������������B
    /// </summary>
    /// <param name="fieldParent"> �t�B�[���h�p�[�c�̐e�I�u�W�F�N�g��Transform </param>
    /// <param name="combinedMat"> �����������b�V���ɕt����}�e���A�� </param>
    public void CombineMesh(GameObject fieldParent, Material combinedMat)
    {
        // �e�I�u�W�F�N�g��MeshFilter�����邩�ǂ����m�F
        MeshFilter parentMeshFilter = CheckParentComponent<MeshFilter>(fieldParent);


        // �q�I�u�W�F�N�g��MeshFilter�ւ̎Q�Ƃ�z��Ƃ��ĕێ�
        // �������A�e�I�u�W�F�N�g�̃��b�V����GetComponentsInChildren�Ɋ܂܂��̂ŏ��O
        MeshFilter[] meshFilters = fieldParent.GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        for (int i = 1; i < meshFilters.Length; i++)
        {
            meshFilterList.Add(meshFilters[i]);
        }

        // �������郁�b�V���̔z����쐬
        CombineInstance[] combine = new CombineInstance[meshFilterList.Count];

        // �������郁�b�V���̏���CombineInstance�ɒǉ�
        for (int i = 0; i < meshFilterList.Count; i++)
        {
            combine[i].mesh = meshFilterList[i].sharedMesh;
            combine[i].transform = meshFilterList[i].transform.localToWorldMatrix;
            meshFilterList[i].gameObject.SetActive(false);
        }

        // �����������b�V�����Z�b�g
        parentMeshFilter.mesh = new Mesh();
        /// <Summary>
        /// �W���ŃT�|�[�g���Ă���C���f�b�N�X�o�b�t�@?���Ă�̒��_����65535�炵���A
        /// mesh ����x�Ɏ����_��������𒴂��Ă��܂��ƃo�O��̂ŁA
        /// �C���f�b�N�X�o�b�t�@�ɂ��܂��钸�_�����A16bit ���� 32bit �ɕύX���邱�Ƃ� �A
        /// �����钸�_����65535����40���ɂȂ�B
        /// </Summary>        
        parentMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        parentMeshFilter.mesh.CombineMeshes(combine);

        Debug.Log($"{parentMeshFilter.mesh.vertexCount} ��");

        fieldParent.AddComponent<MeshCollider>();

        // �����������b�V���Ƀ}�e���A�����Z�b�g
        CheckParentComponent<MeshRenderer>(fieldParent).material = combinedMat;

        // �e�I�u�W�F�N�g��\��
        fieldParent.SetActive(true);
    }



    /// <Summary>
    /// �����̃I�u�W�F�N�g�̃R���|�[�l���g���f�^�b�`����B
    /// </Summary>
    void RemoveMeshes(GameObject obj)
    {
        // �e�I�u�W�F�N�g�̃R���|�[�l���g���擾���ATransform�ȊO�̃R���|�[�l���g���f�^�b�`
        foreach (Component a in obj.GetComponents<Component>())
        {
            if (a.GetType() != typeof(Transform))
            {
                GameObject.Destroy(a);
            }
        }
    }
}