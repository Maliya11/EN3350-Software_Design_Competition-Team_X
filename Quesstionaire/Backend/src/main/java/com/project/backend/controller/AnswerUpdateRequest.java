package com.project.backend.controller;

public class AnswerUpdateRequest {
    private Integer qNum;
    private Integer selAns;

    public Integer getqNum() {
        return qNum;
    }

    public void setqNum(Integer qNum) {
        this.qNum = qNum;
    }

    public Integer getSelAns() {
        return selAns;
    }

    public void setSelAns(Integer selAns) {
        this.selAns = selAns;
    }
}
