using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;

namespace BugHomeButtonMediaPlugin.Droid
{
    [Service]
    public class LongRunningTaskService : Service
    {
        private Notification.Builder _noteBuilder;

        public override void OnCreate()
        {
            base.OnCreate();
            BuildNotification();
        }

        private void BuildNotification()
        {
            if (_noteBuilder != null) return;

            var contentIntent = new Intent(this, typeof(MainActivity));
            contentIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            var pending = PendingIntent.GetActivity(this, 0, contentIntent, PendingIntentFlags.UpdateCurrent);

            _noteBuilder = new Notification.Builder(ApplicationContext);

            _noteBuilder
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon))
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle("Test")
                .SetContentText("Test")
                .SetOngoing(true)
                .SetPriority((int) NotificationPriority.Max)
                .SetStyle(new Notification.BigTextStyle(_noteBuilder))
                .SetContentIntent(pending);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                _noteBuilder.SetCategory(Notification.CategoryService)
                    .SetVisibility(NotificationVisibility.Private);
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var bundle = intent?.Extras;
            var commandType = bundle?.GetInt("CommandType", 0);
            if (commandType == 1)
            {
                StartForeground(NotificationID, new Notification());
                UpdateNotification();
            }

            return StartCommandResult.Sticky;
        }

        private void UpdateNotification()
        {
            _noteBuilder.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis());
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(NotificationID, _noteBuilder.Build());
        }

        public int NotificationID = 1;

        public override void OnDestroy()
        {
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CancelAll();
            StopForeground(true);
            base.OnDestroy();
        }
    }
}