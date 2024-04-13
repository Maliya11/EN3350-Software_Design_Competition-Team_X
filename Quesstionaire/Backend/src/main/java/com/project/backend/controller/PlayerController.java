package com.project.backend.controller;

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
    public String authenticatePlayer(@RequestBody String apiKey) {
        // Call method to send API key to MOCK API and authenticate player
        return playerService.authenticatePlayer(apiKey);
    }





}
