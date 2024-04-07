package com.project.backend.dao;

import com.project.backend.Question;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

@Repository
public interface QuestionDao extends JpaRepository<Question, Integer> {


    @Transactional
    @Modifying
    @Query("UPDATE Question q SET q.selAns = :selAns WHERE q.id = :id")
    void updateQuestionAnswer(Integer id, Integer selAns);
}
