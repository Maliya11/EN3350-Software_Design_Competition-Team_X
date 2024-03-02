class HTTPRequestManager {
  constructor(apiKey) {
    this.apiKey = apiKey;
    this.jwtToken = null;
  }

  async authenticate() {
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
        this.jwtToken = data.token;
        handleTokenResponse(data.token);
        return true;
      } else {
        throw new Error(data.message);
      }
    } catch (error) {
      console.error('Authentication failed:', error);
      return false;
    }
  }

  async makeRequest(url, method = 'GET', body = null) {
    const headers = {
      'Content-Type': 'application/json'
    };

    if (this.jwtToken) {
      headers['Authorization'] = `Bearer ${this.jwtToken}`;
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