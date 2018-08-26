package com.example.hadas.myapplication1;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.annotation.RequiresApi;
import android.support.v4.app.NotificationCompat;
import android.widget.Toast;

public class PictureService extends Service {

    private BroadcastReceiver yourReceiver;
    private TcpAndroidConnection tcp;

    /**
     * on bind method
     * @param intent
     * @return
     */
    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    /**
     * on create method
     */
    @Override
    public void onCreate() {
        super.onCreate();
    }

    /**
     * onStartCommand method
     * @param intent
     * @param flags
     * @param startId
     * @return
     */
    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Toast.makeText(this,"service Started", Toast.LENGTH_LONG).show();

        final IntentFilter theFilter = new IntentFilter();
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");

        this.yourReceiver = new BroadcastReceiver() {
            @RequiresApi(api = Build.VERSION_CODES.O)
            @Override
            public void onReceive(Context context, Intent intent) {
                WifiManager wifiManager = (WifiManager)context.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);

                /*progress bar*/
                final NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
                NotificationChannel channel = new NotificationChannel("default",
                        "Channel name",
                        NotificationManager.IMPORTANCE_DEFAULT);
                channel.setDescription("Channel description");
                notificationManager.createNotificationChannel(channel);
                final NotificationCompat.Builder builder = new NotificationCompat.Builder(context, "default");
                builder.setSmallIcon(R.drawable.ic_launcher_background)
                        .setContentTitle("Transferring Images status")
                        .setContentText("In progress");


                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {
                        /*get the different network states*/
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {
                            tcp=new TcpAndroidConnection();
                            tcp.connectToService();
                            tcp.sendPhotos(notificationManager,builder); /*Starting the Transfer*/
                        }
                    }
                }
            }
        };
        this.registerReceiver(this.yourReceiver, theFilter);
        return START_STICKY;
    }

    @Override
    public void onDestroy() {
        tcp.closeConn();
        Toast.makeText(this,"service stopped", Toast.LENGTH_LONG).show();
    }



}
