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

    @Autowired
    PlayerDao playerDao;

    @Autowired
    QuestionDao questionDao;

    public boolean authenticatePlayer(String apiKey) {
        boolean isAuthenticated = authenticate(apiKey);
        if (isAuthenticated) {
            addPlayer(apiKey);
            return true;
        }
        else {
            return false;
        }
    }

    private boolean authenticate(String apiKey) {
        // Call MOCK API to validate API key and get token
        // Implement this logic using RestTemplate or WebClient
        // Return true if authentication is successful, false otherwise
        RestTemplate restTemplate = new RestTemplate();
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        HttpEntity<String> requestEntity = new HttpEntity<>(apiKey, headers);

        String url = "http://20.15.114.131:8080/api/login";
        ResponseEntity<String> responseEntity = restTemplate.postForEntity(url, requestEntity, String.class);

        return responseEntity.getStatusCode().is2xxSuccessful();

    }

    public boolean playerStateIdentify(String apiKey) {
        Optional<Player> playerOptional = playerDao.findById(apiKey);
        if (playerOptional.isPresent()){
            Player existingPlayer = playerOptional.get();
            existingPlayer.setPlayerState(1);
            playerDao.save(existingPlayer);
            return true;
        }
        else{
            return authenticatePlayer(apiKey);
        }
    }

    public void addPlayer(String apiKey){
        Player newPlayer = new Player();
        newPlayer.setApiKey(apiKey);
        newPlayer.setMarks(0);
        newPlayer.setCurrentQuestion(0);
        newPlayer.setPlayerState(1);
        playerDao.save(newPlayer);
    }

    public void setPlayerStatesToZero() {
        List<Player> players = playerDao.findAll();
        for(Player player : players){
            player.setPlayerState(0);
            playerDao.save(player);
        }
    }

    public void playerAnswerSubmit(Integer qNum, Integer selAns) {
        Player currentPlayer = new Player();
        currentPlayer = identifyActivePlayer();
        Optional<Question> questionOptional = questionDao.findById(qNum);

        if(questionOptional.isPresent()){
            Question question = questionOptional.get();

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

    public Player identifyActivePlayer() {
        List<Player> players = playerDao.findAll();
        for (Player player : players){
            if (player.getPlayerState() == 1){
                return player;
            }
        }
        return null;
    }

    public void incrementCurrentQuestion(Player player) {
        Integer currentQuestion = player.getCurrentQuestion();
        Integer nextQuestion = currentQuestion++;
        player.setCurrentQuestion(nextQuestion);
        playerDao.save(player);
    }
}
