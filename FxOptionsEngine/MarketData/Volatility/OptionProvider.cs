using FxOptionsEngine.Calibration.SabrCalibration;
using FxOptionsEngine.Data;
using HtmlAgilityPack;
using System.Globalization;

public static class OptionsProvider
{

    private static readonly string BASE_URL = "https://www.cmegroup.com/markets/fx/g10/euro-fx.settlements.options.html";

    public static List<StrikeToMarketVolatility> Scrape(
        double forward,
        double timeToExpiry)
    {
        var web = new HtmlWeb();
        var doc = web.Load(BASE_URL);

        var rows = doc.DocumentNode.SelectNodes("//table//tr");

        var vols = new List<StrikeToMarketVolatility>();

        foreach (var row in rows.Skip(1))
        {
            var cells = row.SelectNodes("td");
            if (cells == null || cells.Count < 10) continue;

            double strike = double.Parse(
                cells[6].InnerText.Trim(),
                CultureInfo.InvariantCulture);

            double callSettle = Parse(cells[4]);
            if (callSettle > 0)
            {
                double vol = ImpliedVolatilityCalculator.SolveImpliedVolatility(
                    callSettle, forward, strike, timeToExpiry, 1.0, true);

                vols.Add(new StrikeToMarketVolatility(strike, vol));
            }
        }

        foreach(var vol in vols)
        {
            Console.WriteLine($"{vol.Strike}");
        }

        return vols;
    }

    private static double Parse(HtmlNode node)
    {
        var text = node.InnerText.Trim();
        return double.TryParse(text, NumberStyles.Any,
            CultureInfo.InvariantCulture, out var v)
            ? v : 0.0;
    }
}
