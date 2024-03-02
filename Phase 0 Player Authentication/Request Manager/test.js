import HTTPRequestManager from './req_manager.js';

const apiKey = 'NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNiOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMQ';
const requestManager = new HTTPRequestManager(apiKey);

async function initializeGame() {
  const isAuthenticated = await requestManager.authenticate();
  if (isAuthenticated) {
    console.log('Successfully authenticated');
    let data = await requestManager.makeRequest('http://20.15.114.131:8080/api/user/profile/view', 'GET'); 
    console.log(data.user);
  } 
  else {
    console.log('Failed to authenticate');
  }
}

initializeGame();