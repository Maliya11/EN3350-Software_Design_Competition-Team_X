package com.project.backend.controller;

import com.project.backend.entity.Player;
import com.project.backend.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("player")
@CrossOrigin
public class PlayerController {

    // Autowiring PlayerService to interact with player-related operations
    @Autowired
    PlayerService playerService;

    // Endpoint to authenticate players from Unity
    @PostMapping("/authenticate")
    public ResponseEntity<Map<String,Object>> playerStateIdentify(@RequestBody Map<String, String> requestBody){
        try {
            // Set all player states to zero (not playing)
            playerService.setPlayerStatesToZero();

            // Get the apiKey from the request body
            String apiKey = requestBody.get("apiKey");

            // Identify or register player and set player state as active (playing)
            boolean validKey = playerService.playerStateIdentify(apiKey);
            Player activePlayer = playerService.identifyActivePlayer();

            // Prepare response to Unity containing whether the apiKey is valid and number of completed questions
            Integer completedQuestions = activePlayer.getCompletedQuestions();
            Map<String, Object> response = new HashMap<>();
            response.put("validKey", validKey);
            response.put("completedQuestions", completedQuestions);
            return ResponseEntity.ok(response);
        }catch (Exception e){
            // Handle any exception and return an error response
            Map<String, Object> errorResponse = new HashMap<>();
            errorResponse.put("error", "An error occurred while processing the request.");
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(errorResponse);
        }
    }

    // Endpoint to retrieve details of the current player
    @GetMapping("/details")
    public ResponseEntity<Player> sendPlayerDetails(){
        try {
            //Retrieve details of the active player
            Player activePlayer = playerService.identifyActivePlayer();
            return ResponseEntity.ok(activePlayer);
        }catch (Exception e){
            // Handle any exception and return an error response
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(null);
        }
    }


    // Endpoint to submit the selected answer from frontend
    @PostMapping("/answer")
    public ResponseEntity<Void> playerAnswerSubmit(@RequestBody AnswerUpdateRequest request){
        try {
            //Identify the active player
            Player player = playerService.identifyActivePlayer();

            //Save the submitted answer in the player table
            playerService.playerAnswerSubmit(request.getqNum(), request.getSelAns());

            //Increment the completed questions of the player by one
            playerService.incrementCompletedQuestion(player);
            return ResponseEntity.ok().build();
        }catch (Exception e){
            // Handle any exception and return an error response
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).build();
        }
    }

    // Endpoint to set the bonus given state of the player
    @PostMapping("/bonus")
    public ResponseEntity<Void> playerBonusGiven(@RequestBody Map<String, Integer> requestBody) {
        try {
            // Extract bonusGiven value from request body
            Integer bonusGiven = requestBody.get("bonusGiven");

            // Update the player in the database
            playerService.playerBonusGiven(bonusGiven);
            return ResponseEntity.ok().build();
        }catch (Exception e){
            // Handle any exception and return an error response
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).build();
        }
    }
}
