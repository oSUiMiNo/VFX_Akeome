using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class MeshParticle : MonoBehaviour
{
    //public static MeshParticleUnit unitPrefab1;
    //public static MeshParticleUnit unitPrefab2;

    //[SerializeField] private float effectDiableDelay = 10f;
    //[SerializeField] private float pointCountPerArea = 10000f;
   
    //public bool startEffect;
    
    public GameObject model;
    private float modelDiableDelay = 2;
    private float modelEnableDelay = 6.5f;
    private float animEnableDelay = 1;
    private float autoDestroyDelay = 22;

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
            bool activeness = true
            )
        {
            GameObject a = (GameObject)Resources.Load(unitPrefabName);
            MeshParticleUnit unitPrefab = a.GetComponent<MeshParticleUnit>();
            this.unit = Instantiate(unitPrefab);
            this.effectDiableDelay = effectDiableDelay;
            this.pointCountPerArea = pointCountPerArea;
            this.startEffect = activeness;

            Debug.Log($"Effコンストラクタ {unit}");
        }
    }

    //[SerializeField] List<MeshParticleUnit> units = new List<MeshParticleUnit>();
    //[SerializeField] Dictionary<MeshParticleUnit, float> units = new Dictionary<MeshParticleUnit, float>();
    [SerializeField] List<Eff> effs = new List<Eff>();
    
    Animator pausedAnim;

    private void Start()
    {
        effs.Add(new Eff("MeshParticleVFXUnit1", modelEnableDelay + 5,6000));
        effs.Add(new Eff("MeshParticleVFXUnit2", modelDiableDelay, 3000));
        effs.Add(new Eff("MeshParticleVFXUnit3", modelEnableDelay + 15, 3000));
        Debug.Log(modelDiableDelay);
        //LoadResources();
        Destroy(gameObject, autoDestroyDelay);
        StartCoroutine(StartTargetActiveSequence());
        effs.ForEach(eff => Debug.Log(eff));
        effs.ForEach(eff => UpdateEffect(eff));
    }

    //void LoadResources()
    //{
    //    if (unitPrefab1 == null)
    //    {
    //        GameObject a = (GameObject)Resources.Load("MeshParticleVFXUnit1");
    //        unitPrefab1 = a.GetComponent<MeshParticleUnit>();
    //    }
    //    if (unitPrefab2 == null)
    //    {
    //        GameObject a = (GameObject)Resources.Load("MeshParticleVFXUnit2");
    //        unitPrefab2 = a.GetComponent<MeshParticleUnit>();
    //    }
    //}

    //private void Update()
    //{
    //    if (effs.Any())
    //    {
    //        Debug.Log("通った1");
    //        effs.ForEach(eff => UpdateEffect(eff));
    //    }
    //}

    private void UpdateEffect(Eff eff)
    {
        if (!eff.startEffect) return;
        Debug.Log("通った2");
        InitEffect(eff);

        StartCoroutine(StartEffectAtiveSequence(eff));
    }
    //private void UpdateEffect(Eff eff)
    //{
    //    if (startEffect && units.Any() == false)
    //    {
    //        InitEffect();

    //        foreach (var a in units)
    //        {
    //            StartCoroutine(StartEffectAtiveSequence(a.Key, a.Value));
    //        }
    //        StartCoroutine(StartTargetActiveSequence());
    //        Destroy(gameObject, autoDestroyDelay);
    //    }
    //}

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
        
        //startEffect = false;
    }


    private void InitEffect(Eff eff)
    {

        Debug.Log("Init");
        var modelTrans = model.transform;
        transform.SetPositionAndRotation(modelTrans.position, modelTrans.rotation);

        var meshAndTexs = GetMeshData();
        foreach (var (mesh, tex) in meshAndTexs)
        {
            Debug.Log("Init2");
            //units.Add(Instantiate(eff.unit));
            //foreach (var a in units)
            //{
            //    a.gameObject.SetActive(true);
            //    a.transform.SetParent(transform, false);

            //    a.mapSet = MeshToMap.ComputeMap(mesh, pointCountPerArea);
            //    a.modelMainTex = tex;
            //}

            eff.unit.gameObject.SetActive(true);
            eff.unit.transform.SetParent(transform, false);

            eff.unit.mapSet = MeshToMap.ComputeMap(mesh, eff.pointCountPerArea);
            eff.unit.modelMainTex = tex;


            //unit.effect.SetFloat("ParticleSize", particleSize);
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