package com.project.backend.controller;

import com.project.backend.entity.Question;
import com.project.backend.service.QuestionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("question")
@CrossOrigin(origins = "http://localhost:5173/")
public class QuestionController {

    //This is the controller class where http requests are handled

    @Autowired
    QuestionService questionService;

    //To get all the questions from the database in JASON format
    @RequestMapping("allQuestions")
    public List<Question> getAllQuestions(){
        return questionService.getAllQuestions();
    }

    //To store the selected answer in the database
    // {"id":2, "selAns":3}
//    @PostMapping("answer")
//    public void updateQuestionAnswer(@RequestBody QuestionUpdateRequest request) {
//        questionService.updateQuestionAnswer(request.getId(), request.getSelAns());
//    }

    //To calculate marks using the stored values in the database
//    @GetMapping("marks")
//    public int questionGetMarks(){
//        return questionService.questionGetMarks();
//    }



}
