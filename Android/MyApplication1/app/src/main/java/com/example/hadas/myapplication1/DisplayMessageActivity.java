package com.example.hadas.myapplication1;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

public class DisplayMessageActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //setContentView(R.layout.activity_display_message);
        Intent intent  =  getIntent();
        String msg  =  intent.getStringExtra("message");
        //  TextView textView  =  (TextView)  findViewById(R.id.textViewMsg); textView.setText(msg);

    }
}