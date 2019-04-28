using Android.Content;
using Com.Karumi.Dexter;

namespace GPS_test2
{
    internal class MyDismissListener : Java.Lang.Object,IDialogInterfaceOnDismissListener
    {
        private IPermissionToken token;

        public MyDismissListener(IPermissionToken token)
        {
            this.token = token;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            token.CancelPermissionRequest();
        }
    }
}