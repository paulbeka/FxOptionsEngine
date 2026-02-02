namespace FxOptionsEngine.Model.Sabr
{
    public sealed class SabrParams
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Rho { get; set; }
        public double VolOfVol { get; set; }
        
        public SabrParams(double alpha, double beta, double rho, double volOfVol) 
        {
            if (alpha <= 0.0) throw new ArgumentException("Alpha must be positive.", nameof(alpha));
            if (beta < 0.0 || beta > 1.0) throw new ArgumentException("Beta must be in [0, 1].", nameof(beta));
            if (rho <= -1.0 || rho >= 1.0) throw new ArgumentException("Rho must be in (-1, 1).", nameof(rho));
            if (volOfVol <= 0.0) throw new ArgumentException("VolOfVol must be positive.", nameof(volOfVol));

            Alpha = alpha;
            Beta = beta;
            Rho = rho;
            VolOfVol = volOfVol;
        }

        public override bool Equals(object? obj)
        {
            return obj is SabrParams @params &&
                   Alpha == @params.Alpha &&
                   Beta == @params.Beta &&
                   Rho == @params.Rho &&
                   VolOfVol == @params.VolOfVol;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
