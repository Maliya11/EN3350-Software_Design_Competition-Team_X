package com.project.backend.controller;

import com.project.backend.entity.Question;
import com.project.backend.service.QuestionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("question")
@CrossOrigin
public class QuestionController {

    //This is the controller class where http requests are handled

    @Autowired
    QuestionService questionService;

    //To get all the questions from the database in JASON format
    @RequestMapping("allQuestions")
    public List<Question> getAllQuestions(){
        return questionService.getAllQuestions();
    }
}
