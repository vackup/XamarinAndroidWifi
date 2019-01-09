using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace AndroidWifi
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private WifiManager wifiManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.activity_main);

            this.wifiManager = GetWifiManager();

            RegisterReceiver(new WifiMonitor(this.wifiManager), new IntentFilter(WifiManager.ScanResultsAvailableAction));
            
            var startScan = this.wifiManager.StartScan();

            this.ConnectToWifi("BebopDrone-007411", "");

        }

        private static WifiManager GetWifiManager()
        {
            return (WifiManager)Application.Context.GetSystemService(WifiService);
        }

        private void ConnectToWifi(string ssid, string password)
        {
            var formattedSsid = $"\"{ssid}\"";
            var formattedPassword = $"\"{password}\"";

            var wifiConfig = new WifiConfiguration
            {
                Ssid = formattedSsid,
                AllowedKeyManagement = new BitSet((int)AuthAlgorithmType.Open)
                //PreSharedKey = formattedPassword
            };

            // This code add a new network ...
            var addNetwork = this.wifiManager.AddNetwork(wifiConfig);
            var configNetworkd = this.wifiManager.ConfiguredNetworks.Select(n => n.Ssid).ToList();

            // ... but in this case, we look for it in the Configured Networks
            var network = this.wifiManager.ConfiguredNetworks
                .FirstOrDefault(n => n.Ssid == formattedSsid);

            if (network == null)
            {
                Console.WriteLine($"Cannot connect to network: {ssid}");
                return;
            }

            this.wifiManager.Disconnect();
            var enableNetwork = this.wifiManager.EnableNetwork(network.NetworkId, true);
        }

        public void DisplayText(string text)
        {
            this.FindViewById<TextView>(Resource.Id.txtScanResults).Text = "Wifi networks: \r\n" + text;
        }
    }
}

