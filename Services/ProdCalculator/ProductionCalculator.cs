namespace OneJevelsCompany.Web.Services.ProdCalculator
{
    public class ProductionCalculator : IProductionCalculator
    {
        public (int? minQty, decimal? basePrice) GetForLengthCm(
            decimal targetLengthCm, decimal unitPrice, decimal? pieceLengthMm)
        {
            if (pieceLengthMm is null or <= 0) return (null, null);
            var piecesPerCm = 10m / pieceLengthMm.Value;      // 10 mm in 1 cm
            var raw = targetLengthCm * piecesPerCm;
            var minQty = (int)Math.Ceiling(raw);
            var basePrice = minQty * unitPrice;
            return (minQty, decimal.Round(basePrice, 2));
        }
    }
}
