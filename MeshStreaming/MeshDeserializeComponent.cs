using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using ZeroFormatter;

namespace MeshStreaming
{
    public class MeshDeserializeComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshDeserializeComponent class.
        /// </summary>
        public MeshDeserializeComponent()
          : base("MeshDeserialize", "MeshDeserialize",
              "Deserialize Mesh",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bytes", "Bytes", "Bytes data to deserialize", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Deserialized Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            byte[] bytes = new byte[0];

            if (!DA.GetData(0, ref bytes)) return;

            var customMesh = ZeroFormatterSerializer.Deserialize<CustomMesh>(bytes);


            DA.SetData(0, Utils.GetMesh(customMesh));
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
                //return null;
                return MeshStreaming.Properties.Resources.Deserialize;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{30412cdd-382e-4617-b495-c0455eebd28f}"); }
        }
    }
}