using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MeshStreaming
{
    public class SaveBytesComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public SaveBytesComponent()
          : base("SaveBytes", "SaveBytes",
              "Save Bytes to File",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bytes", "Bytes", "Bytes to save", GH_ParamAccess.item);
            pManager.AddTextParameter("File Name", "Filename", "File name with path", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Save", "Save", "Turn on to save", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool save = false;
            byte[] bytes = new byte[0];
            string filename = "";

            if (!DA.GetData(0, ref bytes)) return;
            if (!DA.GetData(1, ref filename) || filename == "") return;
            if (!DA.GetData(2, ref save)) return;

            if (save)
            {
                try
                {
                    // Open file for reading
                    System.IO.FileStream _FileStream =
                      new System.IO.FileStream(filename, System.IO.FileMode.Create,
                      System.IO.FileAccess.Write);
                    // Writes a block of bytes to this stream using data from
                    // a byte array.
                    _FileStream.Write(bytes, 0, (bytes).Length);

                    // close file stream
                    _FileStream.Close();

                }
                catch (Exception _Exception)
                {
                    // Error
                    Console.Write("Exception caught in process: {0}",
                      _Exception.ToString());
                }
            }
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
                return MeshStreaming.Properties.Resources.SaveBytes;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{cff91749-b711-4288-bbd5-bc39025f53e7}"); }
        }
    }
}