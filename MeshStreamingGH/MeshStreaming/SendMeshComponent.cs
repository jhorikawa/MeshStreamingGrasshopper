using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json.Linq;

namespace MeshStreaming
{
    public class SendMeshComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SendMeshComponent class.
        /// </summary>
        public SendMeshComponent()
          : base("SendMesh", "SendMesh",
              "Send mesh through socket",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bytes", "Bytes", "Bytes to send", GH_ParamAccess.list);//item);
            pManager.AddGenericParameter("Socket", "Socket", "Socket Data", GH_ParamAccess.item);
            pManager.AddTextParameter("Target Event", "Event", "Target event name", GH_ParamAccess.item, "gh");
            pManager.AddBooleanParameter("Send", "Send", "Send data", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "Status", "Socket status", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Length", "Length", "Sent data length", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //byte[] bytes = new byte[0];
            List<byte[]> bytesList = new List<byte[]>();
            Socket socket = null;
            bool send = false;
            List<int> dataLengthList = new List<int>();
            string targetEvent = "";

            //if (!DA.GetData(0, ref bytes)) return;
            if (!DA.GetDataList(0, bytesList)) return;
            if (!DA.GetData(1, ref socket)) return;
            if (!DA.GetData(2, ref targetEvent)) return;
            if (!DA.GetData(3, ref send)) return;


            if (socket != null)
            {
                if (send)
                {
                    var objs = new JArray();
                    for (int i = 0; i < bytesList.Count; i++)
                    {
                        var obj = new JObject();
                        obj["mesh"] = bytesList[i];
                        objs.Add(obj);
                        dataLengthList.Add(bytesList[i].Length);
                    }

                    socket.Emit(targetEvent, objs);

                    DA.SetData(0, "Data Sent");
                    DA.SetDataList(1, dataLengthList);
                }else
                {
                    DA.SetData(0, "Data Not Sent");
                }
            }else
            {
                DA.SetData(0, "Cocket is Null");
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
                return MeshStreaming.Properties.Resources.Send_Mesh;
                
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{690860ff-e5b0-4fcf-aff7-8c74a4be0579}"); }
        }
    }
}