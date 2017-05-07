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

	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("unity", GetMesh);
		// socket.On("open", TestOpen);
		// socket.On("boop", TestBoop);
		// socket.On("error", TestError);
		// socket.On("close", TestClose);
		
		// StartCoroutine("BeepBoop");
	}

	// private IEnumerator BeepBoop()
	// {
	// 	// wait 1 seconds and continue
	// 	yield return new WaitForSeconds(1);
		
	// 	socket.Emit("beep");
		
	// 	// wait 3 seconds and continue
	// 	yield return new WaitForSeconds(3);
		
	// 	socket.Emit("beep");
		
	// 	// wait 2 seconds and continue
	// 	yield return new WaitForSeconds(2);
		
	// 	socket.Emit("beep");
		
	// 	// wait ONE FRAME and continue
	// 	yield return null;
		
	// 	socket.Emit("beep");
	// 	socket.Emit("beep");
	// }

	// public void TestOpen(SocketIOEvent e)
	// {
	// 	Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	// }
	
	// public void TestBoop(SocketIOEvent e)
	// {
	// 	Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

	// 	if (e.data == null) { return; }

	// 	Debug.Log(
	// 		"#####################################################" +
	// 		"THIS: " + e.data.GetField("this").str +
	// 		"#####################################################"
	// 	);
	// }
	
	// public void TestError(SocketIOEvent e)
	// {
	// 	Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	// }
	
	// public void TestClose(SocketIOEvent e)
	// {	
	// 	Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	// }

	public void GetMesh(SocketIOEvent e)
	{
		print("data received");

		//print(e.data.GetField("mesh"));
		byte[] data = Convert.FromBase64String(e.data.GetField("mesh").str);

		//print(data.Length);
		
		var customMesh = ZeroFormatterSerializer.Deserialize<CustomMesh>(data);

		// print(customMesh.GetType());
		print("verts: " + customMesh.vertices.Count);

		List<Vector3> vertices = new List<Vector3> ();
		if (vertices != null) {
			for (int i = 0; i < customMesh.vertices.Count; i++) {
				//CustomVector3d cv = customMesh.vertices [i];
				vertices.Add (new Vector3 (customMesh.vertices[i][0], customMesh.vertices[i][1],customMesh.vertices[i][2]));
			}
		}


		List<int> triangles = new List<int> ();
		if (customMesh.faces != null) {
			for (int i=0; i<customMesh.faces.Count; i++) {
				if (customMesh.faces[i][0] == 0) {
					triangles.Add (customMesh.faces[i][1]);
					triangles.Add (customMesh.faces[i][2]);
					triangles.Add (customMesh.faces[i][3]);
				}else if(customMesh.faces[i][0] == 1){
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
		if(customMesh.uvs != null){
			for(int i=0; i<customMesh.uvs.Count; i++){
				uvs.Add(new Vector2(customMesh.uvs[i][0], customMesh.uvs[i][1]));
			}
		}

		List<Vector3> normals = new List<Vector3>();
		if(customMesh.normals != null){
			for(int i=0; i<customMesh.normals.Count; i++){
				normals.Add(new Vector3(customMesh.normals[i][0], customMesh.normals[i][1], customMesh.normals[i][2]));
			}
		}

		
		MeshDraft meshDraft = new MeshDraft ();
		meshDraft.vertices = vertices;
		meshDraft.triangles = triangles;
		meshDraft.uv = uvs;
		meshDraft.normals = normals;

		GetComponent<MeshFilter>().mesh = meshDraft.ToMesh();

		//CustomMesh customMesh = new CustomMesh();

		// print(customMesh.ToString());
		//var d = ZeroFormatterSerializer.Serialize(customMesh);
		//var dd = ZeroFormatterSerializer.Deserialize<CustomMesh>(d);
	}
}
