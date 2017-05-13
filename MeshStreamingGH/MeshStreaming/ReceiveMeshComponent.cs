using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Newtonsoft.Json.Linq;

using Quobject.SocketIoClientDotNet.Client;

namespace MeshStreaming
{
    public class ReceiveMeshComponent : GH_Component
    {
        private List<string> eventNames = new List<string>();
        private string status = "";
        private List<object> receivedDatas = new List<object>();

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ReceiveMeshComponent()
          : base("ReceiveMesh", "ReceieveMesh",
              "Receive Mesh",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Socket", "Socket", "Socket data.", GH_ParamAccess.item);
            pManager.AddTextParameter("Event Name", "Event", "Event name.", GH_ParamAccess.item, "test");
            pManager.AddBooleanParameter("Receive", "Receive", "True to receive.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "Status", "Status", GH_ParamAccess.item);
            pManager.AddGenericParameter("Received Data", "Data", "Data", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Socket socket = null;
            string eventName = "";
            bool receive = false;

            if (!DA.GetData(0, ref socket)) return;
            if (!DA.GetData(1, ref eventName)) return;
            if (!DA.GetData(2, ref receive)) return;

            if (receive && socket != null)
            {
                try
                {
                    if (!eventNames.Contains(eventName))
                    {
                        status = "Event: " + eventName + " registered";
                        
                        socket.On(eventName, (data) =>
                        {
                            status = "Data received";

                            receivedDatas.Clear();

                            JArray jarray = (JArray)data;
                            if (jarray != null)
                            {
                                for (int i = 0; i < jarray.Count; i++)
                                {
                                    JObject jobject = (JObject)jarray[i];
                                    string d = (string)jobject["mesh"];
                                    byte[] bytes = Convert.FromBase64String(d);

                                    receivedDatas.Add(bytes);
                                }
                            }

                            Grasshopper.Instances.DocumentEditor.Invoke((MethodInvoker)delegate
                            {
                                this.ExpireSolution(true);
                            });

                        });

                        eventNames.Add(eventName);
                    }
                }
                catch (Exception e)
                {
                    status = e.Message;
                }
            }
            else {
                /// Removing all socket event registration.
                status = "Expired all events in this component.";
                for(int i=0; i<eventNames.Count; i++)
                {
                    socket.Off(eventNames[i]);
                }
                eventNames.Clear();
                
            }

            DA.SetData(0, status);
            DA.SetDataList(1, receivedDatas);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{6cbe809b-50a8-498c-b0b5-0924e3b555d3}"); }
        }
    }
}