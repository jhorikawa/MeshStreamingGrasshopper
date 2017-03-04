using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


using Quobject.SocketIoClientDotNet.Client;

namespace MeshStreaming
{
    public class ConnectSocketComponent : GH_Component
    {
        private int count = 0;
        public Socket socket;
        //private static bool _askingNewSolution = false;
        private string status = "";
        /// <summary>
        /// Initializes a new instance of the ConnectSocketComponent class.
        /// </summary>
        public ConnectSocketComponent()
          : base("ConnectSocket", "ConnectSocket",
              "Connect socket",
              "MeshStreaming", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Address", "Address", "IP Address to send to", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Connect", "Connect", "Send data", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "Status", "Socket status", GH_ParamAccess.item);
            pManager.AddGenericParameter("Socket", "Socket", "Received data", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string address = "";
            bool send = false;

            if (!DA.GetData(0, ref address)) return;
            if (!DA.GetData(1, ref send)) return;


            if (send)
            {
                if(count == 0)
                {
                    try
                    {
                        socket = IO.Socket(address);


                        socket.On(Socket.EVENT_CONNECT, () =>
                        {
                            

                            status = "Connected";

                            Grasshopper.Instances.DocumentEditor.Invoke((MethodInvoker)delegate
                            {
                                this.ExpireSolution(true);
                            });

                            //Grasshopper.Instances.ActiveCanvas.Document.NewSolution(false);
                            //Reset();
                        });

                        socket.On(Socket.EVENT_ERROR, () =>
                        {
                            status = "Error";
                        });

                        socket.On(Socket.EVENT_DISCONNECT, () =>
                        {
                            status = "Disconnected";
                        });

                        socket.On(Socket.EVENT_RECONNECT, () =>
                        {
                            status =  "Reconnected";
                        });


                        count++;
                    }
                    catch(System.UriFormatException e)
                    {
                        status = "URI Format Exception";
                    }
                }
                //else
                //{
                //    if(socket != null)
                //    {
                //        var obj = new JObject();
                //        obj["value"] = "Sample Data";

                //        socket.Emit("testdata",obj);
                //    }
                //}
                
            }else
            {
                if (socket != null){
                    socket.Disconnect();
                }
                count = 0;
            }

            DA.SetData(0, status);
            DA.SetData(1, socket);
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
                return MeshStreaming.Properties.Resources.Connect;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{44bae8ec-4694-4bbb-ae47-dd2b43926a6a}"); }
        }

        public void Reset()
        {
            
            //try
            //{
            //this.ExpireSolution(true);
            //Grasshopper.Instances.ActiveCanvas.Document.ExpireSolution();
            //}
            //catch (Exception ex)
            //{

            //    Rhino.RhinoApp.WriteLine("Exception in SolutionEnd event: {0}", ex.ToString());
            //}
        }
    }
}