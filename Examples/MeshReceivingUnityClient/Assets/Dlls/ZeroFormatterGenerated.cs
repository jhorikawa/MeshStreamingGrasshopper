#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;
    using global::ZeroFormatter.Comparers;

    public static partial class ZeroFormatterInitializer
    {
        static bool registered = false;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            if(registered) return;
            registered = true;
            // Enums
            // Objects
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MeshStreaming.CustomMesh>.Register(new ZeroFormatter.DynamicObjectSegments.MeshStreaming.CustomMeshFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            // Structs
            // Unions
            // Generics
            ZeroFormatter.Formatters.Formatter.RegisterList<ZeroFormatter.Formatters.DefaultResolver, float[]>();
            ZeroFormatter.Formatters.Formatter.RegisterList<ZeroFormatter.Formatters.DefaultResolver, int[]>();
        }
    }
}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.MeshStreaming
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class CustomMeshFormatter<TTypeResolver> : Formatter<TTypeResolver, global::MeshStreaming.CustomMesh>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::MeshStreaming.CustomMesh value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (3 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref bytes, startOffset, offset, 0, value.vertices);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref bytes, startOffset, offset, 1, value.uvs);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref bytes, startOffset, offset, 2, value.normals);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Collections.Generic.IList<int[]>>(ref bytes, startOffset, offset, 3, value.faces);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 3);
            }
        }

        public override global::MeshStreaming.CustomMesh Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new CustomMeshObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class CustomMeshObjectSegment<TTypeResolver> : global::MeshStreaming.CustomMesh, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 0, 0, 0, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        global::System.Collections.Generic.IList<float[]> _vertices;
        global::System.Collections.Generic.IList<float[]> _uvs;
        global::System.Collections.Generic.IList<float[]> _normals;
        global::System.Collections.Generic.IList<int[]> _faces;

        // 0
        public override global::System.Collections.Generic.IList<float[]> vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                __tracker.Dirty();
                _vertices = value;
            }
        }

        // 1
        public override global::System.Collections.Generic.IList<float[]> uvs
        {
            get
            {
                return _uvs;
            }
            set
            {
                __tracker.Dirty();
                _uvs = value;
            }
        }

        // 2
        public override global::System.Collections.Generic.IList<float[]> normals
        {
            get
            {
                return _normals;
            }
            set
            {
                __tracker.Dirty();
                _normals = value;
            }
        }

        // 3
        public override global::System.Collections.Generic.IList<int[]> faces
        {
            get
            {
                return _faces;
            }
            set
            {
                __tracker.Dirty();
                _faces = value;
            }
        }


        public CustomMeshObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 3, __elementSizes);

            _vertices = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(originalBytes, 0, __binaryLastIndex, __tracker);
            _uvs = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(originalBytes, 1, __binaryLastIndex, __tracker);
            _normals = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(originalBytes, 2, __binaryLastIndex, __tracker);
            _faces = ObjectSegmentHelper.DeserializeSegment<TTypeResolver, global::System.Collections.Generic.IList<int[]>>(originalBytes, 3, __binaryLastIndex, __tracker);
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (3 + 1));

                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref targetBytes, startOffset, offset, 0, _vertices);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref targetBytes, startOffset, offset, 1, _uvs);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::System.Collections.Generic.IList<float[]>>(ref targetBytes, startOffset, offset, 2, _normals);
                offset += ObjectSegmentHelper.SerializeSegment<TTypeResolver, global::System.Collections.Generic.IList<int[]>>(ref targetBytes, startOffset, offset, 3, _faces);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 3);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
