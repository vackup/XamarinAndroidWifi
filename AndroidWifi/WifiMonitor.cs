using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Net.Wifi;

namespace AndroidWifi
{
    public class WifiMonitor : BroadcastReceiver
    {
        private readonly WifiManager wifiManager;

        public WifiMonitor(WifiManager wifiManager) : base()
        {
            this.wifiManager = wifiManager;
        }

        public override async void OnReceive(Context context, Intent intent)
        {
            var mainActivity = (MainActivity) context;

            var success = intent.GetBooleanExtra(WifiManager.ExtraResultsUpdated, false);

            var message = string.Join("\r\n", this.wifiManager.ScanResults
                .Select(r => $"{r.Ssid} ({r.Bssid}) - {r.Level} dB"));

            mainActivity.DisplayText(message);

            var bebotWifi = this.wifiManager.ScanResults.FirstOrDefault(r => r.Ssid.StartsWith("Bebop"));

            await Task.Delay(TimeSpan.FromSeconds(5));

            this.wifiManager.StartScan();
        }
    }
}