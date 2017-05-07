using System.Collections.Generic;
using ZeroFormatter;

namespace MeshStreaming
{
    [ZeroFormattable]
    public class CustomMesh
    {
        [Index(0)]
        public virtual IList<float[]> vertices { get; set; }

        [Index(1)]
        public virtual IList<float[]> uvs { get; set; }

        [Index(2)]
        public virtual IList<float[]> normals { get; set; }

        [Index(3)]
        public virtual IList<int[]> faces { get; set; }

        public CustomMesh() { }
    }
}
