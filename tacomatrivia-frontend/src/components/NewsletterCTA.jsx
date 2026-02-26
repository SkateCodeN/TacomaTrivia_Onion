// src/components/NewsletterCTA.jsx
import { Container, Title, Text, TextInput, Button, Group } from '@mantine/core';

export default function NewsletterCTA() {
  return (
    <Container size="sm" py={80} ta="center">
      <Title order={2}>Never Miss a Trivia Night</Title>
      <Text mb="lg">
        Get weekly Tacoma trivia updates straight to your inbox.
      </Text>

      <Group justify="center">
        <TextInput placeholder="Your email" radius="xl" />
        <Button radius="xl" color="brand.8">
          Subscribe
        </Button>
      </Group>
    </Container>
  );
}