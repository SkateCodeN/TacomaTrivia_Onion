import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { theme } from './theme.js';
import App from './app/App.jsx';

// Mantine styles
import '@mantine/core/styles.css';
import '@mantine/notifications/styles.css';
import { MantineProvider } from '@mantine/core';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <MantineProvider theme={theme} withGlobalStyles withNoralizeCSS>
      <App />
    </MantineProvider>
    
  </StrictMode>
);
