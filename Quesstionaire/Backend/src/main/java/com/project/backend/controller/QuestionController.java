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

    // Autowiring QuestionService to interact with question-related operations
    @Autowired
    QuestionService questionService;

    // Endpoint to retrieve all questions from the database in JSON format
    @RequestMapping("allQuestions")
    public List<Question> getAllQuestions(){
        return questionService.getAllQuestions();
    }
}
