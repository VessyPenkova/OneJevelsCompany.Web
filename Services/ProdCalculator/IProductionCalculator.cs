namespace OneJevelsCompany.Web.Services.ProdCalculator
{
    public interface IProductionCalculator
    {
        (int? minQty, decimal? basePrice) GetForLengthCm(
            decimal targetLengthCm, decimal unitPrice, decimal? pieceLengthMm);
    }
}
