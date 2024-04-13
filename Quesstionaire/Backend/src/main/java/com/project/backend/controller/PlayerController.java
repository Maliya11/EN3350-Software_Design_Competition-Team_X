package com.project.backend.controller;

import com.project.backend.entity.Player;
import com.project.backend.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;

@RestController
@RequestMapping("player")
@CrossOrigin
public class PlayerController {

    @Autowired
    PlayerService playerService;

    @PostMapping("/authenticate")
    public boolean playerStateIdentify(@RequestHeader String apiKey){
        playerService.setPlayerStatesToZero();
        return playerService.playerStateIdentify(apiKey);
    }

    @GetMapping("/details")
    public Player sendPlayerDetails(){
        return playerService.identifyActivePlayer();
    }

    @PostMapping("/answer")
    public void playerAnswerSubmit(@RequestHeader Integer qNum, @RequestHeader Integer selAns ){
        Player player = playerService.identifyActivePlayer();
        playerService.playerAnswerSubmit(qNum, selAns);
        playerService.incrementCurrentQuestion(player);
    }

}
