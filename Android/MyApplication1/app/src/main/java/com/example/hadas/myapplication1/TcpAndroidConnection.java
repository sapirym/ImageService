package com.example.hadas.myapplication1;

import android.app.NotificationManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Environment;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;

public class TcpAndroidConnection {
    //Members
    private Socket socket;
    private OutputStream outputStream;

    /**
     * constructor
     */
    public TcpAndroidConnection(){}

    /**
     * connect to service
     */
    public void connectToService(){

        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    InetAddress serverAddr = InetAddress.getByName("10.0.2.2");
                    socket = new Socket(serverAddr, 8001);
                    try {
                        outputStream = socket.getOutputStream();
                    } catch (Exception e) {
                        Log.e("TCP", "the socket not connect:", e);
                    }
                } catch (Exception e) {
                    Log.e("TCP", "the create socket fail:", e);
                }
            }
        });
        thread.start();
    }

    /**
     * send photo to tcp service
     * @param notificationManager
     * @param builder
     */
    public void sendPhotos(final NotificationManager notificationManager, final NotificationCompat.Builder builder){

        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                //get the file from the camera
                File dcim = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM), "Camera");
                if (dcim == null) {
                    return;
                }
                //get list of pictures
                File[] files = dcim.listFiles();
                double picNum = files.length;
                double count = 0;

                if (files != null) {
                    for (File file : files) {
                        try {
                            FileInputStream fis = new FileInputStream(file);
                            Bitmap bm = BitmapFactory.decodeStream(fis);
                            byte[] imgByte = getBytesFromBitmap(bm);
                            try {
                                //write the length of the specific photo
                                int imageLen = imgByte.length;
                                String sendImageLen = imageLen + "";
                                outputStream.write(sendImageLen.getBytes(), 0, sendImageLen.getBytes().length);
                                outputStream.flush();
                                Thread.sleep(120);

                                //write the name of the specific photo
                                String fileName = file.getName();
                                outputStream.write(fileName.getBytes(), 0, fileName.getBytes().length);
                                outputStream.flush();
                                Thread.sleep(120);

                                outputStream.write(imgByte, 0, imageLen);
                                outputStream.flush();
                                Thread.sleep(600);
                            } catch (Exception e) {
                                Log.e("TCP", "fail to write", e);
                            }
                        } catch (Exception e) {
                            Log.e("TCP", "can't FileInputStream", e);
                        }
                        count++;
                        int progBar = (int) ((count/picNum)*100);
                        String msg = progBar +"%";
                        builder.setProgress(100, progBar, false).setContentText(msg);
                        notificationManager.notify(1, builder.build());
                    }
                    try {
                        String toSend = "Close\n";
                        outputStream.write(toSend.getBytes(), 0, toSend.getBytes().length);
                        outputStream.flush();
                        builder.setContentTitle("finished the Transportation.").
                        setContentText("the photos were sent successfully!");
                        notificationManager.notify(1, builder.build());
                    } catch (Exception e) {
                        Log.e("TCP", "fail to write:", e);
                        builder.setContentTitle("Error").
                        setContentText("Error on sending the photos");
                        notificationManager.notify(1, builder.build());
                    }
                }
            }
        });
        thread.start();
    }

    /**
     * get Bytes From Bitmap method
     * @param bitmap
     * @return byte[]
     */
    public byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }

    /**
     * close the connection
     */
    public void closeConn(){
        try{
            this.socket.close();
        }catch (IOException e){
            Log.e("TCP","Error on closing the socket..", e);
        }
    }
}
