package com.project.backend.service;

import com.project.backend.Question;
import com.project.backend.dao.QuestionDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class QuestionService {

    @Autowired
    QuestionDao questionDao;
    public List<Question> getAllQuestions() {
        return questionDao.findAll();
    }

    public void updateQuestionAnswer(Integer id,Integer selAns) {
        questionDao.updateQuestionAnswer(id, selAns);
    }

    public int questionGetMarks() {
        List<Question> questions = questionDao.findAll();
        int marks = 0;
        for(Question question : questions){
            if(question.getSelAns() != null && question.getSelAns().equals(question.getCorAns())){
                marks++;
            }
        }
        return marks;
    }

}
