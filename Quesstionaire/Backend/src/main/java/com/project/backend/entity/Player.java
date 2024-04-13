package com.project.backend.entity;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.Data;

@Data
@Entity
public class Player {

    @Id
    private String apiKey;
    private String playerState;
    private Integer q1Ans;
    private Integer q2Ans;
    private Integer q3Ans;
    private Integer q4Ans;
    private Integer q5Ans;
    private Integer q6Ans;
    private Integer q7Ans;
    private Integer q8Ans;
    private Integer q9Ans;
    private Integer q10Ans;
    private Integer marks;
    private Integer currentQuestion;




}
