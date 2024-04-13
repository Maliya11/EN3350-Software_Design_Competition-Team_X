package com.project.backend.service;

import com.project.backend.entity.Question;
import com.project.backend.dao.QuestionDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class QuestionService {

    @Autowired
    QuestionDao questionDao;

    //Getting all questions from the database
    public List<Question> getAllQuestions() {
        return questionDao.findAll();
    }
}
