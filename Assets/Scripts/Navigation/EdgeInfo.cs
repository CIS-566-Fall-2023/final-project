using Geom;

namespace Navigation
{
    public struct EdgeInfo
    {
        public int ToVertex;
        public ICurve Curve;
        public EdgeTag Tag;

        public EdgeInfo(int toVertex, ICurve curve, EdgeTag tag)
        {
            ToVertex = toVertex;
            Curve = curve;
            Tag = tag;
        }
    }
}