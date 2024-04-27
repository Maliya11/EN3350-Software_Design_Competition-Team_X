package com.project.backend.dao;

import com.project.backend.entity.Player;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PlayerDao extends JpaRepository<Player, String> {
    //Accessing the player table in the database
}
