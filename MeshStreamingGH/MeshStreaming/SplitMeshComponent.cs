using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace MeshStreaming
{
    public class SplitMeshComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SplitMeshComponent class.
        /// </summary>
        public SplitMeshComponent()
          : base("SplitMesh", "SplitMesh",
              "Split Mesh",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh to split.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Max Vertices Count", "Count", "Max vertices count to split mesh.", GH_ParamAccess.item, 65000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Splitted meshes.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = null;
            int maxVertices  = 0;
            if (!DA.GetData(0, ref mesh)) return;
            if (!DA.GetData(1, ref maxVertices)) return;

            mesh.CreatePartitions(maxVertices, maxVertices * 2);
            List<GH_Mesh> newMeshes = new List<GH_Mesh>();

            for (int i = 0; i < mesh.PartitionCount; i++)
            {
                MeshPart meshPart = mesh.GetPartition(i);
                IEnumerable<MeshFace> meshFaces = mesh.Faces.Skip(meshPart.StartFaceIndex).Take(meshPart.EndFaceIndex - meshPart.StartFaceIndex);

                Mesh partMesh = new Mesh();
                partMesh.Vertices.AddVertices(mesh.Vertices);
                partMesh.Faces.AddFaces(meshFaces);
                partMesh.Normals.ComputeNormals();
                partMesh.Compact();

                GH_Mesh ghmesh = new GH_Mesh(partMesh);

                newMeshes.Add(ghmesh);
            }

            DA.SetDataList(0, newMeshes);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return MeshStreaming.Properties.Resources.Split;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9ec2c5fd-4d4e-4151-bc3a-b83b43dc3833}"); }
        }
    }
}