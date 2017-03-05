using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace MeshStreaming
{
    class Utils
    {
        public static Mesh GetMesh(CustomMesh customMesh)
        {
            Mesh mesh = new Mesh();

            List<Point3f> tempVertices = new List<Point3f>();
            for (int i = 0; i < customMesh.vertices.Count; i++)
            {
                Point3d pt = new Point3d(customMesh.vertices[i][0], customMesh.vertices[i][1], customMesh.vertices[i][2]);
                mesh.Vertices.Add(pt);
            }

            for (int i = 0; i < customMesh.uvs.Count; i++)
            {
                Point2f uv = new Point2f(customMesh.uvs[i][0], customMesh.uvs[i][1]);
                mesh.TextureCoordinates.Add(uv);
            }

            for (int i = 0; i < customMesh.normals.Count; i++)
            {
                Vector3d normal = new Vector3d(customMesh.normals[i][0], customMesh.normals[i][1], customMesh.normals[i][2]);
                mesh.Normals.Add(normal);
            }

            for (int i = 0; i < customMesh.faces.Count; i++)
            {
                if (customMesh.faces[i][0] == 0)
                {
                    mesh.Faces.AddFace(new MeshFace(customMesh.faces[i][1], customMesh.faces[i][2], customMesh.faces[i][3]));
                }
                else
                {
                    mesh.Faces.AddFace(new MeshFace(customMesh.faces[i][1], customMesh.faces[i][2], customMesh.faces[i][3], customMesh.faces[i][4]));
                }
            }


            return mesh;

        }

        public static CustomMesh InitCustomMesh(Mesh m)
        {
            CustomMesh customMesh = new CustomMesh();

            customMesh.vertices = new List<float[]>();
            for (int i = 0; i < m.Vertices.Count; i++)
            {
                customMesh.vertices.Add(new float[] { m.Vertices[i].X, m.Vertices[i].Y, m.Vertices[i].Z });
            }

            customMesh.uvs = new List<float[]>();
            for (int i = 0; i < m.TextureCoordinates.Count; i++)
            {
                customMesh.uvs.Add(new float[] { m.TextureCoordinates[i].X, m.TextureCoordinates[i].Y });
            }

            customMesh.normals = new List<float[]>();
            for (int i = 0; i < m.Normals.Count; i++)
            {
                customMesh.normals.Add(new float[] { m.Normals[i].X, m.Normals[i].Y, m.Normals[i].Z });
            }

            customMesh.faces = new List<int[]>();
            for (int i = 0; i < m.Faces.Count; i++)
            {
                customMesh.faces.Add(new int[] { Convert.ToInt32(m.Faces[i].IsQuad), m.Faces[i].A, m.Faces[i].B, m.Faces[i].C, m.Faces[i].D });
            }

            return customMesh;
        }
    }
}
