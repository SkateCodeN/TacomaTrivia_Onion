import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './app/App.jsx';

// Mantine styles
import '@mantine/core/styles.css';
import '@mantine/notifications/styles.css';

// we installed Auth0 library
import { Auth0Provider } from '@auth0/auth0-react';
//console.log("this is the origin page", window.location.origin )
createRoot(document.getElementById('root')).render(
  <Auth0Provider
  domain={""}
  clientId={""}
  authorizationParams={{
    redirect_uri: window.location.origin + "/auth/callback",
    audience:"https://tacomatrivia.com/authorizeUser",
    scope: "openid profile email read:trivia write:trivia"
  }}
  
  >
    
    <App />
  
  </Auth0Provider>
);
