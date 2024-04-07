package com.project.backend.dao;

import com.project.backend.Question;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;


@Repository
public interface QuestionDao extends JpaRepository<Question, Integer> {


}
