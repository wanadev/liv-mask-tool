using UnityEngine;
using System.IO;
using System.Collections;
using UnityExtension;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Exporter : MonoBehaviour
{
	private const string OUTPUT_PATH = @"Exports/liv-mask.obj";

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			Export();
		}
	}

	public void Export()
	{
		FileStream exportStream;
		OBJData exportOBJData;
		MeshFilter exportMeshFilter = GetComponent<MeshFilter>();
		Vector3[] exportVertices;

		// Get gameObjects of panels
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Panel");
		Debug.Log(objs.Length + " panels found");
		CombineInstance[] combine = new CombineInstance[objs.Length];

		// Is there actually something to export ?
		if (objs.Length > 0)
		{
			// Setup export array
			MeshFilter firstMF = objs[0].GetComponent<MeshFilter>();
			exportVertices = new Vector3[objs.Length * firstMF.mesh.vertices.Length]; // Warning ! We make the assumption that all panels will have the same number of vertices
						
			// Iterate over all panels
			for (int i = 0; i < objs.Length; i++)
			{
				MeshFilter mf = objs[i].GetComponent<MeshFilter>();

				combine[i].mesh = mf.sharedMesh;
				combine[i].transform = mf.transform.localToWorldMatrix;
				mf.gameObject.SetActive(false);
			}
			
			//	Export the new Mesh
			if (File.Exists(OUTPUT_PATH))
			{
				File.Delete(OUTPUT_PATH);
			}

			exportStream = new FileStream(OUTPUT_PATH, FileMode.Create);

			exportMeshFilter.mesh = new Mesh();
			exportMeshFilter.mesh.CombineMeshes(combine);

			exportOBJData = exportMeshFilter.mesh.EncodeOBJ();
			OBJLoader.ExportOBJ(exportOBJData, exportStream);
			exportStream.Close();

		} else
		{
			Debug.LogWarning("No panel to export");
		}
	}
}