package com.project.backend.controller;

import com.project.backend.entity.Question;
import com.project.backend.service.QuestionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
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
    public ResponseEntity<List<Question>> getAllQuestions(){
        try {
            // Retrieve all questions from the service
            List<Question> questions = questionService.getAllQuestions();
            // Return the questions in the response body
            return ResponseEntity.ok(questions);
        }catch (Exception e){
            // If an error occurs, return an HTTP 500 Internal Server Error response
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(null);
        }
    }
}
