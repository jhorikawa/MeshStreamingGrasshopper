using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using ZeroFormatter;

namespace MeshStreaming
{
    public class MeshSerializeComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MeshSerializeComponent()
          : base("MeshSerialize", "MeshSerialize",
              "Serialize Mesh",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh to serialize", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bytes", "Bytes", "Serialized data", GH_ParamAccess.item);
            pManager.AddGenericParameter("SplittedBytes", "SplittedBytes", "Splitted serialized data", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = new Mesh();

            if (!DA.GetData(0, ref mesh)) return;

            mesh.Normals.ComputeNormals();
            CustomMesh customMesh = Utils.InitCustomMesh(mesh);
            var bytes = ZeroFormatterSerializer.Serialize(customMesh);

            DA.SetData(0, bytes);


            Mesh newMesh = new Mesh();
            newMesh.CopyFrom(mesh);
            int vertCount = 65000;
            int index = 0;
            int newMeshFacesCount = newMesh.Faces.ToIntArray(false).ToList<int>().Count;
            while (index < newMeshFacesCount)
            {

                int startIndex = index < newMeshFacesCount ? index : newMeshFacesCount;
                int[] deleteIndexes1 = newMesh.Faces.ToIntArray(false).ToList<int>().Take<int>(startIndex).ToArray<int>();
                int[] deleteIndexes2 = newMesh.Faces.ToIntArray(false).ToList<int>().GetRange(startIndex + vertCount, newMeshFacesCount - (startIndex + vertCount)).ToArray<int>();
                newMesh.Faces.DeleteFaces(deleteIndexes2);
                newMesh.Faces.DeleteFaces(deleteIndexes1);
                //newMesh.Faces = newMesh.Faces.Take<MeshFace>(vertCount);

                newMesh.Compact();

                index += vertCount;
            }

            DA.SetData(1, newMesh);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                //return null;
                return MeshStreaming.Properties.Resources.Serialize;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4e2bb095-45be-4eaa-ae79-b901d20805bf}"); }
        }
    }
}
