// src/pages/Home.jsx
import { Container, Stack } from '@mantine/core';
import Hero from '../components/Hero';
import FeaturedEvents from '../components/FeaturedEvents';
import HelpCTA from '../components/HelpCTA';
import NewsletterCTA from '../components/NewsletterCTA';

export default function Home() {
  return (
    <Stack gap={80}>
      <Hero />
      <Container size="lg">
        <FeaturedEvents />
      </Container>
      <HelpCTA />
      <NewsletterCTA />
    </Stack>
  );
}