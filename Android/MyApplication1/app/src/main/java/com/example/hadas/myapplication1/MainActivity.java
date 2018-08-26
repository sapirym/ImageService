package com.example.hadas.myapplication1;

import android.app.NotificationManager;
import android.content.Context;
import android.content.Intent;
import android.support.v4.app.NotificationCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

public class MainActivity extends AppCompatActivity {

    /**
     * on create methood
     * @param savedInstanceState
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    /**
     * start the service
     * @param view
     */
    public void StartService(View view){
        Intent intent = new Intent(this,PictureService.class);
        startService(intent);
    }

    /**
     * stop the service
     * @param view
     */
    public void StopService(View view){
        Intent intent = new Intent(this,PictureService.class);
        stopService(intent);
    }
}