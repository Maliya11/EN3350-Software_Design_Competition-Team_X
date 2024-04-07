package com.project.backend;

public class QuestionUpdateRequest {

    //This class is used to handle the (id, selected_answer) given from the POST request(question/answer)
    private Integer id;
    private Integer selAns;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getSelAns() {
        return selAns;
    }

    public void setSelAns(Integer selAns) {
        this.selAns = selAns;
    }
}
