using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using Rhino.Geometry;

namespace MeshStreaming
{
    [ZeroFormattable]
    public class CustomMesh
    {
        [Index(0)]
        public virtual List<float[]> vertices { get; set; }

        [Index(1)]
        public virtual List<float[]> uvs { get; set; }

        [Index(2)]
        public virtual List<float[]> normals { get; set; }

        [Index(3)]
        public virtual List<int[]> faces { get; set; }

        public CustomMesh() { }

        public CustomMesh(Mesh m)
        {
            vertices = new List<float[]>();
            for (int i = 0; i < m.Vertices.Count; i++)
            {
                vertices.Add(new float[] { m.Vertices[i].X, m.Vertices[i].Y, m.Vertices[i].Z });
            }

            uvs = new List<float[]>();
            for (int i = 0; i < m.TextureCoordinates.Count; i++)
            {
                uvs.Add(new float[] { m.TextureCoordinates[i].X, m.TextureCoordinates[i].Y });
            }

            normals = new List<float[]>();
            for (int i = 0; i < m.Normals.Count; i++)
            {
                normals.Add(new float[] { m.Normals[i].X, m.Normals[i].Y, m.Normals[i].Z });
            }

            faces = new List<int[]>();
            for (int i = 0; i < m.Faces.Count; i++)
            {
                faces.Add(new int[] { Convert.ToInt32(m.Faces[i].IsQuad), m.Faces[i].A, m.Faces[i].B, m.Faces[i].C, m.Faces[i].D });
            }
        }

        public Mesh GetMesh()
        {
            Mesh mesh = new Mesh();

            List<Point3f> tempVertices = new List<Point3f>();
            for(int i=0; i<vertices.Count; i++)
            {
                Point3d pt = new Point3d(vertices[i][0], vertices[i][1], vertices[i][2]);
                mesh.Vertices.Add(pt);
            }

            for(int i=0; i<uvs.Count; i++)
            {
                Point2f uv = new Point2f(uvs[i][0], uvs[i][1]);
                mesh.TextureCoordinates.Add(uv);
            }

            for(int i=0; i<normals.Count; i++)
            {
                Vector3d normal = new Vector3d(normals[i][0], normals[i][1], normals[i][2]);
                mesh.Normals.Add(normal);
            }

            for (int i = 0; i < faces.Count; i++)
            {
                if (faces[i][0] == 0) {
                    mesh.Faces.AddFace(new MeshFace(faces[i][1],faces[i][2],faces[i][3]));
                }else
                {
                    mesh.Faces.AddFace(new MeshFace(faces[i][1], faces[i][2], faces[i][3], faces[i][4]));
                }
            }


            return mesh;

        }
    }
}
