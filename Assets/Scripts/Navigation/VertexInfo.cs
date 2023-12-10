using Geom;

namespace Navigation
{
    public struct VertexInfo
    {
        public Geom.IRegion region;
        public VertexTag tag;

        public VertexInfo(IRegion region, VertexTag tag = VertexTag.None)
        {
            this.region = region;
            this.tag = tag;
        }
        
    }
}