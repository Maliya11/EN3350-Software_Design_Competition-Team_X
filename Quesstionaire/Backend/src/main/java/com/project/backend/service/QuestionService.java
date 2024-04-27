package com.project.backend.service;

import com.project.backend.entity.Question;
import com.project.backend.dao.QuestionDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class QuestionService {

    // Autowiring QuestionDao to interact with question-related data
    @Autowired
    QuestionDao questionDao;

    // Method to retrieve all questions from the database
    public List<Question> getAllQuestionsSortedById() {
        try {
            // Define sorting based on the id column
            Sort sort = Sort.by(Sort.Direction.ASC, "id");
            // Retrieve all questions from the database
            return questionDao.findAll(sort);
        }catch (Exception e){
            // Handle exceptions
            throw new RuntimeException("An error occurred while retrieving questions.", e);
        }
    }
}
