package com.project.backend.service;

import com.project.backend.dao.PlayerDao;
import com.project.backend.dao.QuestionDao;
import com.project.backend.entity.Player;
import com.project.backend.entity.Question;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import org.springframework.http.*;

import java.util.List;
import java.util.Optional;

@Service
public class PlayerService {

    // Autowiring PlayerDao to interact with Player entities
    @Autowired
    PlayerDao playerDao;

    // Autowiring QuestionDao to interact with Question entities
    @Autowired
    QuestionDao questionDao;

    // Method to authenticate player using API key
    public boolean authenticatePlayer(String apiKey) {
        // Authenticate player using provided API key
        boolean isAuthenticated = authenticate(apiKey);
        if (isAuthenticated) {
            // Add player if authentication is successful
            addPlayer(apiKey);
            return true;
        }
        else {
            // Return false if authentication fails
            return false;
        }
    }

    // Method to authenticate player using API key by calling external service
    private boolean authenticate(String apiKey) {
        // Create JSON object with the apiKey
        String jsonBody = "{\"apiKey\":\"" + apiKey + "\"}";

        // Using RestTemplate to make HTTP requests
        RestTemplate restTemplate = new RestTemplate();
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);

        // Create HTTP entity with JSON body and headers
        HttpEntity<String> requestEntity = new HttpEntity<>(jsonBody, headers);

        // Specify the URL of the API endpoint
        String url = "http://20.15.114.131:8080/api/login";
        ResponseEntity<String> responseEntity = restTemplate.postForEntity(url, requestEntity, String.class);

        // Check if the response status code is 2xx (indicating success)
        return responseEntity.getStatusCode().is2xxSuccessful();

    }

    // Method to identify player state
    public boolean playerStateIdentify(String apiKey) {
        // Check if player exists in database
        Optional<Player> playerOptional = playerDao.findById(apiKey);
        if (playerOptional.isPresent()){
            // If player exists, set player state to active
            Player existingPlayer = playerOptional.get();
            existingPlayer.setPlayerState(1);
            playerDao.save(existingPlayer);
            return true;
        }
        else{
            // If player does not exist, authenticate player
            return authenticatePlayer(apiKey);
        }
    }

    // Method to add a new player
    public void addPlayer(String apiKey){
        // Create a new player object and save it to the database
        Player newPlayer = new Player();
        newPlayer.setApiKey(apiKey);
        newPlayer.setMarks(0);
        newPlayer.setCompletedQuestions(0);
        newPlayer.setPlayerState(1);
        playerDao.save(newPlayer);
    }

    // Method to set player states to zero
    public void setPlayerStatesToZero() {
        // Set player state to inactive for all players
        List<Player> players = playerDao.findAll();
        for(Player player : players){
            player.setPlayerState(0);
            playerDao.save(player);
        }
    }

    // Method to submit player's answer for a question
    public void playerAnswerSubmit(Integer qNum, Integer selAns) {
        // Identify the active player
        Player currentPlayer = identifyActivePlayer();
        Optional<Question> questionOptional = questionDao.findById(qNum);

        if(questionOptional.isPresent()){
            Question question = questionOptional.get();

            // Switch statement to handle different questions
        switch (qNum) {
            case 1:
                currentPlayer.setQ1_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 2:
                currentPlayer.setQ2_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 3:
                currentPlayer.setQ3_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 4:
                currentPlayer.setQ4_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 5:
                currentPlayer.setQ5_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 6:
                currentPlayer.setQ6_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 7:
                currentPlayer.setQ7_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 8:
                currentPlayer.setQ8_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 9:
                currentPlayer.setQ9_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
            case 10:
                currentPlayer.setQ10_ans(selAns);
                if (selAns.equals(question.getCorAns())) {
                    currentPlayer.setMarks(currentPlayer.getMarks()+1);
                }
                playerDao.save(currentPlayer);
                break;
        }
        }
    }

    // Method to identify active player
    public Player identifyActivePlayer() {
        // Get list of all players and identify the one with active state
        List<Player> players = playerDao.findAll();
        for (Player player : players){
            if (player.getPlayerState() == 1){
                return player;
            }
        }
        return null;
    }

    // Method to increment completed questions count for a player
    public void incrementCompletedQuestion(Player player) {
        // Increment completed questions count for the specified player
        player.setCompletedQuestions(player.getCompletedQuestions()+1);
        playerDao.save(player);
    }

    // Method to record bonus given to player
    public void playerBonusGiven(Integer bonusGiven) {
        // Identify the active player
        Player player = identifyActivePlayer();
        // Set bonus given for the player
        player.setBonusGiven(bonusGiven);
        playerDao.save(player);
    }
}
