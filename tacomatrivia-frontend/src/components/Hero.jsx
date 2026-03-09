// src/components/Hero.jsx
import { Container, Title, Text, Button, Stack } from '@mantine/core';
import styles from '../styles/Hero.module.css';
import { Link } from 'react-router-dom';
const handleClick =() =>
{

}
export default function Hero() {
  return (
    <div className={styles.hero}>
      <Container size="md">
        <Stack align="center" gap="lg">
          <Title order={1} className={styles.title}>
            Discover Trivia Nights in Tacoma
          </Title>
          <Text size="lg" className={styles.subtitle}>
            Find the best pub quizzes, themed trivia, and weekly competitions happening around Tacoma.
          </Text>
          <Button size="lg" radius="xl" variant="white" color="brand.8" >
            <Link to="venues" className={styles.link}>Explore Venues</Link>
          </Button>
        </Stack>
      </Container>
    </div>
  );
}