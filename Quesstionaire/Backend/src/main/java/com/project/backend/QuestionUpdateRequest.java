package com.project.backend;

public class QuestionUpdateRequest {
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
