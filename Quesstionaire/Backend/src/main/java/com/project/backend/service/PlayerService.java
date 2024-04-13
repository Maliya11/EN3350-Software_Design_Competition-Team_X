package com.project.backend.service;

import org.springframework.http.MediaType;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import org.springframework.http.*;

@Service
public class PlayerService {

    public String authenticatePlayer(String apiKey) {
        boolean isAuthenticated = authenticate(apiKey);
        if (isAuthenticated) {
            return "Player authenticated successfully";
        } else {
            return "Authentication failed";
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
}
