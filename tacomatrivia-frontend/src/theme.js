// src/theme.js
import { createTheme } from '@mantine/core';

export const theme = createTheme({
  fontFamily: 'Lexend, sans-serif',

  colors: {
    brand: [
      '#fdf3e6',
      '#f9e2c7',
      '#f5d1a7',
      '#f1c088',
      '#eeaf61', // primary light accent
      '#fb9062',
      '#ee5d6c',
      '#ce4993',
      '#6a0d83', // deep purple (safe for white text)
      '#4a065d'
    ]
  },

  primaryColor: 'brand',

  headings: {
    fontWeight: '700'
  }
});