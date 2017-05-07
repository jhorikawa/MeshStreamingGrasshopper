#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
 
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using ZeroFormatter;
using MeshStreaming;
using ProceduralToolkit;

public class MeshReceiving : MonoBehaviour
{
	private SocketIOComponent socket;
    public GameObject receivedMeshPrefab;

	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("unity", GetMesh);

	}


    public void GetMesh(SocketIOEvent e)
    {
        
        GameObject[] receivedMeshes = GameObject.FindGameObjectsWithTag("ReceivedMesh");

        List<JSONObject> meshDatas = e.data.GetField("meshes").list;
        for (int n=0; n< meshDatas.Count; n++)
        //foreach (JSONObject meshData in e.data.GetField("meshes").list)
        {
            JSONObject meshData = meshDatas[n];
            byte[] data = Convert.FromBase64String(meshData.str);

            var customMesh = ZeroFormatterSerializer.Deserialize<CustomMesh>(data);

            //print("verts: " + customMesh.vertices.Count);

            List<Vector3> vertices = new List<Vector3>();
            if (vertices != null)
            {
                for (int i = 0; i < customMesh.vertices.Count; i++)
                {
                    vertices.Add(new Vector3(customMesh.vertices[i][0], customMesh.vertices[i][1], customMesh.vertices[i][2]));
                }
            }


            List<int> triangles = new List<int>();
            if (customMesh.faces != null)
            {
                for (int i = 0; i < customMesh.faces.Count; i++)
                {
                    if (customMesh.faces[i][0] == 0)
                    {
                        triangles.Add(customMesh.faces[i][1]);
                        triangles.Add(customMesh.faces[i][2]);
                        triangles.Add(customMesh.faces[i][3]);
                    }
                    else if (customMesh.faces[i][0] == 1)
                    {
                        triangles.Add(customMesh.faces[i][1]);
                        triangles.Add(customMesh.faces[i][2]);
                        triangles.Add(customMesh.faces[i][3]);

                        triangles.Add(customMesh.faces[i][1]);
                        triangles.Add(customMesh.faces[i][3]);
                        triangles.Add(customMesh.faces[i][4]);
                    }
                }
            }

            List<Vector2> uvs = new List<Vector2>();
            if (customMesh.uvs != null)
            {
                for (int i = 0; i < customMesh.uvs.Count; i++)
                {
                    uvs.Add(new Vector2(customMesh.uvs[i][0], customMesh.uvs[i][1]));
                }
            }

            List<Vector3> normals = new List<Vector3>();
            if (customMesh.normals != null)
            {
                for (int i = 0; i < customMesh.normals.Count; i++)
                {
                    normals.Add(new Vector3(customMesh.normals[i][0], customMesh.normals[i][1], customMesh.normals[i][2]));
                }
            }

            MeshDraft meshDraft = new MeshDraft();
            meshDraft.vertices = vertices;
            meshDraft.triangles = triangles;
            meshDraft.uv = uvs;
            meshDraft.normals = normals;

            if(n+1 >= receivedMeshes.Length)
            {
                GameObject receivedMeshInstance = (GameObject)Instantiate(receivedMeshPrefab);
                receivedMeshInstance.GetComponent<MeshFilter>().mesh = meshDraft.ToMesh();
                
            }else
            {
                receivedMeshes[n].GetComponent<MeshFilter>().mesh = meshDraft.ToMesh();
            }

        }

        if(receivedMeshes.Length > meshDatas.Count)
        {
            for(int i= meshDatas.Count; i< receivedMeshes.Length; i++)
            {
                Destroy(receivedMeshes[i]);
            }

            Resources.UnloadUnusedAssets();
        }

    }
}
