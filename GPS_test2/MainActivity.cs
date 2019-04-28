using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Com.Karumi.Dexter;
using Android;
using Com.Karumi.Dexter.Listener.Single;
using Com.Karumi.Dexter.Listener;
using System;
using Android.Support.Design.Widget;
using Android.Gms.Location;
using Android.Support.V4.App;

namespace GPS_test2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btn_start, btn_stop;
        public TextView txt_location;
        RelativeLayout root_view;

        FusedLocationProviderClient fusedLocationProviderClient;
        LocationRequest locationRequest;
        LocationCallback locationCallback;

        private class SamplePermissionListener : Java.Lang.Object, IPermissionListener
        {

            MainActivity activity;

            public SamplePermissionListener(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnPermissionDenied(PermissionDeniedResponse p0)
            {
                Snackbar.Make(activity.root_view, "Permission Denied", Snackbar.LengthShort).Show();
            }

            public void OnPermissionGranted(PermissionGrantedResponse p0)
            {
                Snackbar.Make(activity.root_view, "Permission Granted", Snackbar.LengthShort).Show();
            }

            public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken token)
            {
                activity.ShowRequestPermissionRationale(token);
            }
        }

        private void ShowRequestPermissionRationale(IPermissionToken token)
        {
            new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("we need this permission")
                .SetMessage("This permission needed for doing some fancy stuff so please , allow it !")
                .SetNegativeButton("Cancel", delegate
                {
                    token.ContinuePermissionRequest();

                })
                .SetPositiveButton("OK", delegate
                {
                    token.ContinuePermissionRequest();

                })
                .SetOnDismissListener(new MyDismissListener(token)).Show();

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            root_view = FindViewById<RelativeLayout>(Resource.Id.root_view);

            btn_start = FindViewById<Button>(Resource.Id.btn_start);
            btn_stop = FindViewById<Button>(Resource.Id.btn_stop);

            txt_location = FindViewById<TextView>(Resource.Id.txt_location);

            //퍼미션
            Dexter.WithActivity(this)
                .WithPermission(Manifest.Permission.AccessFineLocation)
                .WithListener(new CompositePermissionListener(new SamplePermissionListener(this))).Check();

            if (!ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
            {
                BuildLocationRequest();
                BuildLocationCallBack();

                fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

                btn_start.Click += delegate
                {
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
                        return;
                    fusedLocationProviderClient.RequestLocationUpdates(locationRequest, locationCallback, Looper.MyLooper());

                    btn_start.Enabled = !btn_start.Enabled;
                    btn_stop.Enabled = !btn_stop.Enabled;
                };

                btn_stop.Click += delegate
                {
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
                        return;
                    fusedLocationProviderClient.RemoveLocationUpdates(locationCallback);

                    btn_start.Enabled = !btn_start.Enabled;
                    btn_stop.Enabled = !btn_stop.Enabled;

                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
           if (!btn_start.Enabled)
            {
                if (fusedLocationProviderClient != null)
                    fusedLocationProviderClient.RequestLocationUpdates(locationRequest, locationCallback, Looper.MyLooper());
            }
        }

        protected override void OnStart()
        {
            if (fusedLocationProviderClient != null)
                fusedLocationProviderClient.RemoveLocationUpdates(locationCallback);
            base.OnStart();
        }

        private void BuildLocationCallBack()
        {
            locationCallback = new MyLocationCallBack(this);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            locationRequest.SetInterval(5000);
            locationRequest.SetFastestInterval(3000);
            locationRequest.SetSmallestDisplacement(10f);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}