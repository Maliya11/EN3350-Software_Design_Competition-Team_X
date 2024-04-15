package com.project.backend.controller;

import com.project.backend.entity.Player;
import com.project.backend.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("player")
@CrossOrigin
public class PlayerController {

    @Autowired
    PlayerService playerService;

    @PostMapping("/authenticate")
    public Map<String,Object> playerStateIdentify(@RequestBody Map<String, String> requestBody){
        playerService.setPlayerStatesToZero();
        String apiKey = requestBody.get("apiKey");
        boolean validKey = playerService.playerStateIdentify(apiKey);
        Player activePlayer = playerService.identifyActivePlayer();
        Integer completedQuestions = activePlayer.getCompletedQuestions();
        Map<String, Object> response = new HashMap<>();
        response.put("validKey", validKey);
        response.put("completedQuestions", completedQuestions);
        return response;
    }

    @GetMapping("/details")
    public Player sendPlayerDetails(){
        return playerService.identifyActivePlayer();
    }


    @PostMapping("/answer")
    public void playerAnswerSubmit(@RequestBody AnswerUpdateRequest request){
        Player player = playerService.identifyActivePlayer();
        playerService.playerAnswerSubmit(request.getqNum(), request.getSelAns());
        playerService.incrementCompletedQuestion(player);
    }
}
