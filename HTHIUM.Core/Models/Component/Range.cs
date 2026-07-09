namespace HTHIUM.Core.Models.Component
{
    public struct Range
    {
        public double Start { get; set; }
        public double End { get; set; }

        public Range(double start, double end)
        {
            Start = start;
            End = end;
        }
    }
}
