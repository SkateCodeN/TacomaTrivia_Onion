// src/components/EventCard.jsx
import { Card, Text, Badge, Group, Button } from '@mantine/core';
import styles from '../styles/EventCard.module.css';

export default function EventCard() {
  return (
    <Card shadow="md" radius="lg" padding="lg" className={styles.card}>
      <Group justify="space-between" mb="sm">
        <Text fw={600}>The Office Trivia Night</Text>
        <Badge color="brand.6" variant="light">
          Themed
        </Badge>
      </Group>

      <Text size="sm" c="dimmed">
        Tuesday • 7:00 PM
      </Text>

      <Text size="sm" mt="sm">
        Hosted at a local Tacoma pub. Win prizes and compete with friends!
      </Text>

      <Button fullWidth mt="md" radius="xl">
        View Details
      </Button>
    </Card>
  );
}