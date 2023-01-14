using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class MeshParticle : MonoBehaviour
{
    public GameObject model;
    float modelDiableDelay = 2;
    float modelEnableDelay = 6.5f;
    float animEnableDelay = 1;
    float autoDestroyDelay = 22;

    class Eff
    {
        public MeshParticleUnit unit;
        public float effectDiableDelay;
        public float pointCountPerArea;
        public bool startEffect;
         
        public Eff(
            string unitPrefabName,
            float effectDiableDelay,
            float pointCountPerArea,
            bool startEffect = true
            )
        {
            GameObject a = (GameObject)Resources.Load(unitPrefabName);
            MeshParticleUnit unitPrefab = a.GetComponent<MeshParticleUnit>();
            this.unit = Instantiate(unitPrefab);
            this.effectDiableDelay = effectDiableDelay;
            this.pointCountPerArea = pointCountPerArea;
            this.startEffect = startEffect;

            Debug.Log($"Effコンストラクタ {unit}");
        }
    }
    List<Eff> effs = new List<Eff>();
    
    Animator pausedAnim;

    void Start()
    {
        effs.Add(new Eff("MeshParticleVFXUnit1", modelEnableDelay + 5, 6000));
        effs.Add(new Eff("MeshParticleVFXUnit2", modelDiableDelay, 3000));
        effs.Add(new Eff("MeshParticleVFXUnit3", modelEnableDelay + 15, 3000));
        Destroy(gameObject, autoDestroyDelay);
        StartCoroutine(StartTargetActiveSequence());
        effs.ForEach(eff => UpdateEffect(eff));
    }


    void UpdateEffect(Eff eff)
    {
        if (!eff.startEffect) return;
        InitEffect(eff);
        StartCoroutine(StartEffectAtiveSequence(eff));
    }


    IEnumerator StartTargetActiveSequence()
    {
        yield return new WaitForSeconds(modelDiableDelay);

        model.SetActive(false);

        yield return new WaitForSeconds(modelEnableDelay);

        //model.SetActive(true);

        if (pausedAnim != null)
        {
            yield return new WaitForSeconds(animEnableDelay);
            pausedAnim.enabled = true;
            pausedAnim = null;
        }
    }

    IEnumerator StartEffectAtiveSequence(Eff eff)
    {
        Debug.Log($"1  {eff.unit}  {eff.effectDiableDelay}");
        yield return new WaitForSeconds(eff.effectDiableDelay);
        Debug.Log($"2  {eff.unit}  {eff.effectDiableDelay}");
        Destroy(eff.unit.gameObject);
        effs.Remove(eff);        
    }


    void InitEffect(Eff eff)
    {

        Debug.Log("Init");
        var modelTrans = model.transform;
        transform.SetPositionAndRotation(modelTrans.position, modelTrans.rotation);

        var meshAndTexs = GetMeshData();
        foreach (var (mesh, tex) in meshAndTexs)
        {
            Debug.Log("Init2");
        
            eff.unit.gameObject.SetActive(true);
            eff.unit.transform.SetParent(transform, false);

            eff.unit.mapSet = MeshToMap.ComputeMap(mesh, eff.pointCountPerArea);
            eff.unit.modelMainTex = tex;
        }
    }


    IEnumerable<(Mesh, Texture)> GetMeshData()
    {
        var smr = model.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            // stop animation
            pausedAnim = model.GetComponentInParent<Animator>();
            pausedAnim.enabled = false;

            var material = smr.sharedMaterial;

            var mesh = new Mesh();
            smr.BakeMesh(mesh);

            Debug.Log($"上 {material.mainTexture}");

            return new[] { (mesh, material.mainTexture) };
        }
        else
        {
            var mf = model.GetComponentInChildren<MeshFilter>();
            var renderer = model.GetComponentInChildren<Renderer>();

            var mesh = mf.sharedMesh;
            var meshCount = mesh.subMeshCount;
            var materials = renderer.sharedMaterials;

            Debug.Log($"下 {materials}");

            Debug.Log(materials.First().mainTexture);

            return new[] { (mesh, materials.First().mainTexture) };
        }
    }
}