package com.project.backend.controller;

import com.project.backend.Question;
import com.project.backend.QuestionUpdateRequest;
import com.project.backend.service.QuestionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("question")
@CrossOrigin
public class QuestionController {

    @Autowired
    QuestionService questionService;
    @RequestMapping("allQuestions")
    public List<Question> getAllQuestions(){
        return questionService.getAllQuestions();
    }
    @PostMapping("answer")
    public void updateQuestionAnswer(@RequestBody QuestionUpdateRequest request) {
        questionService.updateQuestionAnswer(request.getId(), request.getSelAns());
    }

    @GetMapping("marks")
    public int questionGetMarks(){
        return questionService.questionGetMarks();
    }

}
