package edu.uncc.homework4;

import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.preference.PreferenceManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;

import java.io.IOException;
import java.util.Date;
import java.util.List;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 * Created by Nitin on 11/14/2017.
 */

public class MessagesRecyclerAdapter extends RecyclerView.Adapter<MessagesRecyclerAdapter.ViewHolder>{


    private OnItemClickListener listener;
    public boolean editMode = true;

    public interface OnItemClickListener {
        void onItemClick(View itemView, int position);
    }
    public void setOnItemClickListener(OnItemClickListener listener) {
        this.listener = listener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {

        public TextView message;
        public Button sendResponse;
        public RadioButton btnYes;
        public RadioButton btnNo;
        public RadioGroup rgResponse;



        Context vContext;

        public ViewHolder(Context context,final View itemView) {
            super(itemView);

            message = (TextView) itemView.findViewById(R.id.textViewMessage);
            sendResponse = (Button) itemView.findViewById(R.id.buttonSendResponse);
            btnYes = (RadioButton) itemView.findViewById(R.id.radioButtonYes);
            btnNo = (RadioButton) itemView.findViewById(R.id.radioButtonNo);
            rgResponse = (RadioGroup) itemView.findViewById(R.id.rgChoice);

            vContext = context;
            SharedPreferences sharedPref =   PreferenceManager.getDefaultSharedPreferences(mContext);// mContext.getSharedPreferences("token",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
            final String access_token = sharedPref.getString("token","");

            SharedPreferences sharedPrefUser = PreferenceManager.getDefaultSharedPreferences(mContext);   //getPreferences(Context.MODE_PRIVATE);
            Gson gson = new Gson();
            final User user = gson.fromJson(sharedPrefUser.getString("user",""),User.class);

            if (sendResponse.isClickable()) {
                sendResponse.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                      //  if (listener != null) {
                        String responseText = "";
                        if(btnYes.isChecked()){responseText = "Yes";}else{responseText = "No";}
                            final int position = getAdapterPosition();
                            if (position != RecyclerView.NO_POSITION) {
                                //listener.onItemClick(itemView, position);
                                sendResponse.setEnabled(false);
                                /*RequestBody formBody = new FormBody.Builder()

                                        .build();*/

                                RequestBody formBody = new FormBody.Builder()
                                        .add("UserId", messages.get(getAdapterPosition()).getUserId())
                                        .add("StudyGroupId", messages.get(getAdapterPosition()).getStudyGrpId())
                                        .add("SurveyId", messages.get(getAdapterPosition()).getSurveyId())
                                        .add("UserResponseText",responseText )
                                        .add("SurveyResponseReceivedTime", (new Date()).toString())
                                        .build();

                                Request request = new Request.Builder()
                                        .url("http://careme-surveypart2.azurewebsites.net/api/SurveyResponses")
                                        .header("Content-Type","application/x-www-form-urlencoded")
                                        .header("Authorization", "Bearer "+access_token)
                                        .post(formBody)
                                        .post(formBody)
                                        .build();

                                /*Request request = new Request.Builder()
                                        .url("http://careme-surveypart2.azurewebsites.net/api/SurveyResponses")//+user.getSurveyGroupId())
                                        .header("Authorization", "Bearer "+access_token)//eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                                        .build();*/

                                OkHttpClient client = new OkHttpClient();
                                client.newCall(request).enqueue(new Callback() {
                                    @Override
                                    public void onFailure(Call call, IOException e) {
                                        Log.d("demo","response failure");
                                       // Toast.makeText(getContext(),"Error in sending response",Toast.LENGTH_SHORT);
                                    }

                                    @Override
                                    public void onResponse(Call call, Response response) throws IOException {
                                       // Toast.makeText(getContext(),"Response sent successfully !!",Toast.LENGTH_SHORT);
                                        listener.onItemClick(itemView,position);
                                        Log.d("demo","response success");
                                        //itemView.setEnabled(false);
                                        if(rgResponse.getCheckedRadioButtonId() == R.id.radioButtonYes){
                                            //btnYes.setBackgroundColor(Color.GREEN);
                                        }else if(rgResponse.getCheckedRadioButtonId() == R.id.radioButtonNo){
                                            //btnNo.setBackgroundColor(Color.GREEN);
                                        }
                                    }
                                });
                            }
                        //}
                    }
                });
            }

            /*itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {

                }
            });*/
        }

    }

    List<SurveyQuestion> messages;
    Context mContext;

    public MessagesRecyclerAdapter(List<SurveyQuestion> messages, Context mContext) {
        this.messages = messages;
        this.mContext = mContext;
    }

    private Context getContext() {
        return mContext;
    }

    @Override
    public MessagesRecyclerAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View contactView = inflater.inflate(R.layout.messages_layout, parent, false);
        ViewHolder viewHolder = new ViewHolder(getContext(),contactView);

        return viewHolder;
    }

    @Override
    public void onBindViewHolder(MessagesRecyclerAdapter.ViewHolder holder, int position) {
        //MusicTrack track = tracks.get(position);
        String message = messages.get(position).getQuestion();
        holder.message.setText(message);
        if (messages.get(position).getQuestion().equals("")){

        }else if(messages.get(position).getResponse().equals("No")){
            holder.rgResponse.setEnabled(false);
            holder.btnNo.setChecked(true);
            holder.btnNo.setBackgroundColor(Color.GREEN);
            holder.sendResponse.setEnabled(false);
        }else if(messages.get(position).getResponse().equals("Yes")){
            holder.rgResponse.setEnabled(false);
            holder.btnYes.setChecked(true);
            holder.btnYes.setBackgroundColor(Color.GREEN);
            holder.sendResponse.setEnabled(false);
        }

        //holder.itemView.setEnabled(editMode);

    }

    @Override
    public int getItemCount() {
        return messages.size();
    }


}

