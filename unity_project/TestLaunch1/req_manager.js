class HTTPRequestManager {
  constructor(apiKey) {
    this.apiKey = apiKey;
  }

  async authenticate() {
    let jwtToken = null;
    try {
      const response = await fetch('http://20.15.114.131:8080/api/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ apiKey: this.apiKey })
      });

      const data = await response.json();
      if (response.ok) {
        jwtToken = data.token;
        return jwtToken;
      } else {
        throw new Error(data.message);
      }
    } catch (error) {
      console.error('Authentication failed:', error);
      return jwtToken;
    }
  }

  async makeRequest(jwtToken, url, method = 'GET', body = null) {
    const headers = {
      'Content-Type': 'application/json'
    };

    if (jwtToken) {
      headers['Authorization'] = `Bearer ${jwtToken}`;
    }

    const requestOptions = {
      method: method,
      headers: headers,
    };

    if (body) {
      requestOptions.body = JSON.stringify(body);
    }

    try {
      const response = await fetch(url, requestOptions);
      const responseData = await response.json();

      if (!response.ok) {
        throw new Error(responseData.message);
      }

      return responseData;
    } catch (error) {
      console.error('Request failed:', error);
      return null;
    }
  }
}

export default HTTPRequestManager;