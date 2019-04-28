using Android.Gms.Location;
using System.Text;

namespace GPS_test2
{
    internal class MyLocationCallBack : LocationCallback
    {
        private MainActivity mainActivity;

        public MyLocationCallBack(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public override void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);

            mainActivity.txt_location.Text = new StringBuilder(result.LastLocation.Latitude.ToString()).Append("/")
                .Append(result.LastLocation.Longitude.ToString()).ToString();
        }

    }
}