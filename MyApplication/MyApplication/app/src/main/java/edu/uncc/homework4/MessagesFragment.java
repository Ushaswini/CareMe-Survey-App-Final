package edu.uncc.homework4;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.gson.Gson;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link MessagesFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 */
public class MessagesFragment extends Fragment implements MessagesRecyclerAdapter.OnItemClickListener {

    MessagesRecyclerAdapter messagesRecyclerAdapter;
    ArrayList<String> messagesList;
    ArrayList<SurveyQuestion> surveyQuestionArrayList;
    RecyclerView messagesView;
    Activity myActivity;

    private OnFragmentInteractionListener mListener;

    public MessagesFragment() {
        // Required empty public constructor
    }

public void getMyActivity(Activity activity){
    this.myActivity = activity;
}
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment

        return inflater.inflate(R.layout.fragment_messages, container, false);

    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        /*if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }*/

    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        SharedPreferences sharedPref = getActivity().getSharedPreferences("token",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
        //SharedPreferences.Editor editor = sharedPref.edit();
        //editor.  .putString("token",access_token);
        //editor.putInt(getString(R.string.saved_high_score), newHighScore);
        //editor.commit();
        final String access_token = sharedPref.getString("token","");
        //final String userId = sharedPref.getString("userId", "");
        final Request requestUserInfo = new Request.Builder()
                //.url("http://careme-surveypart2.azurewebsites.net/api/Users")
                .url("http://careme-surveypart2.azurewebsites.net/api/Account/UserInfo?token="+access_token)
                //.header("Authorization", "Bearer "+ access_token)// eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                //.header("Content-Type","application/x-www-form-urlencoded")
                .build();
        OkHttpClient client1 = new OkHttpClient();
        client1.newCall(requestUserInfo).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                OkHttpClient client = new OkHttpClient();

                //Gson gson = new Gson();

                //User user = gson.fromJson(response.body().string(), User.class);

                /*SharedPreferences sharedPref = getActivity().getSharedPreferences("user",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = sharedPref.edit();
                editor.putString("user",gson.toJson(user));*/

                String userId=response.body().string();
                try {
                    Log.d("demo","responsebody: " + userId);
                    //JSONArray jsonArray = new JSONArray(userId);
                    //for(int i = 0; i < jsonArray.length(); i++){
                        JSONObject js = new JSONObject(userId);
                        userId = js.getString("Id");
                    //}
                } catch (JSONException e) {
                    e.printStackTrace();
                }

                //SharedPreferences pref = getActivity().getSharedPreferences("userId",Context.MODE_PRIVATE);
                //String userId = pref.getString("userId","");

                /*SharedPreferences sharedPref1 = getActivity().getSharedPreferences("userId",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
                String userId = sharedPref1.getString("userId","");*/
                Log.d("demo","user " +userId);
                Request request = new Request.Builder()
                        .url("http://careme-surveypart2.azurewebsites.net/api/Surveys/GetSurvey?userId="+userId)
                        .header("Authorization", "Bearer "+access_token)//eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                        .build();


                final String finalUserId = userId;
                Log.d("demo","final" + finalUserId);
                client.newCall(request).enqueue(new Callback() {
                    @Override
                    public void onFailure(Call call, IOException e) {
                        Log.d("demo","failure");
                    }

                    @Override
                    public void onResponse(Call call, Response response) throws IOException {

                        Log.d("demo","success");
                        messagesList = new ArrayList<>();
                        surveyQuestionArrayList = new ArrayList<SurveyQuestion>();
                        //messagesList = response.body().string();
                        //Log.d("demo", response.body().string());
                        final String myResponse = response.body().string();
                        Log.d("demo",myResponse + " hello " + messagesList);
//                String quest = response.body().string();
                        // messagesList.add();
                        getActivity().runOnUiThread(new Runnable() {
                            @Override
                            public void run() {

                                try {
                                    JSONObject jsonObject = new JSONObject(myResponse);
                                    JSONArray jsonArray= jsonObject.getJSONArray("SurveysResponded");
                                    for(int i = 0; i< jsonArray.length(); i++) {
                                        JSONObject jsonO = jsonArray.getJSONObject(i);
                                       // if (jsonO.has("QuestionText")) {
                                            SurveyQuestion sq = new SurveyQuestion();
                                            sq.setQuestion(jsonO.getString("QuestionText"));
                                            sq.setResponse(jsonO.getString("ResponseText"));
                                            sq.setSurveyId(jsonO.getString("SurveyId"));
                                        sq.setUserId(finalUserId);
                                        sq.setQuesType(jsonO.getInt("QuestionType"));
                                        sq.setSurveyTime(jsonO.getString("ResponseReceivedTime"));
                                        //sq.setStudyGrpId(jsonO.getString("StudyGroupId"));
                                        //sq.setResponseDate(jsonO.getString(""));
                                            //Log.d()
                                            //messagesList.add(jsonO.getString("QuestionText"));
                                            surveyQuestionArrayList.add(sq);
                                    }
                                    JSONArray jsonArray1= jsonObject.getJSONArray("Surveys");
                                    for(int i = 0; i< jsonArray1.length(); i++) {
                                        JSONObject jsonO = jsonArray1.getJSONObject(i);
                                        // if (jsonO.has("QuestionText")) {
                                        SurveyQuestion sq = new SurveyQuestion();
                                        sq.setQuestion(jsonO.getString("QuestionText"));
                                        sq.setSurveyId(jsonO.getString("SurveyId"));
                                        sq.setStudyGrpId(jsonO.getString("StudyGroupId"));
                                        sq.setUserId(finalUserId);
                                        sq.setQuesType(jsonO.getInt("QuestionType"));
                                        sq.setSurveyTime(jsonO.getString("SurveyCreatedTime"));
                                        sq.setResponse("");
                                        //Log.d()
                                        //messagesList.add(jsonO.getString("QuestionText"));
                                        surveyQuestionArrayList.add(sq);
                                    }
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }

                                Log.d("demo","success adapter");
                                messagesRecyclerAdapter = new MessagesRecyclerAdapter(surveyQuestionArrayList, getContext());
                                messagesRecyclerAdapter.setOnItemClickListener(MessagesFragment.this);
                                messagesView = (RecyclerView) getActivity().findViewById(R.id.recyclerViewResult);

                                messagesView.setAdapter(messagesRecyclerAdapter);
                                messagesView.setLayoutManager(new LinearLayoutManager(getContext()));
                            }
                        });


                    }
                });


            }
        });




        /*Request request = new Request.Builder()
                .url("http://caremesurvey.azurewebsites.net/api/Surveys?studyGroupId=1")
                //.header("Content-Type","application/x-www-form-urlencoded")
                .addHeader("Authorization","Bearer "+access_token)
                //.header("Authorization","Bearer "+access_token)
                //.post(formBody)
                .build();*/





        //messagesRecyclerAdapter.notifyDataSetChanged();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    @Override
    public void onItemClick(View itemView, int position) {
//        itemView.setEnabled(false);
        SharedPreferences sharedPref = getActivity().getSharedPreferences("token",Context.MODE_PRIVATE);
        final String access_token = sharedPref.getString("token","");
        //final String userId = sharedPref.getString("userId", "");
        final Request requestUserInfo = new Request.Builder()
                //.url("http://careme-surveypart2.azurewebsites.net/api/Account/Users")
                .url("http://careme-surveypart2.azurewebsites.net/api/Account/UserInfo?token="+access_token)
                //.header("Authorization", "Bearer "+ access_token)// eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                .build();
        OkHttpClient client1 = new OkHttpClient();
        client1.newCall(requestUserInfo).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                OkHttpClient client = new OkHttpClient();

                //Gson gson = new Gson();

                //User user = gson.fromJson(response.body().string(), User.class);

                /*SharedPreferences sharedPref = getActivity().getSharedPreferences("user",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = sharedPref.edit();
                editor.putString("user",gson.toJson(user));*/
                String userId="";
                try {
                    //JSONArray jsonArray = new JSONArray(response.body().string());
                    //for(int i = 0; i < jsonArray.length(); i++){
                        JSONObject js = new JSONObject(response.body().string());
                        userId = js.getString("Id");
                    //}
                } catch (JSONException e) {
                    e.printStackTrace();
                }

                //SharedPreferences pref = getActivity().getSharedPreferences("userId",Context.MODE_PRIVATE);
                //String userId = pref.getString("userId","");

                /*SharedPreferences sharedPref1 = getActivity().getSharedPreferences("userId",Context.MODE_PRIVATE);   //getPreferences(Context.MODE_PRIVATE);
                String userId = sharedPref1.getString("userId","");*/

                Request request = new Request.Builder()
                        .url("http://careme-surveypart2.azurewebsites.net/api/Surveys/GetSurvey?userId="+userId)
                        .header("Authorization", "Bearer "+access_token)//eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTEyMjkwNTUsIm5iZiI6MTUxMTE0MjY1NX0._c9mA6bFl09xY_vB1Z8iqIYueFKuEfXlzj8J6Os9MtE")
                        .build();


                final String finalUserId = userId;
                client.newCall(request).enqueue(new Callback() {
                    @Override
                    public void onFailure(Call call, IOException e) {
                        Log.d("demo","failure");
                    }

                    @Override
                    public void onResponse(Call call, Response response) throws IOException {

                        Log.d("demo","success");
                        messagesList = new ArrayList<>();
                        surveyQuestionArrayList = new ArrayList<SurveyQuestion>();
                        //messagesList = response.body().string();
                        //Log.d("demo", response.body().string());
                        final String myResponse = response.body().string();
                        Log.d("demo",myResponse + " hello " + messagesList);
//                String quest = response.body().string();
                        // messagesList.add();
                        getActivity().runOnUiThread(new Runnable() {
                            @Override
                            public void run() {

                                try {
                                    JSONObject jsonObject = new JSONObject(myResponse);
                                    JSONArray jsonArray= jsonObject.getJSONArray("SurveysResponded");
                                    for(int i = 0; i< jsonArray.length(); i++) {
                                        JSONObject jsonO = jsonArray.getJSONObject(i);
                                        // if (jsonO.has("QuestionText")) {
                                        SurveyQuestion sq = new SurveyQuestion();
                                        sq.setQuestion(jsonO.getString("QuestionText"));
                                        sq.setResponse(jsonO.getString("ResponseText"));
                                        sq.setSurveyId(jsonO.getString("SurveyId"));
                                        sq.setUserId(finalUserId);
                                        sq.setQuesType(jsonO.getInt("QuestionType"));
                                        sq.setSurveyTime(jsonO.getString("ResponseReceivedTime"));
                                        //sq.setStudyGrpId(jsonO.getString("StudyGroupId"));
                                        //sq.setResponseDate(jsonO.getString(""));
                                        //Log.d()
                                        //messagesList.add(jsonO.getString("QuestionText"));
                                        surveyQuestionArrayList.add(sq);
                                    }
                                    JSONArray jsonArray1= jsonObject.getJSONArray("Surveys");
                                    for(int i = 0; i< jsonArray1.length(); i++) {
                                        JSONObject jsonO = jsonArray1.getJSONObject(i);
                                        // if (jsonO.has("QuestionText")) {
                                        SurveyQuestion sq = new SurveyQuestion();
                                        sq.setQuestion(jsonO.getString("QuestionText"));
                                        sq.setSurveyId(jsonO.getString("SurveyId"));
                                        sq.setStudyGrpId(jsonO.getString("StudyGroupId"));
                                        sq.setUserId(finalUserId);
                                        sq.setQuesType(jsonO.getInt("QuestionType"));
                                        sq.setSurveyTime(jsonO.getString("SurveyCreatedTime"));
                                        sq.setResponse("");
                                        //Log.d()
                                        //messagesList.add(jsonO.getString("QuestionText"));
                                        surveyQuestionArrayList.add(sq);
                                    }
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }

                                Log.d("demo","success adapter");
                                messagesRecyclerAdapter = new MessagesRecyclerAdapter(surveyQuestionArrayList, getContext());
                                messagesRecyclerAdapter.setOnItemClickListener(MessagesFragment.this);
                                messagesView = (RecyclerView) getActivity().findViewById(R.id.recyclerViewResult);

                                messagesView.setAdapter(messagesRecyclerAdapter);
                                messagesView.setLayoutManager(new LinearLayoutManager(getContext()));
                            }
                        });


                    }
                });


            }
        });


    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}
