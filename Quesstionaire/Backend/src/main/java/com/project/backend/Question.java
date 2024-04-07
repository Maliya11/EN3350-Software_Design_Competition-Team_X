package com.project.backend;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.Data;

@Data
@Entity
public class Question {

    // This is the Question class, used Lombok here
    // Naming convention -> Camel case
    // ex: cor_ans column in database is corAns here
    // "id" is named as primary key

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    private String q;
    private String ans1;
    private String ans2;
    private String ans3;
    private String ans4;
    private Integer corAns;
    private String genFeed;
    private String feed1;
    private String feed2;
    private String feed3;
    private String feed4;
    private Integer selAns;

}
