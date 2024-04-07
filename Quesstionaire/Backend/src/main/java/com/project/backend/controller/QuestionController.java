package com.project.backend.controller;

import com.project.backend.Question;
import com.project.backend.service.QuestionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

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
    @GetMapping("answer")
    public void updateQuestionAnswer(Integer id,Integer corAns){
        questionService.updateQuestionAnswer(id, corAns);
    }
    @GetMapping("marks")
    public int questionGetMarks(){
        return questionService.questionGetMarks();
    }

}
