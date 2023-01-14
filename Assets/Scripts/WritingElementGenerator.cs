using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 左クリックでなぞった領域に障害物を生成するスクリプト
public class WritingElementGenerator : MonoBehaviour
{
	[Header("Prefabs"), SerializeField]
	public GameObject ObstacleSphere;
	public GameObject ObstacleCylinder;

	[Header("Settings"), SerializeField]
	public float thickness = 10.0f;

	private Camera mainCamera;
	private Vector3 prevPos;
	private Vector3 nowPos;

	//加筆*****************************************
    [SerializeField]
	float planeDistanceFromCamera = 80f;

	List<GameObject> writingElements = new List<GameObject> ();

	List<List<GameObject>> writtenStrings = new List<List<GameObject>> ();
	//加筆*****************************************

	private bool previsin, nowisin;

	private Vector3 defaultPos = new Vector3(-1000, -1000, 1000);
	void Start()
	{
		mainCamera = Camera.main;
	}

	void FixedUpdate()
	{
		// フレームごとに現在のマウス位置と前フレームのマウス位置を更新
		// terrain上にのみ生成
		if (Input.GetMouseButtonDown(0) && isInArea())	
		{
			prevPos = Input.mousePosition;
		}
		else
		if (Input.GetMouseButton(0) && isInArea())
		{
			nowPos = Input.mousePosition;
			CreateObstacle();
			prevPos = nowPos;
		}

		if (Input.GetMouseButtonUp(0))
		{
			prevPos = nowPos = defaultPos;
		}
	}

	void Update()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
			Combine();
		}
    }

	// nowPos,prevPosを結ぶようなカプセル型のオブジェクトを生成する
	// カプセル型のオブジェクトは2つの球と円柱で表現される
	void CreateObstacle()
	{
		var screenPos_Prev = new Vector3(prevPos.x, prevPos.y, planeDistanceFromCamera);
		var screenPos_Now = new Vector3(nowPos.x, nowPos.y, planeDistanceFromCamera);
		if (Vector3.Distance(prevPos, nowPos) == 0.0f) return;
		if (prevPos == defaultPos) return;
		if (nowPos == defaultPos) return;

        Vector3 pos1 = mainCamera.ScreenToWorldPoint(screenPos_Prev);
		Vector3 pos2 = mainCamera.ScreenToWorldPoint(screenPos_Now);
		pos1.y = 0.0f;
		pos2.y = 0.0f;
		Vector3 pos3 = (pos1 + pos2) / 2;

		GameObject Sphere1 = Instantiate(ObstacleSphere, pos1, Quaternion.identity);
		GameObject Sphere2 = Instantiate(ObstacleSphere, pos2, Quaternion.identity);
		GameObject Cylinder = Instantiate(ObstacleCylinder, pos3, Quaternion.identity);

		writingElements.Add(Sphere1);
		writingElements.Add(Sphere2);
		writingElements.Add(Cylinder);

		Sphere1.transform.localScale = new Vector3(thickness, thickness, thickness);
		Sphere2.transform.localScale = new Vector3(thickness, thickness, thickness);

		Cylinder.transform.eulerAngles = new Vector3(90f, Vector3.SignedAngle(Vector3.forward, pos2 - pos1, Vector3.up), 0);
		Cylinder.transform.localScale = new Vector3(thickness, Vector3.Distance(pos1, pos2) / 2, thickness);
	}


	void Combine()
    {
        List<GameObject> buffer_WritingElements = new List<GameObject>();
        writingElements.ForEach(a => buffer_WritingElements.Add(a));
        writingElements.Clear();

        writtenStrings.Add(buffer_WritingElements);
		
		GameObject writingElementsParent = new GameObject();
		writingElementsParent.name = $"writingElementsParent{writtenStrings.Count}";
		writingElementsParent.AddComponent<OnClick_GenarateMeshParticle>();
		buffer_WritingElements.ForEach(a => a.transform.parent = writingElementsParent.transform);

		MeshCombiner.Ins.CombineMesh(writingElementsParent, ObstacleSphere.GetComponent<Renderer>().material);
	}



	bool isInArea()
	{
		// terrainのレイヤーのみ判定
		LayerMask mask = ~LayerMask.NameToLayer("Terrain");
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		return Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
	}

	void OnTriggerEnter(Collider collider)
	{
		string debugText = $"amount = {collider.gameObject.layer}\neat num = {LayerMask.NameToLayer("ColonyPheromone")}\n";
		Debug.Log(debugText);
	}
}