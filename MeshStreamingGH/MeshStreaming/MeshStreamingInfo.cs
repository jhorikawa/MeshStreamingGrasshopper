using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace MeshStreaming
{
    public class MeshStreamingInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "MeshStreaming";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("639065fd-aaea-4f1a-9e06-918eeed01d45");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
