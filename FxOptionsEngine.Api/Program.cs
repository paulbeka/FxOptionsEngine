using FxOptionsEngine.Data;
using FxOptionsEngine.MarketData.Rates;
using FxOptionsEngine.Model.Sabr;
using FxOptionsEngine.Surfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IVolatilitySurface>(sp =>
{
    var http = new HttpClient();

    RateProvider sofrRates = new SofrRateProvider(http);
    RateProvider estrRates = new EstrRateProvider(http);

    var usdCurve = sofrRates.GetDicsountCurve().GetAwaiter().GetResult();
    var eurCurve = estrRates.GetDicsountCurve().GetAwaiter().GetResult();

    var usdDiscountCurve = new DiscountCurve(usdCurve);
    var eurDiscountCurve = new DiscountCurve(eurCurve);

    var forwardCurve = new ForwardCurve(1.0, usdDiscountCurve, eurDiscountCurve);

    return new SabrVolatilitySurface(new SabrModel(), forwardCurve);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
