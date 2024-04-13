package com.project.backend.controller;

import com.project.backend.entity.Player;
import com.project.backend.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("player")
@CrossOrigin
public class PlayerController {

    @Autowired
    PlayerService playerService;

    @PostMapping("/authenticate")
    public boolean playerStateIdentify(@RequestBody String apiKey){
        playerService.setPlayerStatesToZero();
        return playerService.playerStateIdentify(apiKey);
    }

    @GetMapping("/details")
    public Player sendPlayerDetails(){
        return playerService.identifyActivePlayer();
    }


    @PostMapping("/answer")
    public void playerAnswerSubmit(@RequestBody AnswerUpdateRequest request){
        Player player = playerService.identifyActivePlayer();
        playerService.playerAnswerSubmit(request.getqNum(), request.getSelAns());
        playerService.incrementCurrentQuestion(player);
    }

}
