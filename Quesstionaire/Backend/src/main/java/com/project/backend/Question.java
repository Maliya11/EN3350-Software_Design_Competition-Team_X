package com.project.backend;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.Data;

@Data
@Entity
public class Question {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE)
    private int id;
    private String question;
    private String qans1;
    private String qans2;
    private String qans3;
    private String qans4;
    private String cor_ans;
    private String gen_feed;
    private String feed1;
    private String feed2;
    private String feed3;
    private String feed4;
}
