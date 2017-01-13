using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Rdio.Service
{
    public class LequeService
    {
        public async Task<Models.Legue.Varzesh3Legue> GetFootbalLegue(int FootbalLegueId)
        {
            string Url = string.Empty;
            switch (FootbalLegueId)
            {
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarIran:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/173";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Azadegan:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/191";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarEnglish:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/177";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Laliga:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/183";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.SerieA:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/188";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Leshampione:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/179";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarFootsalIran:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/192";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Bondes:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/178";
                    break;
                default:
                    break;
            }

            string htmlContent = "";
            var uri = new Uri(Url);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0");
                client.DefaultRequestHeaders.Add("Host", uri.Authority);
                using (var r = await client.GetAsync(uri))
                {
                    htmlContent = await r.Content.ReadAsStringAsync();
                }
            }
            return Util.Common.fromJSON<Models.Legue.Varzesh3Legue>(htmlContent);
        }
        public async Task<Models.Legue.Varzesh3LegueFixture> GetFootbalLegueFixture(int FootbalLegueId)
        {
            string Url = string.Empty;
            switch (FootbalLegueId)
            {
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarIran:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/172";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Azadegan:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/189";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarEnglish:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/174";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Laliga:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/180";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.SerieA:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/187";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Leshampione:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/182";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.BartarFootsalIran:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/190";
                    break;
                case (int)Rdio.Util.Configuration.FootbalLegue.Bondes:
                    Url = "http://api.varzesh3.com/v0.2/leaguestat/widget/5/181";
                    break;
                default:
                    break;
            }

            string htmlContent = "";
            var uri = new Uri(Url);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0");
                client.DefaultRequestHeaders.Add("Host", uri.Authority);
                using (var r = await client.GetAsync(uri))
                {
                    htmlContent = await r.Content.ReadAsStringAsync();
                }
            }
            return Util.Common.fromJSON<Models.Legue.Varzesh3LegueFixture>(htmlContent);
        }

    }
}