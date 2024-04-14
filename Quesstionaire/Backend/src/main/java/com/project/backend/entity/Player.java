package com.project.backend.entity;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.Data;

@Data
@Entity
public class Player {

    @Id
    private String apiKey;
    private Integer playerState;
    private Integer q1_ans;
    private Integer q2_ans;
    private Integer q3_ans;
    private Integer q4_ans;
    private Integer q5_ans;
    private Integer q6_ans;
    private Integer q7_ans;
    private Integer q8_ans;
    private Integer q9_ans;
    private Integer q10_ans;
    private Integer marks;
    private Integer completedQuestions;




}
