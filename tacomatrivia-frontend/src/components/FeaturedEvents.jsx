// src/components/FeaturedEvents.jsx
import { Title, SimpleGrid } from '@mantine/core';
import EventCard from './EventCard';

export default function FeaturedEvents() {
  return (
    <>
      <Title order={2} mb="lg">
        Upcoming Trivia Events
      </Title>

      <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }} spacing="lg">
        <EventCard />
        <EventCard />
        <EventCard />
      </SimpleGrid>
    </>
  );
}