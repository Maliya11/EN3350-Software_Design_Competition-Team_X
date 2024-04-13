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

    //Saving the selected answer in the database
//    public void updateQuestionAnswer(Integer id,Integer selAns) {
//        Question question = questionDao.findById(id).get();
//        question.setSelAns(selAns);
//        questionDao.save(question);
//    }

    //Calculating the user's marks from the database comparing corAns and selAns
//    public int questionGetMarks() {
//        List<Question> questions = questionDao.findAll();
//        int marks = 0;
//        for(Question question : questions){
//            if(question.getSelAns() != null && question.getSelAns().equals(question.getCorAns())){
//                marks++;
//            }
//        }
//        return marks;
//    }
}
