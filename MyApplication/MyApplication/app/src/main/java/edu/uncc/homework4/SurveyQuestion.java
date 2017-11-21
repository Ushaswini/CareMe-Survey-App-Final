package edu.uncc.homework4;

import java.io.Serializable;

/**
 * Created by Nitin on 11/20/2017.
 */

public class SurveyQuestion implements Serializable {

    String question;
    String response;

    public SurveyQuestion() {
    }

    public String getQuestion() {
        return question;
    }

    public void setQuestion(String question) {
        this.question = question;
    }

    public String getResponse() {
        return response;
    }

    public void setResponse(String response) {
        this.response = response;
    }
}
