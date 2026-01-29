namespace FxOptionsEngine.Data
{
    public sealed class ForwardCurve
    {
        private readonly double spot;
        private readonly DiscountCurve domestic;
        private readonly DiscountCurve foreign;

        public ForwardCurve(
            double spot,
            DiscountCurve domestic,
            DiscountCurve foreign)
        {
            this.spot = spot;
            this.domestic = domestic;
            this.foreign = foreign;
        }

        public double GetForwardPrice(double timeToExpiry)
        {
            double dfDom = domestic.GetDiscountFactor(timeToExpiry);
            double dfFor = foreign.GetDiscountFactor(timeToExpiry);

            return spot * dfFor / dfDom;
        }
    }
}
