using UnityEngine;
using UnityEngine.VFX;


/// <summary>
/// 【読んでね】
/// このスクリプトにはVFXGraphのいくつかのプロパティを設定する処理が書いてあるが、
/// いづれもメッシュから動的に AttributeMap を生成するのに必要なプロパティである。
/// その目的に対して最低限の機能だけを書いたものなので、
/// ここでパーティクルの数の設定など、関係ないプロパティはいじらない。
/// あくまでここでは動的に AttributeMap を設定する責任だけをうけおい、
/// 他の設定は他のスクリプトなりインスペクターなりVFXGraphから直接なりで行う。
/// </summary>

/// <summary>
/// 【使い方】
/// これをアタッチしたゲームオブシェクトのインスペクターから子オブジェクトに、
/// 実際のVFXGraphをアタッチしたオブシェクト(つまりエフェクト本体)を1つだけ設置する。
/// それが自動的に effect変数 に入って処理が進む。
/// それ以外の子オブジェクトは作らず、エフェクト本体を1つのみ。
/// エディタ上では実質、これをアタッチしたオブシェクトがエフェクトそのもののように扱う。
/// アタッチするオブシェクトのネーミングは「子オブシェクトの名前+Unit」とか良いと思う。
/// </summary>
public class MeshParticleUnit : MonoBehaviour
{
    public static class PropName
    {
        public const string PositionMap = "PositionMap";
        public const string UVMap = "UVMap";
        public const string ModelMainTex = "ModelMainTex";
        public const string ParticleCount = "ParticleCount";
    }


    public VisualEffect effect;
    public MapSet mapSet;
    public Texture modelMainTex;

    private void Start()
    {
        effect = transform.GetChild(0).GetComponent<VisualEffect>();
    }

    void Update()
    {
        DinamicSetMeshParticle();
    }

    void DinamicSetMeshParticle()
    {
        effect.SetTexture(PropName.PositionMap, mapSet.position);
        effect.SetTexture(PropName.UVMap, mapSet.uv);
        effect.SetTexture(PropName.ModelMainTex, modelMainTex);
        effect.SetInt(PropName.ParticleCount, mapSet.vtxCount);
    }
}
 