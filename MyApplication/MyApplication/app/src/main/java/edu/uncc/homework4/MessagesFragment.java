package edu.uncc.homework4;

import android.content.Context;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

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
public class MessagesFragment extends Fragment {

    MessagesRecyclerAdapter messagesRecyclerAdapter;
    ArrayList<String> messagesList;
    RecyclerView messagesView;

    private OnFragmentInteractionListener mListener;

    public MessagesFragment() {
        // Required empty public constructor
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
        String access_token = sharedPref.getString("token","");
        OkHttpClient client = new OkHttpClient();

        RequestBody formBody = new FormBody.Builder()
                .add("userId", "57aeef8f-3140-4295-87eb-8f304cd4cfe6")
                .build();

        /*Request request = new Request.Builder()
                .url("http://caremesurvey.azurewebsites.net/api/Surveys?studyGroupId=1")
                //.header("Content-Type","application/x-www-form-urlencoded")
                .addHeader("Authorization","Bearer "+access_token)
                //.header("Authorization","Bearer "+access_token)
                //.post(formBody)
                .build();*/



        Request request = new Request.Builder()
                .url("http://caremesurvey.azurewebsites.net/api/Surveys?studyGroupId=1")
                .header("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjU3YWVlZjhmLTMxNDAtNDI5NS04N2ViLThmMzA0Y2Q0Y2ZlNiIsInN1YiI6InVzZXIxIiwicm9sZSI6IlVzZXIiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAvIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEiLCJleHAiOjE1MTA5NjQwMjUsIm5iZiI6MTUxMDg3NzYyNX0.y7Iyynudl27Ov-zMQ-MzC7KWOGG59o77Dhi55SRVXDk")
                .build();



        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                Log.d("demo","failure");
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {

                Log.d("demo","success");
                    messagesList = new ArrayList<>();
                    //messagesList = response.body().string();
                    //Log.d("demo", response.body().string());
                final String myResponse = response.body().string();

//                String quest = response.body().string();
               // messagesList.add();
                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {

                        try {
                            JSONArray jsonArray= new JSONArray(myResponse);
                            for(int i = 0; i< jsonArray.length(); i++) {
                                JSONObject jsonO = jsonArray.getJSONObject(i);
                                if (jsonO.has("QuestionText")) {
                                    //Log.d()
                                    messagesList.add(jsonO.getString("QuestionText"));
                                }
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }


                        messagesRecyclerAdapter = new MessagesRecyclerAdapter(messagesList, getContext());

                        messagesView = (RecyclerView) getActivity().findViewById(R.id.recyclerViewResult);

                        messagesView.setAdapter(messagesRecyclerAdapter);
                        messagesView.setLayoutManager(new LinearLayoutManager(getContext()));
                    }
                });


            }
        });


        //messagesRecyclerAdapter.notifyDataSetChanged();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
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
