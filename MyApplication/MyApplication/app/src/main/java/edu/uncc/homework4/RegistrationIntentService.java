package edu.uncc.homework4;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.util.Log;

import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.google.android.gms.iid.InstanceID;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 * Created by Nitin on 11/19/2017.
 */

public class RegistrationIntentService extends IntentService {
    SharedPreferences pref;
    String access_token;
    public RegistrationIntentService(){
        super("");
    }

    /**
     * Creates an IntentService.  Invoked by your subclass's constructor.
     *
     * @param name Used to name the worker thread, important only for debugging.
     */
    public RegistrationIntentService(String name) {
        super(name);
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {
        pref = getApplicationContext().getSharedPreferences("token", Context.MODE_PRIVATE);
        access_token = pref.getString("token","");
        InstanceID instanceID = InstanceID.getInstance(this);
        String token = null;
        //int id = R.string.;
        try {
            token = instanceID.getToken(getApplicationContext().getString(R.string.gcm_defaultSenderId),
                    GoogleCloudMessaging.INSTANCE_ID_SCOPE, null);
        } catch (IOException e) {
            e.printStackTrace();
        }

        Request request = new Request.Builder()
                .url("http://careme-surveypart2.azurewebsites.net/api/Users")//+user.getSurveyGroupId())
                .header("Authorization", "Bearer "+access_token)//eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                .build();

        OkHttpClient client = new OkHttpClient();
        final String finalToken = token;
        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                String id = "";
                try {
                    JSONArray jsonArray = new JSONArray(response.body().string());
                    for(int i = 0; i < jsonArray.length(); i++){
                        JSONObject obj = jsonArray.getJSONObject(i);
                        id = obj.getString("Id");
                        SharedPreferences sharedPref = PreferenceManager.getDefaultSharedPreferences(getApplicationContext());   //getPreferences(Context.MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPref.edit();
                        editor.putString("userId",id);
                        //editor.putInt(getString(R.string.saved_high_score), newHighScore);
                        editor.commit();

                        Log.d("demo", "GCM Registration Token: " + finalToken);
                        //Log.d("demo", "id: " + id + " " );
                        sendDeviceID(finalToken, id);
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }


            }
        });


    }

    public void sendDeviceID(String s, String id){
        SharedPreferences pref = PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
        String access_token = pref.getString("token","");

        RequestBody formBody = new FormBody.Builder()
                .add("Id", id)
                .add("DeviceId", s)
                .build();

        Request request = new Request.Builder()
                .url("http://careme-surveypart2.azurewebsites.net/api/Users")//+user.getSurveyGroupId())
                .header("Authorization", "Bearer "+access_token)//eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                .post(formBody)
                .build();

        OkHttpClient client = new OkHttpClient();

        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {

            }
        });
    }
}
