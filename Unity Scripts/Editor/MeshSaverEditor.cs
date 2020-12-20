using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class MeshSaverEditor {

	[MenuItem("CONTEXT/MeshFilter/Save Mesh as OBJ...")]
	public static void SaveMeshInPlace (MenuCommand menuCommand) {
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMeshOBJ(m, m.name, true);
	}

	[MenuItem("CONTEXT/MeshFilter/Save Mesh as Asset...")]
	public static void SaveMeshNewInstanceItem (MenuCommand menuCommand) {
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMesh(m, m.name, true, true);
	}

	public static void SaveMesh (Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) {
		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		if (string.IsNullOrEmpty(path)) return;
        
		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;
		
		if (optimizeMesh)
		     MeshUtility.Optimize(meshToSave);
        
		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
	}

	public static void SaveMeshOBJ(Mesh m, string name, bool optimize) {
		string path = EditorUtility.SaveFilePanel("Save Mesh OBJ", "Assets/", name, "obj");
		if (string.IsNullOrEmpty(path)) return;
        
		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = Object.Instantiate(m) as Mesh;
		
		if (optimize)
		     MeshUtility.Optimize(meshToSave);
        
		FileStream fParameter = new FileStream(path, FileMode.Create, FileAccess.Write);
		StreamWriter m_WriterParameter = new StreamWriter(fParameter);
        m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);

        m_WriterParameter.Write("o " + name + "\n");
        m_WriterParameter.Write("g " + name + "\n");
		//vn -0.0000 -0.0000 -1.0000
		foreach (var v in meshToSave.vertices) {
        	m_WriterParameter.Write("v " + -((float)v.x) + " " + ((float)v.y) + " " + ((float)v.z) + "\n");
		}
        m_WriterParameter.Write("vn 0 0 1\n");
        m_WriterParameter.Write("usemtl None\n");
        m_WriterParameter.Write("s off\n");
		for (int i = 0; i < meshToSave.triangles.Length; i += 3) {
        	//m_WriterParameter.Write("f " + (meshToSave.triangles[i]+1) + "//1 " + (meshToSave.triangles[i+1]+1) + "//1 " + (meshToSave.triangles[i+2]+1) + "//1" + "\n");
        	m_WriterParameter.Write("f " + (meshToSave.triangles[i+2]+1) + "//1 " + (meshToSave.triangles[i+1]+1) + "//1 " + (meshToSave.triangles[i]+1) + "//1" + "\n");
		}

        m_WriterParameter.Flush();
        m_WriterParameter.Close();
	}
	
}
